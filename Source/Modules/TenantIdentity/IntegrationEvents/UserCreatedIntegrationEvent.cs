﻿using Shared.Kernel.BuildingBlocks.Events;
using System;

namespace Modules.TenantIdentity.IntegrationEvents
{
    public class UserCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}