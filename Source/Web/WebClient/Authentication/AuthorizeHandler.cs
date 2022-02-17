﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebClient.Authentication.Antiforgery;

namespace WebClient.Authentication
{
    public class AuthorizedHandler : DelegatingHandler
    {
        private readonly HostAuthenticationStateProvider authenticationStateProvider;
        private readonly AntiforgeryTokenService antiforgeryTokenService;
        public AuthorizedHandler(HostAuthenticationStateProvider authenticationStateProvider, AntiforgeryTokenService antiforgeryTokenService)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.antiforgeryTokenService = antiforgeryTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            HttpResponseMessage responseMessage;
            if (!authState.User.Identity.IsAuthenticated)
            {
                // if user is not authenticated, immediately set response status to 401 Unauthorized
                responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                request.Headers.Add("X-XSRF-TOKEN", await antiforgeryTokenService.GetAntiforgeryTokenAsync());
                responseMessage = await base.SendAsync(request, cancellationToken);
            }

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                // if server returned 401 Unauthorized, redirect to login page
                authenticationStateProvider.SignIn();
            }

            return responseMessage;
        }
    }
}