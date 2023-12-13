using HH.Backend.Common.Core.Configuration;
using HH.Backend.Common.Core.Constants;
using HH.Backend.Common.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using HH.Backend.Common.Core.Enums;

namespace HH.Backend.IntegrationAPI.Request.Filters
{
    public class AuthorizeIntegration : ActionFilterAttribute
    {
        private readonly BaseOptions _baseOptions;
        private readonly ILogService _logger;

        public AuthorizeIntegration(
            IOptions<BaseOptions> options,
            ILogService logger)
        {
            _baseOptions = options.Value;
            _logger = logger;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string integrationSecretKey = string.Empty;


            if (context is not null)
            {
                integrationSecretKey = context?.HttpContext?.Request?.Headers[SessionConstants.IntegrationSecretKey] ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(integrationSecretKey))
            {
                await _logger.LogAsync(new()
                {
                    LogLevel = Common.Core.Enums.LogLevel.Critical,
                    LogType = LogType.Antiforgery,
                    Message = $"External integration request authorization was challenged: missing integration secret key."
                });

                if (context is not null)
                {
                    context.Result = new UnauthorizedResult();
                }

                return;
            }

            if (!string.Equals(integrationSecretKey, _baseOptions.IntegrationSecretKey, System.StringComparison.OrdinalIgnoreCase))
            {
                await _logger.LogAsync(new()
                {
                    LogLevel = Common.Core.Enums.LogLevel.Critical,
                    LogType = LogType.Antiforgery,
                    Message = "External integration request authorization was challenged: " +
                        $"Invalid integration secret key '{_baseOptions.IntegrationSecretKey}'"
                });

                if (context is not null)
                {
                    context.Result = new UnauthorizedResult();
                }

                return;
            }

            await next();
        }
    }
}
