using task2np.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task2np
{
    namespace Extension
    {
        public static class ExtensionMethods
        {
            public static string GetName(this City e)
            {
                string res = "";
                string name = Enum.GetName(typeof(City), e);
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] == '_')
                    {
                        res = res + " ";
                    }
                    else if (i != 0 && name[i - 1] != '_')
                    {
                        res = res + char.ToLower(name[i]);
                    }
                    else
                    {
                        res = res + name[i];
                    }
                }
                return res;
            }
            public static int CompareTo(this City e1, City e2)
            {
                return e1.GetName().CompareTo(e2.GetName());
            }
        }
    }
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum City
    {
        [EnumMember(Value = "Berlin")]
        BERLIN,
        [EnumMember(Value = "Kyiv")]
        KYIV,
        [EnumMember(Value = "London")]
        LONDON,
        [EnumMember(Value = "Los Angeles")]
        LOS_ANGELES,
        [EnumMember(Value = "Lviv")]
        LVIV,
        [EnumMember(Value = "New York")]
        NEW_YORK,
        [EnumMember(Value = "Paris")]
        PARIS,
        [EnumMember(Value = "Toronto")]
        TORONTO
    }

    
}
