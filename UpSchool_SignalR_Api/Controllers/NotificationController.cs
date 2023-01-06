using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using UpSchool_SignalR_Api.Hubs;

namespace UpSchool_SignalR_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        //Burada odalara ayıracağız, sivas odası, ankara odası gibi.
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //başlatıldığında bir roomCount gönderecek
        // bu apiye istekte bulunulacak ve istekte bulunduktan sonra odada kaç kişinin olacağını set edecek
        [HttpGet("{roomCount}")]
        public async Task<IActionResult> SetRoomCount(int roomCount)
        {
            MyHub.roomCount = roomCount;//böylece apiden gönderdiğim roomcounta göre çalışacak
            //bir değer göndermediğim zaman MyHub içindeki default 7'yi alacak
            await _hubContext.Clients.All.SendAsync("Notify", $"Bu oda en fazla {roomCount} kişi olabilir.");
            //Burada verilen Notify ismi Index tarafında kullanılacak

            return Ok();
        }
    }
}
