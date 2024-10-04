using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogManagementAPI.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value?.Errors.Select(e => e.ErrorMessage)
                    });

                var problemDetails = new
                {
                    status = 400,
                    title = "Validation Errors",
                    errors = errors
                };

                context.Result = new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/json" }
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
