using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ContainersApiTask.Models.Containers
{
    [DataContract]
    public class PublishedContainer
    {
        [DataMember(Order = 0)]
        public string Id { get; set; }
        [DataMember(Order = 1)]
        public string Number { get; set; }
        [DataMember(Order = 2)]
        public City DepartureCity { get; set; }
        [DataMember(Order = 3)]
        public City ArrivalCity { get; set; }
        [DataMember(Order = 4)]
        public DateTime DepartureDate { get; set; }
        [DataMember(Order = 5)]
        public DateTime ArrivalDate { get; set; }
        [DataMember(Order = 6)]
        public int AmountOfItems { get; set; }

        //public bool Search(string search)
        //{
        //    foreach (var p in typeof(PublishedContainer).GetProperties())
        //    {
        //        if (p.GetValue(this).ToString().ToLower().Contains(search.ToLower()))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}
