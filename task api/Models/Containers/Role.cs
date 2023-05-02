using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContainersApiTask.Models.Containers
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Role
    {
        [EnumMember(Value = "Customer")]
        CUSTOMER,
        [EnumMember(Value = "Admin")]
        ADMIN,
        [EnumMember(Value = "Manager")]
        MANAGER
    }
}
