﻿using Ray.Core;
using Ray.Core.EventSourcing;
using Ray.Core.MQ;
using Ray.IGrains;
using Ray.IGrains.Actors;
using Ray.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace Ray.Handler
{
    [RabbitSub("Core", "Account", "account", QueueCount = 20)]
    public sealed class AccountCoreHandler : MultHandler<long, MessageInfo>
    {
        IClientFactory clientFactory;
        public AccountCoreHandler(IServiceProvider svProvider, IClientFactory clientFactory) : base(svProvider)
        {
            this.clientFactory = clientFactory;
        }

        protected override Task SendToAsyncGrain(byte[] bytes, IEventBase<long> evt)
        {
            var client = clientFactory.GetClient();
            return client.GetGrain<IAccountFlow>(evt.StateId).ConcurrentTell(bytes);
        }
    }
}
