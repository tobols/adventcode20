using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode18
{
    class Program
    {
        private enum Operators
        {
            Addition,
            Multiplication,
            Value
        }

        private static char[] Ops = new char[] { '+', '*' };


        static void Main(string[] args)
        {
            var expressions = ReadFile().ToList();
            var sum = expressions.Select(e => BuildExpressionTree(e, Ops)).Sum(t => t.Evaluate());
            Console.WriteLine(sum);

            var sum2 = expressions.Select(e => BuildExpressionTree(e, '*')).Sum(t => t.Evaluate());
            Console.WriteLine(sum2);
        }


        private static List<string> TermSplitter(string expression, char[] operators)
        {
            List<string> terms = new List<string>(9);
            int b = 0;

            for (int i = 0; i < expression.Length; i++)
            {

                if (b == 0 && operators.Contains(expression[i]))
                {
                    var leftSide = expression[..i].Trim();
                    if (leftSide.Length > 0)
                        terms.Add(leftSide);

                    terms.Add(expression[i].ToString());

                    terms.AddRange(TermSplit(expression.Substring(i + 1), operators));
                    return terms;
                }

                if (expression[i] == '(')
                    b++;
                if (expression[i] == ')')
                    b--;
            }

            terms.Add(expression);
            return terms;
        }


        private static string TermClean(string term)
        {
            term = term.Trim();

            if (term.StartsWith('('))
            {
                var b = 0;
                for (int i = 0; i < term.Length; i++)
                {
                    if (term[i] == '(')
                        b++;
                    if (term[i] == ')')
                    {
                        b--;
                        if (b == 0 && i < term.Length - 1)
                            return term;
                    }
                }

                return TermClean(term.Substring(1, term.Length - 2));
            }

            return term;
        }


        private static List<string> TermSplit(string expression, params char[] operators)
        {
            return TermSplitter(expression.Trim(), operators).Select(TermClean).ToList();
        }


        private static ExpressionTreeNode BuildExpressionTree(string expression, params char[] opar)
        {
            var terms = TermSplit(expression, opar);
            if (opar.Length == 1 && terms.Count() < 2)
                terms = TermSplit(expression, Ops[(Array.IndexOf(Ops, opar[0]) + 1) % 2]);

            List<ExpressionTreeNode> children = new List<ExpressionTreeNode>();
            ExpressionTreeNode opNode = null;

            foreach (var term in terms)
            {
                if (int.TryParse(term, out var t))
                {
                    children.Add(new ExpressionTreeNode(Operators.Value, t));
                }
                else if (term.Trim()[0] == '+')
                {
                    if (opNode != null)
                    {
                        opNode.AddChildren(children);
                        children = new List<ExpressionTreeNode> { opNode };
                    }

                    opNode = new ExpressionTreeNode(Operators.Addition);
                }
                else if (term.Trim()[0] == '*')
                {
                    if (opNode != null)
                    {
                        opNode.AddChildren(children);
                        children = new List<ExpressionTreeNode> { opNode };
                    }

                    opNode = new ExpressionTreeNode(Operators.Multiplication);
                }
                else
                {
                    children.Add(BuildExpressionTree(term, opar));
                }
            }

            opNode.AddChildren(children);

            return opNode;
        }


        private class ExpressionTreeNode
        {
            private Operators op;
            private int val;
            private List<ExpressionTreeNode> _children = new List<ExpressionTreeNode>();

            public ExpressionTreeNode(Operators op)
            {
                this.op = op;
            }

            public ExpressionTreeNode(Operators op, int val)
            {
                this.op = op;
                this.val = val;
            }

            public void AddChildren(List<ExpressionTreeNode> children)
            {
                _children.AddRange(children);
            }

            public long Evaluate() =>
                op switch
                {
                    Operators.Addition => _children.Aggregate(0L, (r, c) => r + c.Evaluate()),
                    Operators.Multiplication => _children.Aggregate(1L, (r, c) => r * c.Evaluate()),
                    _ => val
                };
        }


        private static IEnumerable<string> ReadFile()
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }
    }
}
