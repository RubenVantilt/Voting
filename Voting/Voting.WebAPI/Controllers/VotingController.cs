using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Voting.Interfaces;


namespace Voting.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class VotingController : Controller
    {
        public VotingController()
        {
            
        }

        [HttpPost]
        [Route("AddOption")]
        public async Task<IActionResult> AddOption([FromBody] string option)
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            await proxy.AddOption(option).ConfigureAwait(false); 

            return Ok();
        }

        [HttpPost]
        [Route("Vote")]
        public async Task<IActionResult> Vote([FromBody] string option)
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            await proxy.Vote(option).ConfigureAwait(false);

            return Ok();
        }
    }
}
