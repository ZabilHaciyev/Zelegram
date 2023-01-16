using System;
using System.Collections.Generic;
using System.Text;

namespace Zelegram.Models
{
    public class ChatForSending
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
