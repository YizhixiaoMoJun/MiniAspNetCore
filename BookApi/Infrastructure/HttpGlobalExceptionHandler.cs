using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shirley.Book.Web.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    public class HttpGlobalExceptionHandler : IExceptionFilter, IOrderedFilter
    {
        private readonly ILogger<HttpGlobalExceptionHandler> logger;

        private readonly IWebHostEnvironment hostEnvironment;

        public HttpGlobalExceptionHandler(ILogger<HttpGlobalExceptionHandler> logger, IWebHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public int Order => 0;

        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            if (context.Exception is DomainException domain)
            {
                this.logger.LogWarning("status : {status},{message}", domain.Status, domain.Message);
                context.Result = new JsonResult(ApiResponse.Error(domain.Message, domain.Status));
                context.ExceptionHandled = true;
                return;
            }

            this.logger.LogError(context.Exception, "an internal exception occured.");

            var error = this.hostEnvironment.IsDevelopment()
                ? context.Exception.ToString()
                : context.Exception.Message;

            context.Result = new JsonResult(ApiResponse.Error(error));
            context.ExceptionHandled = true;
        }
    }
}
