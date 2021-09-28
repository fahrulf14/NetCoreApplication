using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NUNA.Helpers
{
    [HtmlTargetElement("*", Attributes = "asp-displayfor")]
    public class DisplayNameTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-displayfor")]
        public ModelExpression PropertyName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PropertyName.Metadata.DisplayName != null)
            {
                output.Content.SetContent(PropertyName.Metadata.DisplayName);
            }
            else
            {
                output.Content.SetContent(PropertyName.Name);
            }
            base.Process(context, output);
        }
    }
}
