using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hello_world
{
    class Program
    {
        private hello b=new hello();
        private int c;

        public void lan()
        {
            c = b.a;
            Console.WriteLine(c);
        }
        static void Main(string[] args)
        {
            Program l=new Program();
            hello he = new hello();
            he.hi();
            l.lan();
            Console.ReadKey();

        }
    }
}
