using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppGNAggregator.Filters
{
    public class MyActionFilter : Attribute, IActionFilter
    {
        public async void OnActionExecuted(ActionExecutedContext context)
        {
           // await Task.CompletedTask;

            // throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //return OK();

            //throw new NotImplementedException();
        }

    }
}
