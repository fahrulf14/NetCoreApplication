using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NUNA.Helpers
{
    [HtmlTargetElement("*", Attributes = "asp-authorized")]
    public class AuthorizeTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-authorized")]
        public string Permission { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!PermissionHelper.IsAuthorized(Permission))
            {
                output.Attributes.Clear();
                output.Content.Clear();
                output.Attributes.SetAttribute("style", $"display: none;");
            }
        }
    }
}
