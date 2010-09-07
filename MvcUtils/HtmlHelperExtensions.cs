
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcUtils
{
  public static class HtmlHelperExtensions
  {
    public static MvcForm BeginForm<TController>(this HtmlHelper helper, Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      return helper.BeginForm(action.ExtractRouteValues());
    }

    public static MvcHtmlString ActionLink<TController>(this HtmlHelper helper, string linkText, Expression<Action<TController>> action)
      where TController : ControllerBase
    {
      var dict = action.ExtractRouteValues();
      return helper.ActionLink(linkText, (string)dict["Action"], dict);
    }
  }
}
