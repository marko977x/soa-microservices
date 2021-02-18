using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AnalyticsMicroservice.Models;

namespace AnalyticsMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalyticsController : Controller
    {
        private IHubContext<MessageHub> _hubContext;
        public AnalyticsController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("{type}")]
        public async Task<IActionResult> Subscribe(
            [Required, FromQuery(Name = "connectionId")] string connectionId) {
            await _hubContext.Groups.AddToGroupAsync(connectionId, "analytics");
            return Ok();
        }
    }
}
