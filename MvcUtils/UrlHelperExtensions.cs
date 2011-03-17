
using System.Web.Mvc;

namespace MvcUtils
{
  public static class UrlHelperExtensions
  {
    public static string AbsoluteAction(this UrlHelper helper, string action,
      string controller, object routeValues = null)
    {
      var requestUri = helper.RequestContext.HttpContext.Request.Url;

      string actionUrl = (routeValues == null) ?
        helper.Action(action, controller) :
        helper.Action(action, controller, routeValues);

      return string.Format("{0}://{1}{2}",
        requestUri.Scheme,
        requestUri.Authority,
        actionUrl);
    }
  }
}
