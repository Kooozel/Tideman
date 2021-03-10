using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS50
{
    class Program
    {
        static void Main(string[] args)
        {
            tideman tideman = new tideman();

            string[] candidates = new string[] {"A", "B", "C" };

            tideman.Mainx(candidates);
        }
    }
}
