using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using PollActorService.Interfaces;

namespace PollActorService
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Poll : Actor, IPollActorService
    {
        /// <summary>
        /// Initializes a new instance of Poll
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Poll(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            await StateManager.AddStateAsync("options", new Dictionary<string, int>());
        }

        public async Task<Dictionary<string, int>> GetOptions()
        {
            var optionsState = await StateManager.TryGetStateAsync<Dictionary<string, int>>("options");

            return optionsState.HasValue ? optionsState.Value : null;
        }

        public async Task AddOption(string option)
        {
            var optionsState = await StateManager.TryGetStateAsync<Dictionary<string, int>>("options");

            if (!optionsState.HasValue)
            {
                return;
            }

            var options = optionsState.Value;

            //throwing this exception re-initializes the state for some reason
            //if (options.ContainsKey(option))
            //{
            //    throw new ArgumentException($"Option {option} already exists");
            //}

            if (!options.ContainsKey(option))
            {
                options.Add(option, 0);
            }

            await StateManager.SaveStateAsync();
        }

        public async Task Vote(string option)
        {
            var optionsState = await StateManager.TryGetStateAsync<Dictionary<string, int>>("options");

            if (!optionsState.HasValue)
            {
                return;
            }

            var options = optionsState.Value;

            if (options.ContainsKey(option))
            {
                options[option]++;
            }

            await StateManager.SaveStateAsync();
        }
    }
}
