using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
//using WebAppGNAggregator.Models;


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
 

            // Обработка текущей страницы
            int currentPage = 1;
            if (ViewContext.HttpContext.Request.Query.ContainsKey("pageNumber") &&
                int.TryParse(ViewContext.HttpContext.Request.Query["pageNumber"], out var parsedPage))
            {
                currentPage = parsedPage;
            }

            // Кнопка "Первая страница"
            if (currentPage > 1)
            {
                var firstPageTag = new TagBuilder("a");
                firstPageTag.AddCssClass("btn btn-outline-primary");//
                firstPageTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = 1, pageSize = PageInfo.PageSize });
                firstPageTag.InnerHtml.AppendHtml("<<");
                result.InnerHtml.AppendHtml(firstPageTag);
            }

            // Кнопка "Предыдущая"
            if (currentPage > 1)
            {
                var prevPageTag = new TagBuilder("a");
                prevPageTag.AddCssClass("btn btn-outline-primary");
                prevPageTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = currentPage - 1, pageSize = PageInfo.PageSize });
                prevPageTag.InnerHtml.AppendHtml("<");
                result.InnerHtml.AppendHtml(prevPageTag);
            }

            // Диапазон отображаемых кнопок
            int startPage = Math.Max(1, currentPage - PageInfo.DeviceType); // Показываем 2 страницы до текущей
            int endPage = Math.Min(PageInfo.TotalPages, currentPage + PageInfo.DeviceType); // Показываем 2 страницы после текущей

            for (int i = startPage; i <= endPage; i++)
            {
                var pageTag = new TagBuilder("a");
                pageTag.AddCssClass("btn btn-outline-primary");

                if (i == currentPage)
                {
                    pageTag.AddCssClass("btn400"); //active 
                }

                pageTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = i, pageSize = PageInfo.PageSize });
                pageTag.InnerHtml.AppendHtml(i.ToString());
                result.InnerHtml.AppendHtml(pageTag);
            }

            // Кнопка "Следующая"
            if (currentPage < PageInfo.TotalPages)
            {
                var nextPageTag = new TagBuilder("a");
                nextPageTag.AddCssClass("btn btn-outline-primary");
                nextPageTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = currentPage + 1, pageSize = PageInfo.PageSize });
                nextPageTag.InnerHtml.AppendHtml(">");
                result.InnerHtml.AppendHtml(nextPageTag);
            }

            // Кнопка "Последняя страница"
            if (currentPage < PageInfo.TotalPages)
            {
                var lastPageTag = new TagBuilder("a");
                lastPageTag.AddCssClass("btn btn-outline-primary");
                lastPageTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = PageInfo.TotalPages, pageSize = PageInfo.PageSize });
                lastPageTag.InnerHtml.AppendHtml(">>");
                result.InnerHtml.AppendHtml(lastPageTag);
            }

            // Установка финального HTML
            output.TagName = "div";
            output.Content.AppendHtml(result.InnerHtml);
        }

    }
}
