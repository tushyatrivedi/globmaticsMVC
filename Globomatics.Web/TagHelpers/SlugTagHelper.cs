using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Globomatics.Web.TagHelpers
{
    [HtmlTargetElement("url-with-slug")]
    public class SlugTagHelper : AnchorTagHelper
    {
        public SlugTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        [HtmlAttributeName("for-product-id")]
        public Guid ProductId { get; set; }

        [HtmlAttributeName("for-ticket-name")]
        public required string TicketTitle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "a";

            var slug = Regex.Replace(TicketTitle, @"[^a-zA-Z0-9]", "-", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200))
                .ToLowerInvariant()
                .Trim('-');

            RouteValues.Add("productId",ProductId.ToString());
            RouteValues.Add("slug",TicketTitle);

            base.Process(context, output);
        }
    }
}
