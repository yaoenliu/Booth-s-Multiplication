using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long A = 11115039;
            long B = A * A;
            int so = 0;
            long C = (B >> 16) % (1 << 15);
            long Cinv = ((C ^ ((1 << 16) - 1)) + 1) % (1 << 16);
            int D = Convert.ToInt32((B >> 16) % (1 << 15) + (1 << 15));
            Console.WriteLine("A = " + A);
            Console.WriteLine("B = " + Convert.ToString(B, 2));
            Console.WriteLine("C = " + Convert.ToString(C, 2).PadLeft(16, '0'));
            // Console.WriteLine("c = " + Convert.ToString(Cinv, 2).PadLeft(16, '0'));
            Console.WriteLine("D = " + Convert.ToString(D, 2).PadLeft(16));
            Console.WriteLine("C×D\n");
            Console.WriteLine("Step " + "Product".PadLeft(20).PadRight(32) + "Next".PadLeft(14).PadRight(20));
            long product = 0;
            product += D;

            string op = product % 2 + "" + so;
            string command = "";
            if (op == "00" || op == "11") command = "shift";
            else if (op == "01") command = "add";
            else if (op == "10") command = "sub";
            Console.WriteLine(0.ToString().PadLeft(4) + "  " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + so + "  " + op + " -> " + command);
            product = rs(product, ref so);

            for (int i = 1; i <= 16; i++)
            {
                string step = i.ToString().PadLeft(4);
                if (command == "add")
                {
                    Console.WriteLine(i.ToString().PadLeft(4) + " +" + Convert.ToString(C, 2).PadLeft(16, '0'));
                    product = (product + (C << 16)) & ((1L << 32) - 1);
                    Console.WriteLine("      " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0') +" "+ so);
                    step = "    ";

                }
                else if (command == "sub")
                {
                    Console.WriteLine(i.ToString().PadLeft(4) + " +" + Convert.ToString(Cinv, 2).PadLeft(16, '0'));
                    product = (product + (Cinv << 16)) & ((1L << 32) - 1);
                    Console.WriteLine("      " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0') + " " + so);
                    step = "    ";

                }

                if (command != "shift")
                    product = rs(product, ref so);

                op = product % 2 + "" + so;
                if (op == "00" || op == "11") command = "shift";
                else if (op == "01") command = "add";
                else if (op == "10") command = "sub";
                
                if (i == 16)
                {
                    Console.WriteLine(step + "  " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + so + "  done");
                    break;
                }
                    

                Console.WriteLine(step + "  " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + so + "  " + op + " -> " + command);

                if (command == "shift")
                    product = rs(product, ref so);
            }

            Console.WriteLine("C×D = " +
                    Convert.ToString(((product >> 16) % (1 << 16)), 2).PadLeft(16, '0') +
                    " " + Convert.ToString(((product) % (1 << 16)), 2).PadLeft(16, '0'));
            Console.ReadKey();
        }
        static long rs(long source, ref int cf)
        {
            cf = Convert.ToInt32(source & 1); // get the last bit 
            if (((source>>31)&1) == 0) // positive
            {            
                source = (source >> 1) & ((1L << 32)-1);
            }
            else // negative
            {
                source = (source >> 1) | (1L << 31); // shift right and add 1 to the left
            }
            return source;
        }
    }
}
