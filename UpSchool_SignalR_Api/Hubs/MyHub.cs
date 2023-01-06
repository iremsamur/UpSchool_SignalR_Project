using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UpSchool_SignalR_Api.Models;

namespace UpSchool_SignalR_Api.Hubs
{
    public class MyHub : Hub
    {
        public static List<string> Names { get; set; } = new List<string>();
        public int ClientCount { get; set; } = 0;//o an kaç tane client bağlı ise sayısını verecek

        public static int roomCount { get; set; } = 7;// bir oda da maximum 7 kişi bulunsun

        //veritabanına eklemek için
        private readonly Context _context;

        public MyHub(Context context)
        {
            _context = context;
        }

        //isim gönderen bir metot yazalım.
        /*
        public async Task SendName(string name)
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName", name);//gönderilen name değerini alır. ReceiveName olarak alıyor. parametreden gelen değeri alır.


        }
        */
        public async Task GetNames()
        {
            //Buda isimleri getiren bir metot olacak
            await Clients.All.SendAsync("ReceiveNames", Names);//listedeki tüm alanları getirir.
        }

        //bağlantı sağlandığında
        public async override Task OnConnectedAsync()
        {
            ClientCount++;//her bağlantı için 1 artar
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

        }

        //bağlantı kesildiğinde bağlı client sayısı 1 azalacak.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;//her bağlantı için 1 azalır
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

        }

        //odadaki kişi sayısını olması gereken kapasite ile kontrol edecek
        public async Task SendName(string name)
        {
            if (Names.Count >= roomCount)
            {
                //odadaki kişi sayısı yani names'ler sayısı eğer roomCount büyük ise 
                await Clients.Caller.SendAsync("Error",$"Bu oda en fazla {roomCount} kişi kadar üye alabilir.");
            }
            else
            {
                Names.Add(name);//sorun yoksa o kişiyi de bağlatacak yani listenin içerisine ekleyecek.
                await Clients.All.SendAsync("ReceiveName",name);
            }

        }

        //ismi istediği odaya atar.
        public async Task SendNameByGroup(string name,string roomName)
        {
            var room = _context.Rooms.Where(x => x.RoomName == roomName).FirstOrDefault();
            //gönderilen ismin hangi odada bulunduğunu görmek odalara bölmek için bu komut
            //satırını yazıyoruz.
            if (room != null)
            {
                room.Users.Add(new User
                {
                    Name=name
                });
            }
            else
            {
                var newRoom = new Room
                {
                    RoomName = roomName
                };//bu isimde bir oda yoksa odayı oluştur.
                newRoom.Users.Add(new User { Name = name });//yeni kullanıcıyı odaya eklesin.
                _context.Rooms.Add(new Room);

            }
            await _context.SaveChangesAsync();
            await Clients.Group(roomName).SendAsync("ReceiveMessageByGroup",name,room.RoomID);
            //seçili odaya o kişiyi ekleyecek

        }
        //grubun içindeki kişileri listeleyecek
        public async Task GetNamesByGroup()
        {
            var rooms = _context.Rooms.Include(x => x.Users).Select(y => new
            {
                roomId = y.RoomID,
                Users = y.Users.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup", rooms);
        }

    }

    }
