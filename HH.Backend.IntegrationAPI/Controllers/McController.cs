using HH.Backend.Common.API.Services;
using HH.Backend.Common.Core.DTOs.RequestDTOs;
using HH.Backend.Common.Core.DTOs.ResponseDTOs;
using HH.Backend.Common.Web.Attributes;
using HH.Backend.Common.Web.Controllers;
using HH.Backend.IntegrationAPI.Request.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HH.Backend.IntegrationAPI.Controllers
{
    [ServiceFilter(typeof(AuthorizeIntegration))]
    [IgnoreClientValidation]
    [ApiExplorerSettings(IgnoreApi = true)] // INFO: Swagger ignore
    public class McController : BaseAPIController
    {
        private readonly IMcService _mcService;

        public McController(IMcService mcService) : base()
        {
            _mcService = mcService;
        }

        [HttpPost]
        public async Task<LoginFromMcResponseDTO> LoginFromMcAsync(LoginFromMcRequestDTO dto) =>
            await _mcService.LoginFromMcAsync(dto);

        [HttpPost]
        public async Task<RegisterFromMcResponseDTO> RegisterFromMcAsync(RegisterFromMcRequestDTO dto) =>
            await _mcService.RegisterFromMcAsync(dto);
    }
}
