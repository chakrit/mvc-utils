
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcUtils
{
  public static class UrlHelperExtensions
  {
    public static string Action<TController>(this UrlHelper helper,
      Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      var dict = action.ExtractRouteValues();
      return helper.Action((string)dict["Action"], dict);
    }
  }
}
