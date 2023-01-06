using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UpSchool_SignalR_Api.Hubs
{
    public class MyHub : Hub
    {
        public static List<string> Names { get; set; } = new List<string>();

        //isim gönderen bir metot yazalım.
        public async Task SendName(string name)
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName",name);//gönderilen name değerini alır. ReceiveName olarak alıyor. parametreden gelen değeri alır.


        }
        public async Task GetNames()
        {
            //Buda isimleri getiren bir metot olacak
            await Clients.All.SendAsync("ReceiveNames", Names);//listedeki tüm alanları getirir.
        }
    }
}
