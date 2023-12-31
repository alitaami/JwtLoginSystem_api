﻿using Common.Resources;
using Entities.Base;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Net;
using System.Text.RegularExpressions;

namespace TavCompanyTask_Api.Controllers
{
    /// <summary>
    /// Controller for Login and SignUp operations
    /// </summary>
    [AllowAnonymous]
    public class AccountController : APIControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        /// <summary>
        /// Login ( after authenticate user , it sends access and refresh token for future uses ) .
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> 
        /// 
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _accountService.Login(tokenRequest, cancellationToken);
                return APIResponse(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// While access token has expired we can use the refresh token and username ( in the header ) to get new access token . 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string bearerToken, [FromHeader(Name = "Username")] string username, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken) || string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(Resource.InvalidOperationException);
            }

            string refreshToken = bearerToken.Replace("Bearer ", "");

            try
            {
                var result = await _accountService.GetNewAccessTokenUsingRefreshToken(refreshToken, username, cancellationToken);

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(Resource.GeneralErrorTryAgain);
            }
        }

        /// <summary>
        /// Sign up section
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignUp(UserViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                await _accountService.RegisterUser(model, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(Resource.GeneralErrorTryAgain);
            }
        }
    }
}
