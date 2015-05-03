using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetConsole.WebApi.Tests
{
    public static class ReflectionHelper
    {

        public static string GetMethodName<T, TU>(Expression<Func<T, TU>> expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method != null)
                return method.Method.Name;

            throw new ArgumentException("Expression is wrong");
        }

    }
}
