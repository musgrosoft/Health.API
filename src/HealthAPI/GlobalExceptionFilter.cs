using Microsoft.AspNetCore.Mvc.Filters;
using Utils;

namespace HealthAPI
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        //ILogger<GlobalExceptionFilter> logger = null;

        //public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> exceptionLogger)
        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // log the exception
            //logger.LogError(0, context.Exception.GetBaseException(), "Exception occurred.");
            _logger.Error(context.Exception.GetBaseException());
        }
    }
}
