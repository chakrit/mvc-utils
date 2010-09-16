
using System;
using System.Web;

namespace MvcUtils
{
  public static class Errors
  {
    public static Exception NotFound()
    {
      return new HttpException(404, "Not Found.");
    }
  }
}
