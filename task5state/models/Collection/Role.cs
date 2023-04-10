using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Collection
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Role
    {
        [EnumMember(Value = "customer")]
        customer,
        [EnumMember(Value = "admin")]
        admin,
        [EnumMember(Value = "manager")]
        manager
    }
}
