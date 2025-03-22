using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Targets.ViewModels;

public partial class TargetDetailsViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ProductDetail _productDetail;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FavStatusColor))]
    private bool _isFavorite = false;

    [ObservableProperty]
    private bool _isFooterVisible = false;

    #endregion

    #region Properties

    public Color FavStatusColor => IsFavorite ? Color.FromArgb("#00C569") : Color.FromArgb("#000000");

    #endregion

    public TargetDetailsViewModel(INavigationService navigationService) : base(navigationService)
    {
        _navigationService = navigationService;

    }

    #region Relay Commands

    [RelayCommand]
    private void TapFav(Color obj)
    {
        IsFavorite = true ? !IsFavorite : IsFavorite;
    }

    #endregion

    #region Methods

    public override async Task InitializeAsync(object initParams)
    {
        await Task.Run(() => LoadData());
    }

    private void LoadData()
    {
        ProductDetail = new ProductDetail
        {
            Id = 1,
            Name = "BeoPlay Speaker",
            BrandName = "Bang and Olufsen",
            ImageUrls = new List<string>()
            {
                "product_detail_1.jpg",
                "product_detail_2.jpg",
                "product_detail_3.jpg",
                "product_item_3.jpg",
                "product_item_4.jpg"
            },
            Price = "$755",
            Sizes = new List<string>() { "S", "M", "L", "XL", "XXL" },
            Reviews = new List<ReviewModel>() {
                    new ReviewModel() {
                        ImageUrl = "user1.png",
                        Name = "Samuel Smith",
                        Review = "Wonderful jean, perfect gift for my girl for our anniversary! I love this, paired it with a nice blouse and all eyes on me.",
                        Rating = 4.3,
                        Date = "October 8th 2021"
                    },
                    new ReviewModel() {
                        ImageUrl = "user2.png",
                        Name = "Beth Aida",
                        Review = "I love this, paired it with a nice blouse and all eyes on me. Wonderful jean, perfect gift for my girl for our anniversary!",
                        Rating = 4.0,
                        Date = "August 25th 2021"
                    },
                    new ReviewModel() {
                        ImageUrl = "user1.png",
                        Name = "Samuel Smith",
                        Review = "Wonderful jean, perfect gift for my girl for our anniversary!  I love this, paired it with a nice blouse and all eyes on me.",
                        Rating = 4.5,
                        Date = "November 2nd 2021"
                    },
                    new ReviewModel() {
                        ImageUrl = "user2.png",
                        Name = "Beth Aida",
                        Review = "I love this, paired it with a nice blouse and all eyes on me. Wonderful jean, perfect gift for my girl for our anniversary!",
                        Rating = 5.0,
                        Date = "Feb 15th 2022"
                    }
                },
            ColorLists = new List<Color>() { Color.FromArgb("#1A73E8"), Color.FromArgb("#F7B548"), Color.FromArgb("#FF392B"), Color.FromArgb("#00C569"), Color.FromArgb("#2B0B98") },
            Details = "Nike Dri-FIT is a polyester fabric designed to help you keep dry so you can more comfortably work harder, longer."
        };
    }

    #endregion
}
