using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Voting.Interfaces
{
    public interface IVotingService : IService
    {
        Task AddOption(string option);

        Task Vote(string option);
    }
}

