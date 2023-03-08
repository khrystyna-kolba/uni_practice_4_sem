using np_4sem_proj.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace np_4sem_proj
{
    public class ContainerValidation
    {
        public static bool ArrivalDateValidation(DateTime s, DateTime l)
        {
            if (s > l)
            {
                return false;
            }
            return true;
        }
        public static bool NumberValidation(string num)
        {
            Regex re = new Regex(@"^[A-Z]{2}-[0-9]{5}$");
            return re.IsMatch(num);
        }
        public static bool IdValidation(string id)
        {
            foreach (var c in id)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool DateValidation(string dt)
        {
            DateTime res;
            if(DateTime.TryParse(dt, out res))
            {
                return true;
            }
            return false;
        }

        public static bool CityValidation(string city)
        {
            object res;
            if (Enum.TryParse(typeof(City), city.TransformCity(), true, out res))
            {
                return true;
            }
            return false;
        }
        public static bool AmountValidation(string amount)
        {
            int res;
            if(int.TryParse(amount, out res))
            {
                if (res >= 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public static bool NewIdValidation(ContainerCollection c, string id)
        {
            if(c.GetIds().Contains(id)){
                return false;
            }
            return true;
        }

    }
}
