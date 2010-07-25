
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcUtils
{
  public static class FluentMapRoute
  {
    public static RouteCollection Using<TController>
      (this RouteCollection routes, Action<FluentMapRouteSyntax<TController>> mapper)
      where TController : ControllerBase
    {
      mapper(new FluentMapRouteSyntax<TController>(routes));
      return routes;
    }
  }

  public class FluentMapRouteSyntax<TController>
    where TController : ControllerBase
  {
    private static ISet<string> _namespaces;


    private RouteCollection _routes;

    public FluentMapRouteSyntax(RouteCollection routes)
    {
      _routes = routes;
      _namespaces = _namespaces ?? new HashSet<string>();
    }


    public FluentMapRouteSyntax<TController> Handle
      (string url, Expression<Action<TController>> action)
    {
      var dict = new RouteValueDictionary();

      // get controller name from the type
      var controllerType = typeof(TController);
      var controller = controllerType.Name;

      if (controller.EndsWith("Controller"))
        controller = controller.Substring(0, controller.Length - "Controller".Length);

      dict["Controller"] = controller;

      // ensure the controller namespace is included
      _namespaces.Add(controllerType.Namespace);

      // get action name from the method name in the expression
      var lambdaExpr = (LambdaExpression)action;
      var callExpr = (MethodCallExpression)lambdaExpr.Body;

      dict["Action"] = callExpr.Method.Name;

      // get defaults for each arguments/params from the expression
      // using the supplied value as the default value
      var callArgs = callExpr.Arguments
        .Select(arg => arg.CanReduce ? arg.Reduce() : arg)
        .Cast<ConstantExpression>()
        .Select(arg => arg.Value);

      var parameters = callExpr.Method
        .GetParameters()
        .Select(param => param.Name);

      var args = parameters.Zip(callArgs, Tuple.Create);

      foreach (var pair in args) {
        dict.Add(pair.Item1, pair.Item2);
      }

      // construct a new route from the finished dictionary
      var route = new Route(url, new MvcRouteHandler()) {
        Defaults = dict,
        DataTokens = new RouteValueDictionary {
          { "namespaces", _namespaces.ToArray() } 
        }
      };

      _routes.Add(route);

      // return self to enable chaining
      return this;
    }
  }
}