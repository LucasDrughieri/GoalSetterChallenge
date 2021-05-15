namespace Core.Models.Request
{
    public class ClientRequestModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Phone: {Phone}";
        }
    }
}
