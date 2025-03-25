using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ARSounds.UI.Maui.Targets.ViewModels;

public class ProductDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BrandName { get; set; }
    public List<string> ImageUrls { get; set; }
    public string FeaturedImage => ImageUrls.FirstOrDefault();
    public string Price { get; set; }
    public List<string> Sizes { get; set; }
    public List<ReviewModel> Reviews { get; set; }
    public List<Color> ColorLists { get; set; }
    public string Details { get; set; }
}