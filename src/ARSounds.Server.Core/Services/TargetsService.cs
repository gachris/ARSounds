using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Enums;
using ARSounds.Server.Core.Filters;
using ARSounds.Server.Core.Properties;
using ARSounds.Server.Core.Requests;
using ARSounds.Server.Core.Utils;
using ARSounds.Server.EntityFramework.DbContexts;
using ARSounds.Server.EntityFramework.Entities;
using AutoMapper;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenVision.Api.Auth;
using OpenVision.Api.Core;
using OpenVision.Api.Core.Types;
using OpenVision.Api.Target.Resources;
using OpenVision.Api.Target.Services;
using OpenVision.Shared;
using OpenVision.Shared.Requests;
using OpenVision.Shared.Responses;
using TargetResponse = ARSounds.Server.Core.Responses.TargetResponse;
using UpdateTargetRequest = ARSounds.Server.Core.Requests.UpdateTargetRequest;

namespace ARSounds.Server.Core.Services;

public class TargetsService : ITargetsService
{
    private readonly ApplicationDbContext _applicationContext;
    private readonly IUriService _uriService;
    private readonly IMapper _mapper;
    private readonly HttpContext _httpContext;
    private readonly TargetService _service;
    private readonly TargetListResource _resource;

    public TargetsService(
        ApiConfiguration apiConfiguration,
        ApplicationDbContext applicationContext,
        IHttpContextAccessor httpContextAccessor,
        IUriService uriService,
        IMapper mapper)
    {
        _applicationContext = applicationContext;
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _uriService = uriService;
        _mapper = mapper;

        _service = new TargetService(new BaseClientService.Initializer()
        {
            ApplicationName = apiConfiguration.ApiName,
            HttpClientInitializer = new UserCredential(new DatabaseApiKey(apiConfiguration.VisionApiKey)),
            ServerUrl = "https://localhost:44320"
        });

        _resource = new TargetListResource(_service);
    }

    public async Task<IPagedResponse<IEnumerable<TargetResponse>>> GetAsync(TargetBrowserQuery query, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var route = _httpContext.Request.Path.Value;
        route.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.RouteNotFound);

        var validFilter = new PaginationFilter(query.Page, query.Size);
        var take = validFilter.Size;
        var skip = validFilter.Page - 1;

        var targets = await _applicationContext.Target.Where(x => x.UserId == userId && (string.IsNullOrEmpty(query.Description) || x.Description.Contains(query.Description)))
            .Include(a => a.Audio)
            .Include(a => a.Image)
            .OrderBy(x => x.Created)
            .Skip(skip * take)
            .Take(take)
            .ToListAsync(cancellationToken);

        var totalRecords = await _applicationContext.Target.CountAsync(cancellationToken);
        var result = targets.Select(_mapper.Map<TargetResponse>);

