using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pattern_proxy_np.models.Collection
{
    public class CollectionValidation<T> where T : IBaseClass<T>
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
