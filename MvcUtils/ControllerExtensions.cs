
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcUtils
{
  public static class ControllerExtensions
  {
    public static ActionResult RedirectToAction2<TController>(this TController controller,
      Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      return new RedirectToRouteResult(action.ExtractRouteValues());
    }

    public static ActionResult RedirectToAction2<TController>(this ControllerBase controller,
      Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      return new RedirectToRouteResult(action.ExtractRouteValues());
    }
  }
}
