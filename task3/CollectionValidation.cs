using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace np_4sem_proj
{
    public class CollectionValidation<T> where T: ICollectionClass<T>
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
