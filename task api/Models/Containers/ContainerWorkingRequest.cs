using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ContainersApiTask.Models.Containers
{
    [DataContract]
    public class ContainerWorkingRequest
    {
        [Required]
        [DataMember]
        public string Number { get; set; }
        [Required]
        [DataMember]
        public string DepartureCity { get; set; }
        [Required]
        [DataMember]
        public string ArrivalCity { get; set; }
        [Required]
        [DataMember]
        public string DepartureDate { get; set; }
        [Required]
        [DataMember]
        public string ArrivalDate { get; set;}
        [Required]
        [DataMember]
        public int AmountOfItems { get; set; }
    }
}
