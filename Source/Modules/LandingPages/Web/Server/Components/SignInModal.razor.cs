﻿using Microsoft.AspNetCore.Components;

namespace Modules.LandingPages.Server.Components
{
    public partial class SignInModal : ComponentBase
    {
        [Parameter]
        public EventCallback CancelRequestedCallback { get; set; }

        private bool switchToSignUp;

        public async Task CancelRequestedAsync()
        {
            if (CancelRequestedCallback.HasDelegate)
            {
                await CancelRequestedCallback.InvokeAsync(this);
            }
        }
    }
}
