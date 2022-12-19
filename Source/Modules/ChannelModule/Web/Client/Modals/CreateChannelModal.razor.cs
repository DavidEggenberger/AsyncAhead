﻿using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Modules.ChannelModule.Web.DTOs.Commands;
using Shared.Web.Client;
using Shared.Web.Client.Services;

namespace ChannelModule.Client.Modals
{
    public partial class CreateChannelModal : BaseComponent
    {
        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
        private CreateChannelDTO team = new CreateChannelDTO();
        private ValidationServiceResult validationServiceResult;
        private string currentName = string.Empty;
        public string CurrentName
        {
            get => currentName;
            set
            {
                currentName = value;
                validationServiceResult = ValidationService.Validate(new CreateChannelDTO { Name = currentName });
            }
        }
        private async Task CreateChannelAsync()
        {
            await HttpClientService.PostToAPIAsync("/channel", new CreateChannelDTO { Name = currentName });
            await ModalInstance.CancelAsync();
        }
    }
}