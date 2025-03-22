using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

namespace OpenVision.Wpf.Demo.ML.DataModels;

public struct ImageSettings
{
    public const int imageHeight = 416;
    public const int imageWidth = 416;
}

public class ImageInputData
{
    [ImageType(ImageSettings.imageHeight, ImageSettings.imageWidth)]
    public MLImage? Image { get; set; }
}
