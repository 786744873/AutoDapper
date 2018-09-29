
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XDF.Core.Helper.Ajax;
using XDF.Core.Helper.Log;

namespace XDF.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private string _ex = "";

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _ex = "";
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _ex = ex.Message + ":" + ex.StackTrace;
            }
            finally
            {
                if (context.Response.StatusCode > 400 || !_ex.IsStringEmpty())
                {
                    await HandleExceptionAsync(context);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            var msg = "服务器内部错误";
            switch (statusCode)
            {
                case 401:
                    msg = "未授权";
                    break;
                case 404:
                    msg = "未找到接口";
                    break;
                case 500:
                    msg = "服务器内部错误";
                    break;
            }
            if (!_ex.IsStringEmpty())
            {
                LogHelper.Error(context.Request.Path + _ex);
            }
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(AjaxResult.Error<string>(statusCode, msg).ToString());
        }

    }
    public static class ErrorHandleExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder, Action action = null)
        {
            action?.Invoke();
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
