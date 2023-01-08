using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UpSchool_SignalR_Api.Models
{
    public class Room
    {
        //kayıt atmaya çalışırken aldığımız hatayı engellemek için bu constructor'ı ekliyoruz
        public Room()
        {
            Users = new List<User>();
        }
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public List<User> Users { get; set; }

        //her bir kullanıcı bir odada bulunabilir. Ama bir odada birden fazla kişi olabilir
    }
}
