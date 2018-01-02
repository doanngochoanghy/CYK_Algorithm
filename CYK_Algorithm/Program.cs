using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYK_Algorithm
{
    class Rule
    {
        public char left;
        public string right;
        public Rule(char l, string r)
        {
            left = l; right = r;
        }
        public string ToFineString()
        {
            string s = left + " -->";
            for (int i = 0; i < right.Length; i++)
                s += " " + right[i];
            return s;
        }
    }

    class CYKParse
    {
        public List<Rule> rules = new List<Rule>();
        string[,] table = null;
        string w = null;
        int n = 0;

        public void AddRule(char left, string right)
        {
            rules.Add(new Rule(left, right));
        }

        public void PrintAllRules()
        {
            Console.WriteLine("Grammar Rule:");
            foreach (Rule r in rules)
                Console.WriteLine("  " + r.ToFineString());
            Console.WriteLine();
        }

        void InitData(string x)
        {
            w = x;
            n = x.Length;
            table = new string[n + 1, n + 1];
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= n; j++) table[i, j] = "";
        }

        void InitTable()
        {
            for (int i = 1; i <= n; i++)
                foreach (Rule r in rules)
                    if (r.right.Length == 1)
                        if (w[i - 1] == r.right[0]) table[i, 1] += r.left;
        }

        bool Check(char x, string w)
        {
            return w.IndexOf(x) != -1;
        }

        bool CanGenerate(int j, int i, Rule r)
        {
            for (int k = 1; k <= j - 1; k++)
                if (Check(r.right[0], table[i, k]))
                    if (Check(r.right[1], table[i + k, j - k]))
                        return true;
            return false;
        }

        void GenTable()
        {
            for (int j = 2; j <= n; j++)
                for (int i = 1; i <= n - j + 1; i++)
                    foreach (Rule r in rules)
                        if (r.right.Length == 2)
                            if (!Check(r.left, table[i, j]))
                                if (CanGenerate(j, i, r)) table[i, j] += r.left;
        }

        public void Parse(string x)
        {
            InitData(x);
            InitTable();
            GenTable();
        }

        public void PrintResult()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= i; j++) Console.Write("|{0,4}|", table[j, n + 1 - i]);
                Console.WriteLine();
            }
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            CYKParse p = new CYKParse();
            p.AddRule('S', "AB");
            p.AddRule('S', "BC");
            p.AddRule('A', "BA");
            p.AddRule('B', "CC");
            p.AddRule('C', "AB");
            p.AddRule('A', "a");
            p.AddRule('B', "b");
            p.AddRule('C', "a");
            p.PrintAllRules();
            string w = "baaba";
            p.Parse(w);
            Console.WriteLine("String: \"{0}\"\n", w);
            p.PrintResult();
            Console.ReadKey();
        }
    }

}
