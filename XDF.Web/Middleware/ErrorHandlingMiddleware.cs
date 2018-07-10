
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDF.Core.Helper.Ajax;

namespace XDF.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        public readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
                throw;
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                }
                else if (statusCode == 502)
                {
                    msg = "请求错误";
                }
                else if (statusCode != 200)
                {
                    msg = "未知错误";
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }
        private Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            AjaxResultModel<string> data = AjaxResult.Error<string>(statusCode, msg);
            var result = JsonConvert.SerializeObject(data);
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }

    }
    public static class ErrorHandleExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
