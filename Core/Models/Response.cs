using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Response<T> : Response where T : class
    {
        public Response()
        {
        }

        public Response(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class Response
    {
        public Response()
        {
            Messages = new List<Message>();
        }

        public IList<Message> Messages { get; set; }

        public bool HasErrors()
        {
            return Messages.Any(x => x.Type == MessageType.Error);
        }

        public void AddMessages(IList<Message> list)
        {
            foreach (var message in list)
                Messages.Add(message);
        }

        public void AddError(string code, string message)
        {
            Messages.Add(new Message(code, message, MessageType.Error));
        }

        public void AddWarning(string code, string message)
        {
            Messages.Add(new Message(code, message, MessageType.Warning));
        }

        public void AddSuccess(string code, string message)
        {
            Messages.Add(new Message(code, message, MessageType.Success));
        }
    }
}
