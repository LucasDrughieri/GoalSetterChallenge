using Core.Enums;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public sealed class Message
    {
        public string Code { get; }

        public string Description { get; }

        [JsonIgnore]
        public MessageType Type { get; set; }

        public Message(string code, string description, MessageType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string MessageType => Type.ToString();
    }
}
