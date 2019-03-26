using Cinemasters.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Helpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder nav = new TagBuilder("nav");
            nav.Attributes["aria-label"] = "Page navigation";

            TagBuilder ul = new TagBuilder("ul");
            ul.Attributes["class"] = "pagination justify-content-center";

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");
                if (PageModel.CurrentPage == i)
                {
                    li.Attributes["class"] = "page-item active";
                }
                else
                {
                    li.Attributes["class"] = "page-item";
                }

                TagBuilder a = new TagBuilder("a");
                a.Attributes["href"] = urlHelper.Action(PageAction,
                    new {
                        showPage = i ,
                        sortedBy = PageModel.SortedBy
                    });
                a.Attributes["class"] = "page-link";

                a.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
            nav.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(nav.InnerHtml);
        }
    }
}