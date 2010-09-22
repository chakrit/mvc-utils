
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcUtils
{
  internal static class ExpressionUtil
  {
    public static RouteValueDictionary ExtractRouteValues<TController>(
      this Expression<Action<TController>> expr)
      where TController : ControllerBase
    {
      var dict = new RouteValueDictionary();

      // get controller name from the type information
      var controllerType = typeof(TController);
      var controller = controllerType.Name;

      if (controller.EndsWith("Controller"))
        controller = controller.Substring(0, controller.Length - "Controller".Length);

      dict["Controller"] = controller;

      // get action name from the referenced controller's method name in the expr
      var lambdaExpr = (LambdaExpression)expr;
      var callExpr = (MethodCallExpression)lambdaExpr.Body;

      dict["Action"] = callExpr.Method.Name;

      // use the values supplied to the action method as defaults for the
      // respective arguments/pareameters
      var callArgs = callExpr.Arguments.Select(getArgumentValue);

      var args = callExpr.Method
        .GetParameters()
        .Select(param => param.Name)
        .Zip(callArgs, Tuple.Create)
        .Where(arg => arg.Item2 != null);

      foreach (var pair in args)
        dict.Add(pair.Item1, pair.Item2);

      return dict;
    }

    private static object getArgumentValue(Expression e)
    {
      if (e.CanReduce) return getArgumentValue(e.Reduce());

      // TODO: OPTIMIZE! I think lambda can be re-used.
      //       and expression is probably cachable
      //       or maybe we can use T4 instead.
      if (e is ConstantExpression) {
        return ((ConstantExpression)e).Value;
      }
      else {
        e = Expression.Convert(e, typeof(object));
        return Expression.Lambda<Func<object>>(e)
          .Compile()
          .Invoke();
      }
    }
  }
}
