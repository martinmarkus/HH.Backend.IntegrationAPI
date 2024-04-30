using HH.Backend.Common.API.Extensions;
using HH.Backend.Common.API.Services;
using HH.Backend.Common.Core.DTOs.RequestDTOs;
using HH.Backend.Common.Core.DTOs.ResponseDTOs;
using HH.Backend.Common.Web.Attributes;
using HH.Backend.Common.Web.Authentication.Interfaces;
using HH.Backend.Common.Web.Controllers;
using HH.Backend.IntegrationAPI.Request.Filters;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace HH.Backend.IntegrationAPI.Controllers
{
    [ServiceFilter(typeof(AuthorizeIntegration))]
    [IgnoreClientValidation]
    [ApiExplorerSettings(IgnoreApi = true)] // INFO: Swagger ignore
    public class IntegrationController : BaseAPIController
    {
        private readonly IGameServerIntegrationService _gameServerIntegrationService;
        private readonly IUserService _userService;
        private readonly IBaseAuthenticator _baseAuthenticator;
        private readonly ICrateService _crateService;

        public IntegrationController(
            IGameServerIntegrationService gameServerIntegrationService,
            IBaseAuthenticator baseAuthenticator,
            IUserService userService,
            ICrateService crateService) : base()
        {
            _gameServerIntegrationService = gameServerIntegrationService;
            _baseAuthenticator = baseAuthenticator;
            _userService = userService;
            _crateService = crateService;
        }

        /// <summary>
        /// Misc data generated from game units' operation.
        /// These data should be sent periodically
        /// </summary>
        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task<ActionResult> SendGeneralDataAsync(SendGeneralDataRequestDTO dto) =>
            (await _gameServerIntegrationService.SendGeneralDataAsync(dto))
                .ToActionResult();

        [HttpPost]
        [SwaggerResponse(typeof(IList<PurchaseItemActivationResponseDTO>))]
        public async Task<ActionResult> GetPurchaseItemActivationsForVerificationAsync() =>
            Ok(await _gameServerIntegrationService.GetPurchaseItemActivationsForVerificationAsync());

        /// <summary>
        /// Approves verified pruchase items.
        /// </summary>
        /// <param name="purchaseItemIds">Merged list of purchase item ids and kredit activation ids</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task ApproveVerifiedPurchaseItemsAsync([FromBody] IList<Guid> purchaseItemAndKreditPurchaseActivationIds) =>
            await _gameServerIntegrationService.ApproveVerifiedPurchaseItemActivationssAsync(purchaseItemAndKreditPurchaseActivationIds);

        [HttpPost]
        [SwaggerResponse(typeof(IList<PurchaseItemActivationResponseDTO>))]
        public async Task<ActionResult> GetPurchaseItemActivationsForExpirationAsync() =>
            Ok(await _gameServerIntegrationService.GetPurchaseItemActivationsForExpirationValidationAsync());

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task ApproveExpiredPurchasesAsync([FromBody] IList<Guid> purchaseIds) =>
            await _gameServerIntegrationService.ApproveExpiredPurchasesAsync(purchaseIds);

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task<ActionResult> AddHyCoinAsync([FromBody] HyCoinManipulationRequestDTO hyCoinManipulationRequestDTO) =>
            (await _gameServerIntegrationService.AddHyCoinAsync(hyCoinManipulationRequestDTO))
                .ToActionResult();

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task RemoveHyCoinAsync([FromBody] HyCoinManipulationRequestDTO hyCoinManipulationRequestDTO) =>
        await _gameServerIntegrationService.RemoveHyCoinAsync(hyCoinManipulationRequestDTO);

        [HttpPost]
        [SwaggerResponse(typeof(void))]
        public async Task SetHyCoinAsync([FromBody] HyCoinManipulationRequestDTO hyCoinManipulationRequestDTO) =>
            await _gameServerIntegrationService.SetHyCoinAsync(hyCoinManipulationRequestDTO);

        [HttpPost]
        [SwaggerResponse(typeof(AuthenticationResponseDTO))]
        public async Task<ActionResult> ValidateUser(ValidateUserRequestDTO dto) =>
            (await _baseAuthenticator.ValidateUserExternallyAsync(dto))
                .ToActionResult();

        [HttpPost]
        [SwaggerResponse(typeof(UserResponseDTO))]
        public async Task<ActionResult> GetUserAsync(UserNameRequestDTO dto) =>
            (await _userService.GetUserAsync(dto))
                .ToActionResult();

        // INFO: Crates are only openable from game units
        [HttpPost]
        [SwaggerResponse(typeof(OpenCrateResponseDTO))]
        public async Task<ActionResult> OpenCrateAsync(OpenCrateRequestDTO dto) =>
            Ok(await _crateService.OpenCrateAsync(dto));
    }
}
