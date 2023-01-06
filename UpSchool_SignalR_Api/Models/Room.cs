using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UpSchool_SignalR_Api.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public List<User> Users { get; set; }

        //her bir kullanıcı bir odada bulunabilir. Ama bir odada birden fazla kişi olabilir
    }
}
