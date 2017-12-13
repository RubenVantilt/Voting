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
        [Route("CreatePoll")]
        public async Task<IActionResult> CreatePoll()
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            var pollId = await proxy.CreatePoll().ConfigureAwait(false);

            return Ok(pollId);
        }

        [HttpGet]
        [Route("{pollId}/GetOptions")]
        public async Task<IActionResult> GetOptions(Guid pollId)
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            var options = await proxy.GetOptions(pollId);

            return Json(options);
        }

        [HttpPost]
        [Route("{pollId}/AddOption")]
        public async Task<IActionResult> AddOption(Guid pollId, [FromBody] string option)
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            await proxy.AddOption(pollId, option).ConfigureAwait(false); 

            return Ok();
        }

        [HttpPost]
        [Route("{pollId}/Vote")]
        public async Task<IActionResult> Vote(Guid pollId, [FromBody] string option)
        {
            var proxy = ServiceProxy.Create<IVotingService>(new Uri("fabric:/Voting/VotingService"));

            await proxy.Vote(pollId, option).ConfigureAwait(false);

            return Ok();
        }
    }
}
