﻿using Blazored.Modal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Modules.Channels.Web.Shared.DTOs.ChannelAggregate;
using Shared.Client;
using Shared.Kernel.BuildingBlocks.Services.Http;

namespace Modules.Channels.Web.Client.Modals
{
    //public partial class CreateChannelModal : BaseComponent
    //{
    //    [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
    //    private ChannelDTO team = new ChannelDTO();
    //    private ValidationResult validationServiceResult;
    //    private string currentName = string.Empty;
    //    public string CurrentName
    //    {
    //        get => currentName;
    //        set
    //        {
    //            currentName = value; 
    //            //validationServiceResult = ValidationService.Validate(new ChannelDTO { Name = currentName });
    //        }
    //    }
    //    private async Task CreateChannelAsync()
    //    {
    //        //await HttpClientService.PostToAPIAsync("/channel", new ChannelDTO { Name = currentName });
    //        await ModalInstance.CancelAsync();
    //    }
    //}
}
