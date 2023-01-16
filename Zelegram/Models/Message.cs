using System;
using System.Collections.Generic;
using System.Text;

namespace Zelegram.Models
{
    public enum MessageType
    {
        Document,
        Location,
        Text
    }
    public class Message
    {
        public int Id { get; set; }
        public MessageType Type { get; set; }
        public int UserId { get; set; }

        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public Message Parent { get; set; }
    }
}
