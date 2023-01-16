using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zelegram.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public byte[] Photo { get; set; }
        public List<Chat> Chats { get; set; }

    }
}