        var totalPages = totalRecords / (double)validFilter.Size;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        var nextPage =
            validFilter.Page >= 1 && validFilter.Page < roundedTotalPages
            ? _uriService.GetPageUri(new PaginationFilter(validFilter.Page + 1, validFilter.Size), route)
            : null;
        var previousPage =
            validFilter.Page - 1 >= 1 && validFilter.Page <= roundedTotalPages
            ? _uriService.GetPageUri(new PaginationFilter(validFilter.Page - 1, validFilter.Size), route)
            : null;
        var firstPage = _uriService.GetPageUri(new PaginationFilter(1, validFilter.Size), route);
        var lastPage = _uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.Size), route);

        var response = new PagedResponse<IEnumerable<TargetResponse>>(
            validFilter.Page,
            validFilter.Size,
            firstPage,
            lastPage,
            roundedTotalPages,
            totalRecords,
            nextPage,
            previousPage,
            new ResponseDoc<IEnumerable<TargetResponse>>(result),
            Guid.NewGuid(),
            StatusCode.Success,
            new ReadOnlyCollection<Error>([]));

        return response;
    }

    public async Task<IResponseMessage<TargetResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = await _applicationContext.Target.Where(x => x.Id == id && x.UserId == userId)
                                                     .SingleOrDefaultAsync(cancellationToken);

        target.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.TargetNotFound);

        await _applicationContext.Entry(target)
                                 .Reference(x => x.Audio)
                                 .LoadAsync(cancellationToken);

        target.Audio.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.AudioNotFound);

        await _applicationContext.Entry(target)
                                 .Reference(x => x.Image)
                                 .LoadAsync(cancellationToken);

        var targetResponse = _mapper.Map<TargetResponse>(target);

        return Success(targetResponse);
    }

    public async Task<IResponseMessage<Guid>> Create(CreateTargetRequest body, CancellationToken cancellationToken)
    {
        var audioId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        var route = _httpContext.Request.Path.Value;
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = new Target
        {
            Id = targetId,
            Description = body.Description,
            UserId = userId,
            HexColor = "#000000",
            Metadata = "",
            AudioId = audioId,
            Created = DateTime.Now,
            Updated = DateTime.Now,
        };

        var audio = new Audio
        {
            Id = audioId,
            Filename = body.Filename,
            AudioType = body.AudioType,
            FileExtension = Path.GetExtension(body.Filename),
            Created = DateTime.Now,
            Updated = DateTime.Now,
        };

        var audioBase64 = body.AudioBase64;
        if (audioBase64.Contains(',')) audioBase64 = audioBase64[(body.AudioBase64.IndexOf(",") + 1)..];
        audio.AudioBytes = Convert.FromBase64String(audioBase64);
        target.Audio = audio;

        await _applicationContext.Target.AddAsync(target, cancellationToken);
        await _applicationContext.SaveChangesAsync(cancellationToken);

        return Success(targetId);
    }

    public async Task<IResponseMessage> Edit(Guid id, UpdateTargetRequest body, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = await _applicationContext.Target.Where(x => x.Id == id && x.UserId == userId)
                                                     .SingleOrDefaultAsync(cancellationToken);

        target.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.TargetNotFound);

        await _applicationContext.Entry(target)
                                 .Reference(x => x.Image)
                                 .LoadAsync(cancellationToken);

        target.Image.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);

        var route = _httpContext.Request.Path.Value;

        target.Description = body.Description ?? target.Description;
        target.HexColor = body.HexColor ?? target.HexColor;

        if (target.IsActive && target.IsTrackable != body.IsTrackable)
        {
            target.IsTrackable = body.IsTrackable ?? false;

            var updateTrackableRequest = new UpdateTrackableRequest
            {
                ActiveFlag = target.IsTrackable ? ActiveFlag.True : ActiveFlag.False,
            };

            target.Image.VisionTargetId.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);

            var updateTrackableResponse = await _resource.Update(updateTrackableRequest, target.Image.VisionTargetId.ToString()!).ExecuteAsync(cancellationToken);

            if (updateTrackableResponse.StatusCode is StatusCode.Failed)
            {
                return new ResponseMessage(Guid.NewGuid(), StatusCode.Failed, updateTrackableResponse.Errors);
            }
        }

        await _applicationContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<IResponseMessage> Activate(Guid id, ActivateTargetRequest body, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = await _applicationContext.Target.Where(x => x.Id == id && x.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        target.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.TargetNotFound);
        target.IsActive.ThrowIfTrue(ResultCode.InvalidRequest, ErrorMessages.TargetAlreadyActive);

        var imageId = Guid.NewGuid();
        var route = _httpContext.Request.Path.Value;

        target.ImageId = imageId;
        target.Image = new Image()
        {
            Id = imageId,
            Created = DateTime.Now,
            Updated = DateTime.Now
        };

        var imageBytes = body.PngBase64!.Base64ImgToByteArray(ImageType.Png);

        // Decode the original PNG with alpha
        var matImage = new Mat();
        CvInvoke.Imdecode(imageBytes, ImreadModes.Unchanged, matImage);

        // ----------- JPG and PNG without alpha ------------
        var matModelImage = new Mat(matImage.Size, DepthType.Cv8U, 3);
        matModelImage.SetTo(new MCvScalar(255, 255, 255));

        // Get alpha channel
        var alpha = new Mat();
        CvInvoke.ExtractChannel(matImage, alpha, 3);

        // Create binary mask from alpha
        var mask = new Mat();
        CvInvoke.Threshold(alpha, mask, 0, 255, ThresholdType.Binary);

        // Apply black background on the masked area
        matModelImage.SetTo(new MCvScalar(0, 0, 0), mask);

        // JPG output (no alpha)
        var bufferWithoutAlphaPng = CvInvoke.Imencode(".png", matModelImage);
        var pngBase64WithoutAlpha = Convert.ToBase64String(bufferWithoutAlphaPng);

        // Decode the JPEG into a Mat (3 channels, no alpha)
        var matJpeg = new Mat();
        CvInvoke.Imdecode(bufferWithoutAlphaPng, ImreadModes.Color, matJpeg); // color only

        // Create mask: detect near-white pixels (tweak the threshold if needed)
        var lowerWhite = new ScalarArray(new MCvScalar(240, 240, 240));
        var upperWhite = new ScalarArray(new MCvScalar(255, 255, 255));
        var whiteMask = new Mat();
        CvInvoke.InRange(matJpeg, lowerWhite, upperWhite, whiteMask);

        // Invert mask to get non-white as opaque
        var alphaMask = new Mat();
        CvInvoke.BitwiseNot(whiteMask, alphaMask);

        // Add alpha channel to original JPEG Mat
        var matWithAlpha = new Mat();
        CvInvoke.CvtColor(matJpeg, matWithAlpha, ColorConversion.Bgr2Bgra);

        // Replace alpha channel with our generated mask
        CvInvoke.InsertChannel(alphaMask, matWithAlpha, 3); // 3 = alpha channel index

        // Encode to PNG with new alpha
        var bufferPngWithAlpha = CvInvoke.Imencode(".png", matWithAlpha);
        var descriptionBase64 = string.Concat(target.Description, DateTime.Now.ToString("yyyyMMddHHmmssfff")).Base64Encode();
        var metadataBase64 = string.Concat(descriptionBase64, "|", DateTime.Now.ToString()).Base64Encode();

        target.Image.Buffer = bufferPngWithAlpha; // With alpha
        target.Metadata = metadataBase64;
        target.HexColor = body.HexColor!;

        await _applicationContext.Image.AddAsync(target.Image, cancellationToken);

        target.IsActive = true;
        target.IsTrackable = true;

        var postTrackableRequest = new PostTrackableRequest
        {
            Name = descriptionBase64,
            Image = pngBase64WithoutAlpha,
            Width = 1,
            ActiveFlag = target.IsTrackable ? ActiveFlag.True : ActiveFlag.False,
            Metadata = metadataBase64
        };

        var visionPostResponse = await _resource.Insert(postTrackableRequest).ExecuteAsync(cancellationToken);

        if (visionPostResponse.StatusCode is StatusCode.Failed)
        {
            return new ResponseMessage(Guid.NewGuid(), StatusCode.Failed, visionPostResponse.Errors);
        }

        target.Image.VisionTargetId = visionPostResponse.Response.Result;

        await _applicationContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<IResponseMessage> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = await _applicationContext.Target.Where(x => x.Id == id && x.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        target.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.TargetNotFound);
        target.IsActive.ThrowIfFalse(ResultCode.InvalidRequest, ErrorMessages.TargetIsNotActive);

        await _applicationContext.Entry(target).Reference(x => x.Image).LoadAsync(cancellationToken);
        target.Image.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);

        var route = _httpContext.Request.Path.Value;

        target.IsTrackable = false;
        target.IsActive = false;

        target.Image.VisionTargetId.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);

        var deleteResponse = await _resource.Delete(target.Image.VisionTargetId.ToString()!).ExecuteAsync(cancellationToken);

        if (deleteResponse.StatusCode is StatusCode.Failed)
        {
            return new ResponseMessage(Guid.NewGuid(), StatusCode.Failed, deleteResponse.Errors);
        }

        _applicationContext.Image.Remove(target.Image);
        await _applicationContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    public async Task<IResponseMessage> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId.ThrowIfNullOrEmpty(ResultCode.InvalidRequest, ErrorMessages.UserNotFound);

        var target = await _applicationContext.Target.Where(x => x.Id == id && x.UserId == userId)
                                                     .SingleOrDefaultAsync(cancellationToken);

        target.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.TargetNotFound);

        await _applicationContext.Entry(target)
                                 .Reference(x => x.Audio)
                                 .LoadAsync(cancellationToken);

        target.Audio.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.AudioNotFound);

        if (target.IsActive)
        {
            target.Image.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);
            target.Image.VisionTargetId.ThrowIfNull(ResultCode.RecordNotFound, ErrorMessages.ImageNotFound);

            await _applicationContext.Entry(target)
                                     .Reference(x => x.Image)
                                     .LoadAsync(cancellationToken);

            var deleteResponse = await _resource.Delete(target.Image.VisionTargetId.ToString()!)
                .ExecuteAsync(cancellationToken);

            if (deleteResponse.StatusCode is StatusCode.Failed)
            {
                return new ResponseMessage(Guid.NewGuid(), StatusCode.Failed, deleteResponse.Errors);
            }

            _applicationContext.Image.Remove(target.Image);
        }

        _applicationContext.Audio.Remove(target.Audio);
        _applicationContext.Target.Remove(target);

        await _applicationContext.SaveChangesAsync(cancellationToken);

        return Success();
    }

    #region Helpers

    /// <summary>
    /// Creates a success response message with no result.
    /// </summary>
    /// <returns>A success response message with no result.</returns>
    protected static IResponseMessage Success()
    {
        return new ResponseMessage(Guid.NewGuid(), StatusCode.Success, new ReadOnlyCollection<Error>(new List<Error>()));
    }

    /// <summary>
    /// Creates a success response message with a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result.</param>
    /// <returns>A success response message with the specified result.</returns>
    protected static IResponseMessage<TResult> Success<TResult>(TResult result)
    {
        return new ResponseMessage<TResult>(new ResponseDoc<TResult>(result), Guid.NewGuid(), StatusCode.Success, new ReadOnlyCollection<Error>(new List<Error>()));
    }

    #endregion
}