using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.TagHelpers
{
    
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public PageInfo PageInfo { get; set; }
        public string PageAction { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");

            result.AddCssClass("btn-group");

            for (int i = 1; i <= PageInfo.TotalPages; i++)
            {
                var itemTag = new TagBuilder("a");
                var anchorInnerHtml = i.ToString();
                itemTag.AddCssClass("btn btn-outline-primary");

                if (ViewContext.HttpContext.Request.Query.ContainsKey("pageNumber") && int.TryParse(ViewContext.HttpContext.Request.Query["pageNumber"], out var actualPage))
                {
                    if (i == actualPage)
                    {
                        itemTag.AddCssClass("active");
                    }
                    else
                    {
                        //i = 1;
                    }

                }
                else
                {
                    if (i == 1)
                    {
                        itemTag.AddCssClass("active");
                    }
                }
                itemTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = i, pageSize = PageInfo.PageSize });
                itemTag.InnerHtml.AppendHtml(anchorInnerHtml);
                result.InnerHtml.AppendHtml(itemTag);
            }

            output.TagName = "div";
            //output.TagMode = TagMode.SelfClosing;
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
   
    
    /*
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public PageInfo PageInfo { get; set; }
        public string PageAction { get; set; }
        public string AdditionalCssClass { get; set; } = string.Empty;  // Custom CSS class

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");

            result.AddCssClass("btn-group");
            if (!string.IsNullOrEmpty(AdditionalCssClass))
            {
                result.AddCssClass(AdditionalCssClass);
            }

            for (int i = 1; i <= PageInfo.TotalPages; i++)
            {
                var itemTag = new TagBuilder("a");
                var anchorInnerHtml = i.ToString();
                itemTag.AddCssClass("btn btn-outline-primary");

                if (ViewContext.HttpContext.Request.Query.ContainsKey("page") &&
                    int.TryParse(ViewContext.HttpContext.Request.Query["page"], out var actualPage))    //todo check page & size
                {
                    if (i == actualPage)
                    {
                        itemTag.AddCssClass("active");
                    }
                    else if (actualPage < 1 || actualPage > 15)
                    {
                        actualPage = i;
                        itemTag.AddCssClass("active");
                    }
                }
                else if (i == 1)
                {
                    itemTag.AddCssClass("active");
                }
                


                itemTag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i, pageSize = PageInfo.PageSize });


                //itemTag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                itemTag.Attributes["aria-label"] = $"Go to page {i}";  // Accessibility

                itemTag.InnerHtml.AppendHtml(anchorInnerHtml);
                result.InnerHtml.AppendHtml(itemTag);
            }

            output.TagName = "div";
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
    */
}
