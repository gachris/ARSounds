﻿using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Queries;

public record GetTargetsQuery : IRequest<RequestResultDto>;