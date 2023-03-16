using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerUniversalTest
{
    public class CollectionValidation<T> where T: IBaseClass<T>
    {
        public static bool NewIdValidation(Collection<T> c, string id)
        {
            if (c.GetIds().Contains(id))
            {
                return false;
            }
            return true;
        }
    }
}
