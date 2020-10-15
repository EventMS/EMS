using System;
using System.Collections.Generic;
using AutoMapper;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Mapper;
using EMS.SharedTesting.Helper;
using MassTransit;

namespace EMS.EventParticipant_Services.API.UnitTests.Consumers
{
    class EventConsumerTest<TConsumer> : BaseConsumerTest<TConsumer, EventParticipantContext> where TConsumer: class, IConsumer
    {
        protected IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventParticipantProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
    }
}