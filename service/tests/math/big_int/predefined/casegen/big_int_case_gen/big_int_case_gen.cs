
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace osi.tests.casegen.service.math.big_int_case_gen
{
    public static class big_int_case_gen
    {
        private static readonly Random rnd;

        static big_int_case_gen()
        {
            rnd = new Random();
        }

        private static BigInteger rnd_number()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = rnd.Next(512); i >= 0; i--)
                sb.Append(rnd.Next(10));
            if (rnd.Next(2) == 0) return BigInteger.Parse("-" + Convert.ToString(sb));
            else return BigInteger.Parse(Convert.ToString(sb));
        }

        private static void write(this TextWriter o,
                                  BigInteger left,
                                  BigInteger right,
                                  string op,
                                  string result)
        {
            o.WriteLine(Convert.ToString(left) +
                        " " +
                        op +
                        " " +
                        Convert.ToString(right) +
                        " = " +
                        result);
        }

        private static void write(this TextWriter o,
                                  BigInteger left,
                                  BigInteger right,
                                  string op,
                                  BigInteger result)
        {
            write(o, left, right, op, Convert.ToString(result));
        }

        private static void create_case(TextWriter o)
        {
            int r = rnd.Next(6);
            BigInteger left = rnd_number();
            BigInteger right = rnd_number();
            switch (r)
            {
                case 0:
                    o.write(left, right, "+", left + right);
                    break;
                case 1:
                    o.write(left, right, "-", left - right);
                    break;
                case 2:
                    o.write(left, right, "*", left * right);
                    break;
                case 3:
                    if (right == 0) o.write(left, right, "/", "divide_by_zero");
                    else if(left.IsEven || BigInteger.Abs(left) >= BigInteger.Abs(right))
                        o.write(left, right, "/", left / right);
                    else
                        o.write(right, left, "/", right / left);
                    break;
                case 4:
                    if (right == 0) o.write(left, right, "%", "divide_by_zero");
                    else if(left.IsEven || BigInteger.Abs(left) >= BigInteger.Abs(right))
                        o.write(left, right, "%", left % right);
                    else
                        o.write(right, left, "%", right % left);
                    break;
                case 5:
                    right = rnd.Next(256);
                    o.write(left, right, "^", BigInteger.Pow(left, (int)right));
                    break;
                default:
                    throw new Exception();
            }
        }

        public static void Main(string[] args)
        {
            uint count = 0;
            if (args == null || args.Length == 0 || !uint.TryParse(args[0], out count))
                count = 1024 * 16;

            for (uint i = 0; i < count; i++) create_case(Console.Out);
        }
    }
}
