using Core.Enums;

namespace Core.Models
{
    public sealed class Message
    {
        public string Description { get; }

        public MessageType Type { get; set; }

        public Message(string description, MessageType type)
        {
            Description = description;
            Type = type;
        }
    }
}
