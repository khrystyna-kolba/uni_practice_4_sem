using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    internal class Checks
    {
        static public bool CheckIfInt(string i) {
            int num;
            if (int.TryParse(i, out num))
            {
                return true;
            }
            return false;
        }
        static public bool CheckInRange(int i, int start, int end)
        {
            if (i>=start && i<=end) {
                return true;
            }
            return false;
        }
    }
}
