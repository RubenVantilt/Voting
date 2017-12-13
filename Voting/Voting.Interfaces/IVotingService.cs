using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Voting.Interfaces
{
    public interface IVotingService : IService
    {
        Task<Guid> CreatePoll();

        Task<Dictionary<string, int>> GetOptions(Guid pollId);

        Task AddOption(Guid pollId, string option);

        Task Vote(Guid pollId, string option);
    }
}

