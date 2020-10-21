using System;
using System.Collections.Generic;
using AutoMapper;
using EMS.Permission_Services.API.Context;
using EMS.SharedTesting.Helper;
using MassTransit;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    class EventConsumerTest<TConsumer> : BaseConsumerTest<TConsumer, PermissionContext> where TConsumer: class, IConsumer
    {
    }
}