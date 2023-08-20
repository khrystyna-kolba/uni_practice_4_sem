using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainersApiTask.Models.Extensions
{
    public static class EnumExentions
    {
        public static T ParseEnum<T>(this T value, string s) where T : struct, Enum
        {
            s = s.TransformEnum();
            return (T)Enum.Parse(typeof(T), s, true);
        }
        public static string GetName<T>(this T e) where T : struct, Enum
        {
            string res = "";
            string name = Enum.GetName(typeof(T), e);
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
    }
}
