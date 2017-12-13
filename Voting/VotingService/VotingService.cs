using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using PollActorService.Interfaces;
using Voting.Interfaces;

namespace VotingService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class VotingService : StatelessService, IVotingService
    {
        public VotingService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };
        }

        public Task<Guid> CreatePoll()
        {
            Guid pollId = Guid.NewGuid();

            ActorProxy.Create<IPollActorService>(new ActorId(pollId), new Uri("fabric:/Voting/PollActorServiceActorService"));

            return Task.FromResult(pollId);
        }

        public Task<Dictionary<string, int>> GetOptions(Guid pollId)
        {
            var poll = ActorProxy.Create<IPollActorService>(new ActorId(pollId), new Uri("fabric:/Voting/PollActorServiceActorService"));

            return poll.GetOptions();
        }

        public Task AddOption(Guid pollId, string option)
        {
            var poll = ActorProxy.Create<IPollActorService>(new ActorId(pollId), new Uri("fabric:/Voting/PollActorServiceActorService"));

            return poll.AddOption(option);
        }

        public Task Vote(Guid pollId, string option)
        {
            var poll = ActorProxy.Create<IPollActorService>(new ActorId(pollId), new Uri("fabric:/Voting/PollActorServiceActorService"));

            return poll.Vote(option);
        }
    }
}
