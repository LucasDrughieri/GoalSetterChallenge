using System.ComponentModel.DataAnnotations;

namespace Core.Models.Request
{
    public class ClientRequestModel
    {
        /// <summary>
        /// The name of the client
        /// </summary>
        /// <example>Homero Simpson</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The phone of the client
        /// </summary>
        /// <example>1122224444</example>
        [Required]
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Phone: {Phone}";
        }
    }
}
