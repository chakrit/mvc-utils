
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcUtils
{
  public static class ControllerExtensions
  {
    public static ActionResult RedirectToAction<TController>(this TController controller,
      Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      return new RedirectToRouteResult(action.ExtractRouteValues());
    }
  }
}
