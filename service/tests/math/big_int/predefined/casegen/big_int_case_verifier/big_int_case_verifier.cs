
using System;
using System.IO;
using System.Numerics;

namespace osi.tests.casegen.service.math.big_int_case_verifier
{
    public static class big_int_case_verifier
    {
        private const string divide_by_zero = "divide_by_zero";
        private static uint failure = 0;

        private static void verify(this TextWriter o, 
                                   BigInteger r1, 
                                   BigInteger r2,
                                   string sr2,
                                   string r)
        {
            if (r1 != r2)
            {
                o.WriteLine("failure @ " + 
                            r + 
                            ", left " + 
                            r1.ToString() + 
                            ", right " + 
                            r2.ToString());
                failure++;
            }

            if (Convert.ToString(r1) != sr2)
            {
                o.WriteLine("failure @ " +
                            r +
                            " left as string, left " +
                            r1.ToString() +
                            ", right " +
                            r2.ToString());
                failure++;
            }

            if (Convert.ToString(r2) != sr2)
            {
                o.WriteLine("failure @ " +
                            r +
                            " right as string, left " +
                            r1.ToString() +
                            ", right " +
                            r2.ToString());
                failure++;
            }
        }

        private static void verify(this TextWriter o, string r1, string r2, string r)
        {
            if (r1 != r2)
            {
                o.WriteLine("failure @ " + r + ", left " + r1 + ", right " + r2);
                failure++;
            }
        }

        private static void invalid_input(this TextWriter o, string r)
        {
            o.WriteLine("invalid input @ " + r);
            failure++;
        }

        private static void calculate(string r, TextWriter o)
        {
            string[] s = r.Split(' ');
            if (s == null || s.Length != 3)
                o.invalid_input(r);
            else
            {
                BigInteger left = BigInteger.Parse(s[0]);
                BigInteger right = BigInteger.Parse(s[2]);
                switch (s[1])
                {
                    case "+":
                        o.WriteLine(left + right);
                        break;
                    case "-":
                        o.WriteLine(left - right);
                        break;
                    case "*":
                        o.WriteLine(left * right);
                        break;
                    case "/":
                        try
                        {
                            o.WriteLine(left / right);
                        }
                        catch (DivideByZeroException)
                        {
                            o.WriteLine(divide_by_zero);
                        }
                        break;
                    case "%":
                        try
                        {
                            o.WriteLine(left % right);
                        }
                        catch (DivideByZeroException)
                        {
                            o.WriteLine(divide_by_zero);
                        }
                        break;
                    case "^":
                        o.WriteLine(BigInteger.Pow(left, (int)right));
                        break;
                    default:
                        o.invalid_input(r);
                        break;
                }
            }
        }

        private static void input(string r, out BigInteger i, TextWriter o)
        {
            i = default(BigInteger);
            if (string.IsNullOrWhiteSpace(r))
                o.invalid_input(r);
            else i = BigInteger.Parse(r);
        }

        private static void input(string r, TextWriter o)
        {
            BigInteger i;
            input(r, out i, o);
        }

        private static void input_output(string r, TextWriter o)
        {
            BigInteger i;
            input(r, out i, o);
            Console.WriteLine(i);
        }

        private static void verify(string r, TextWriter o)
        {
            string[] s = r.Split(' ');
            if (s == null || s.Length != 5 || s[3] != "=")
                o.invalid_input(r);
            else
            {
                BigInteger left = BigInteger.Parse(s[0]);
                BigInteger right = BigInteger.Parse(s[2]);
                BigInteger result = (s[4] == divide_by_zero) ? default(BigInteger) : BigInteger.Parse(s[4]);
                switch (s[1])
                {
                    case "+":
                        o.verify(left + right, result, s[4], r);
                        break;
                    case "-":
                        o.verify(left - right, result, s[4], r);
                        break;
                    case "*":
                        o.verify(left * right, result, s[4], r);
                        break;
                    case "/":
                        if (right == 0)
                        {
                            try
                            {
                                left /= right;
                                o.verify(divide_by_zero + "failed", s[4], r);
                            }
                            catch (DivideByZeroException)
                            {
                                o.verify(divide_by_zero, s[4], r);
                            }
                        }
                        else o.verify(left / right, result, s[4], r);
                        break;
                    case "%":
                        if (right == 0)
                        {
                            try
                            {
                                left %= right;
                                o.verify(divide_by_zero + "failed", s[4], r);
                            }
                            catch (DivideByZeroException)
                            {
                                o.verify(divide_by_zero, s[4], r);
                            }
                        }
                        else o.verify(left % right, result, s[4], r);
                        break;
                    case "^":
                        o.verify(BigInteger.Pow(left, (int)right), result, s[4], r);
                        break;
                    default:
                        o.invalid_input(r);
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
                System.Diagnostics.ProcessPriorityClass.High;
            System.Threading.Thread.CurrentThread.Priority =
                System.Threading.ThreadPriority.Highest;
            bool c = false;
            bool io = false;
            bool i = false;
            if (args.Length > 0)
            {
                c = (string.Compare(args[0], "c", true) == 0);
                io = (string.Compare(args[0], "io", true) == 0);
                i = (string.Compare(args[0], "i", true) == 0);
            }
            string r = null;
            DateTime start = DateTime.Now;
            Console.WriteLine(start);
            while ((r = Console.ReadLine()) != null)
            {
                try
                {
                    if (c) calculate(r, Console.Out);
                    else if (io) input_output(r, Console.Out);
                    else if (i) input(r, Console.Out);
                    else verify(r, Console.Out);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message + " @ " + r);
                    failure++;
                }
            }
            DateTime end = DateTime.Now;
            Console.WriteLine(end);
            Console.WriteLine("failure " + failure.ToString());
            Console.WriteLine("total ticks " + (end.Ticks - start.Ticks).ToString());
        }
    }
}
