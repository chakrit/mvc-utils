
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
      // ensure the controller namespace is included
      _namespaces.Add(typeof(TController).Namespace);

      // construct a new route from the finished dictionary
      var route = new Route(url, new MvcRouteHandler()) {
        Defaults = action.ExtractRouteValues(),
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