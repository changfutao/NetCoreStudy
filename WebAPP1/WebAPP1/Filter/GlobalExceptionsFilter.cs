using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPP1.Common.LogHelper;

namespace WebAPP1.Filter
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILoggerHelper _loggerHelper;
        //private readonly IHubContext<ChatHub> _hubContext;
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
