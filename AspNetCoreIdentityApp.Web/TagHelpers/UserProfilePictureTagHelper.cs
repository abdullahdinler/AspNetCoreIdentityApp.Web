using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserProfilePictureTagHelper : TagHelper
    {
        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.Attributes.SetAttribute("src",
                string.IsNullOrEmpty(PictureUrl)
                    ? "/images/fe32e999-7655-4280-9421-d79d60f8d96a.jpg"
                    : $"/images/{PictureUrl}");
            output.Attributes.SetAttribute("class", "rounded float-left");

            base.Process(context, output);
        }
    }
}
