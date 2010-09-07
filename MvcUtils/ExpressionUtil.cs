
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcUtils
{
  internal static class ExpressionUtil
  {
    public static RouteValueDictionary ExtractRouteValues<TController>(this Expression<Action<TController>> expr)
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
      var callArgs = callExpr.Arguments
        .Select(arg => arg.CanReduce ? arg.Reduce() : arg)
        .Cast<ConstantExpression>()
        .Select(arg => arg.Value);

      var parameters = callExpr.Method
        .GetParameters()
        .Select(param => param.Name);

      var args = parameters.Zip(callArgs, Tuple.Create);

      foreach (var pair in args)
        dict.Add(pair.Item1, pair.Item2);

      return dict;
    }
  }
}
