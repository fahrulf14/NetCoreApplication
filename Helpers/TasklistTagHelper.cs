using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NUNA.Helpers
{
    [HtmlTargetElement("*", Attributes = "list-length")]
    public class TasklistLengthTagHelper : TagHelper
    {
        [HtmlAttributeName("list-length")]
        public string NumberString { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ListNumber = NumberString.Split(",");
            output.Content.AppendHtml(@"<div class=""kt-form__group""><select class=""form-control kt-selectpicker"" id=""length"">");
            foreach (var item in ListNumber)
            {
                output.Content.AppendFormat(@"<option value=""{0}"">{0}</option>", item);
            }
            output.Content.AppendHtml(@"</select></div>");
            
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("*", Attributes = "list-search")]
    public class TasklistSearchTagHelper : TagHelper
    {
        [HtmlAttributeName("list-search")]
        public string SearchTerm { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetHtmlContent(@"<div class=""kt-input-icon kt-input-icon--left"">
                                        <input type=""text"" class=""form-control"" placeholder=""Search..."" id=""search"">
                                        <span class=""kt-input-icon__icon kt-input-icon__icon--left"">
                                            <span><i class=""la la-search""></i></span>
                                        </span>
                                    </div>");

            base.Process(context, output);
        }
    }

    [HtmlTargetElement("*", Attributes = "list-status")]
    public class TasklistStatusTagHelper : TagHelper
    {
        [HtmlAttributeName("list-status")]
        public string LabelName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendFormat(@"<div class=""kt-form__group kt-form__group--inline"">
                                        <div class=""kt-form__label"">
                                            <label>{0}:</label>
                                        </div>
                                        <div class=""kt-form__control"">
                                            <select class=""form-control kt-selectpicker"" id=""status"">
                                                <option value="" "">All</option>
                                                <option value=""1"">Active</option>
                                                <option value=""0"">Inactive</option>
                                            </select>
                                        </div>
                                    </div>", LabelName);

            base.Process(context, output);
        }
    }
}
