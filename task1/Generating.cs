using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    internal class Generating
    {
        private static readonly Random rand = new Random();
        static public List<int> RandomGenerating(int N, int a, int b)
        {
            List<int> res = new List<int>();
            for(int i=0; i<N; i++)
            {
                res.Add(rand.Next(a, b+1));
            }
            return res;
        }
        static public int GenerateNumInGroup(int group)
        {
            if (group > 0)
            {
                return rand.Next();
            }
            else if (group < 0)
            {
                return rand.Next()* -1;
            }
            else
            {
                return 0;
            }
        }
        static public int GenerateNumInGroup(int group, int max)
        {
            if (max < 0)
            {
                max = max * -1;
            }
            if (group > 0)
            {
                return rand.Next(max);
            }
            else if (group < 0)
            {
                return rand.Next(max) * -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
