using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.ActionFilters;

public class ValidationFilterAttribute : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        ValidateDTOs(context);
        ValidatePatchObject(context);
    }

    private void ValidateDTOs(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"]!;
        var controller = context.RouteData.Values["controller"];

        if (context.ActionArguments.Where(x => x.Value!.ToString()!.Contains("DTO")).Count() == 0) return;

        var param = context.ActionArguments.SingleOrDefault(x => x.Value!.ToString()!.Contains("DTO"));

        if (param.Value is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}, param: {param.Key} ");
            return;
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }

    private void ValidatePatchObject(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"]!;
        var controller = context.RouteData.Values["controller"];

        var t = context.ActionArguments.Where(x => x.Key == "patchObject");

        if (context.ActionArguments.Where(x => x.Key == "patchObject").Count() == 0) return;

        var param = context.ActionArguments.SingleOrDefault(x => x.Key == "patchObject");

        if (param.Value is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}, paramee: {param.Key} ");
            return;
        }
        
        var patchObject = param.Value;
        var properties = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(patchObject.ToString()!);

        if (properties == null)
        {
            throw new ArgumentException($"Failed to deserialize JSON object. Controller: {controller}, action: {action}, param: {param.Key}");
        }
    }
}
