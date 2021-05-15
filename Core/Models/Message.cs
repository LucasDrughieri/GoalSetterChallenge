using Core.Enums;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public sealed class Message
    {
        public string Description { get; }

        [JsonIgnore]
        public MessageType Type { get; set; }

        public Message(string description, MessageType type)
        {
            Description = description;
            Type = type;
        }

        public string MessageType => Type.ToString();
    }
}
