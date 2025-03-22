namespace ARSounds.UI.Common;

public class TestimonialModel
{
    public string Avatar { get; }

    public string UserName { get; }

    public string Professional { get; }

    public double Rating { get; }

    public string Comment { get; }

    public string ImageUrl { get; }

    public TestimonialModel(string avatar, string userName, string professional, double rating, string comment, string imageUrl)
    {
        Avatar = avatar;
        UserName = userName;
        Professional = professional;
        Rating = rating;
        Comment = comment;
        ImageUrl = imageUrl;
    }
}
