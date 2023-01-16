using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zelegram.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
