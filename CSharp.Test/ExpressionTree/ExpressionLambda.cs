using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CSharp.Test.ExpressionTree
{
    public class ExpressionLambda
    {

        private static ExpressionLambda _current;
        public static ExpressionLambda current {
            get
            {
                if (_current == null)
                {
                    _current = new ExpressionLambda();
                }
                return _current;
            }
        }

        public static void Run ()
        { 
            String version = "7.0";
            Console.WriteLine($"Version de C# : {version}");
            ExpressionLambda instance = new ExpressionLambda();

            //instance.ExpressionStartWith();
            //instance.ExpressionStartWithCustomStatic();
            //instance.ExpressionStartWithCustomNonStatic();

            PrintConvertedValue("I'm a string", x => x.Length);
            PrintConvertedValue("I'm a string", x => x.Replace("string", "different String"));

        }
        
        public void ExpressionStartWith()
        {
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var target = Expression.Parameter(typeof(string), "x");
            var methodArg = Expression.Parameter(typeof(string), "y");
            Expression[] methodArgs = new[] { methodArg };
            Expression call = Expression.Call(target, method, methodArgs);

            var lambdaParameters = new[] { target, methodArg };
            var lambda = Expression.Lambda<Func<string, string, bool>>(call, lambdaParameters);
            var compiled = lambda.Compile();
            Console.WriteLine(compiled("First", "Second"));
            Console.WriteLine(compiled("First", "Fir"));
        }
        
        public void ExpressionStartWithCustomStatic()
        {

            MethodInfo method = typeof(ExpressionLambda).GetMethod("startWith", new[] { typeof(string), typeof(string) });
            var target = Expression.Parameter(typeof(string), "x");
            var methodArg = Expression.Parameter(typeof(string), "y");
            Expression[] methodArgs = new[] { target, methodArg };
            Expression call = Expression.Call(method, methodArgs);

            var lambdaParameters = new[] { target, methodArg };
            var lambda = Expression.Lambda<Func<string, string, bool>>(call, lambdaParameters);
            var compiled = lambda.Compile();
            Console.WriteLine(compiled("First", "Second"));
            Console.WriteLine(compiled("First", "Fir"));
        }
        
        public void ExpressionStartWithCustomNonStatic()
        {

            MethodInfo method = typeof(ExpressionLambda).GetMethod("startWithNonStatic", new[] { typeof(string), typeof(string) });
            var target = Expression.Parameter(typeof(string), "x");
            var methodArg = Expression.Parameter(typeof(string), "y");
            var exp = Expression.Parameter(typeof(ExpressionLambda), "exp");
            Expression[] methodArgs = new[] { target, methodArg };
            Expression call = Expression.Call(exp, method, methodArgs);

            var lambdaParameters = new[] { exp, target, methodArg };
            var lambda = Expression.Lambda<Func<ExpressionLambda, string, string, bool>>(call, lambdaParameters);
            var compiled = lambda.Compile();
            Console.WriteLine(compiled(ExpressionLambda.current,"First", "Second"));
            Console.WriteLine(compiled(ExpressionLambda.current, "First", "Fir"));
        }

        public static bool startWith(string a, string b)
        {
            Console.WriteLine($"Custom Method for {a} and {b}");
            return a.StartsWith(b);
        }

        public bool startWithNonStatic(string a, string b)
        {
            Console.WriteLine($"Custom  Non Static Method for {a} and {b}");
            return a.StartsWith(b);
        }

        public static void PrintConvertedValue<TInput, TOutput> (TInput input, Converter<TInput, TOutput> converter)
        {
            Console.WriteLine(converter(input));
        }

    }
}
