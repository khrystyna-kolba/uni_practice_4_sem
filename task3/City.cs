using np_4sem_proj.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace np_4sem_proj
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
            public static string TransformCity(this string e)
            {
                string res = "";
                for (int i = 0; i < e.Length; i++)
                {
                    if (e[i] != ' ')
                    {
                        res = res + char.ToUpper(e[i]);
                    }
                    else
                    {
                        res = res + "_";
                    }
                }
                return res;
            }
            public static City Parse(this string value)
            {
                return (City)Enum.Parse(typeof(City), value, true);
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
