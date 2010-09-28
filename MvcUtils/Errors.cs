
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

    public static Exception Forbidden()
    {
      throw new HttpException(403, "Forbidden.");
    }
  }
}
