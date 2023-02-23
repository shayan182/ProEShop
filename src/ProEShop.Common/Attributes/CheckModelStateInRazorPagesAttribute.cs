﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;

namespace ProEShop.Common.Attributes;

//for using IAsyncPageFilter you most install Microsoft.AspNetCore.Mvc
public class CheckModelStateInRazorPagesAttribute : Attribute, IAsyncPageFilter
{
    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var modelState = context.ModelState;
        if (!modelState.IsValid)
        {
            context.Result = new JsonResult(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = modelState.GetModelStateErrors()
            });

        }else
            await next();
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;

    }
}