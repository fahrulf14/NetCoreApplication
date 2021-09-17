using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NUNA.Helpers
{
    [HtmlTargetElement("*", Attributes = "asp-badge")]
    public class BadgeTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-badge")]
        public ModelExpression PropertyName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PropertyName.Name == "IsActive")
            {
                if (PropertyName.Model.ToString() == "True")
                {
                    output.Content.SetHtmlContent(@"<span><span class=""kt-badge kt-badge--success kt-badge--dot""></span>&nbsp;<span class=""kt-font-bold kt-font-success"">Active</span></span>");
                }
                else
                {
                    output.Content.SetHtmlContent(@"<span><span class=""kt-badge kt-badge--danger kt-badge--dot""></span>&nbsp;<span class=""kt-font-bold kt-font-danger"">Inactive</span></span>");
                }
            }
            else
            {
                if (PropertyName.Model.ToString() == "True")
                {
                    output.Content.SetHtmlContent(@"<span><span class=""kt-badge kt-badge--success kt-badge--dot""></span>&nbsp;<span class=""kt-font-bold kt-font-success"">Yes</span></span>");
                }
                else
                {
                    output.Content.SetHtmlContent(@"<span><span class=""kt-badge kt-badge--danger kt-badge--dot""></span>&nbsp;<span class=""kt-font-bold kt-font-danger"">No</span></span>");
                }
            }
            base.Process(context, output);
        }
    }
}
