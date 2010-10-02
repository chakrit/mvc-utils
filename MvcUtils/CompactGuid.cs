
using System;

namespace MvcUtils
{
  /// <summary>
  /// An implicitly-convertible buffer that automatically converts between
  /// a Guid and its compact url-safe base64-encoded string representation.
  /// </summary>
  /// <remarks>
  /// Reference implementation:
  /// http://www.singular.co.nz/blog/archive/2007/12/20/shortguid-a-shorter-and-url-friendly-guid-in-c-sharp.aspx
  /// </remarks>
  public struct CompactGuid
  {
    private Guid _guid;
    private string _str;


    public Guid Guid { get { return this; } }

    public CompactGuid(string s) { _str = s; _guid = Guid.Empty; }
    public CompactGuid(Guid guid) { _guid = guid; _str = null; }


    public static CompactGuid NewGuid()
    {
      return Guid.NewGuid();
    }


    public static implicit operator CompactGuid(string source) { return new CompactGuid(source); }
    public static implicit operator CompactGuid(Guid source) { return new CompactGuid(source); }

    public static implicit operator string(CompactGuid source)
    {
      return (source._str != null) ?
        source._str :
        (source._str = Pack(source._guid));
    }

    public static implicit operator Guid(CompactGuid source)
    {
      return (source._guid != Guid.Empty) ?
        source._guid :
        (source._guid = Unpack(source._str));
    }


    public static string Pack(Guid guid)
    {
      return Convert
        .ToBase64String(guid.ToByteArray())
        .Replace('/', '_') // url-safe character replacements
        .Replace('+', '-')
        .Substring(0, 22);
    }

    public static Guid Unpack(string sguid)
    {
      return new Guid(Convert.FromBase64String(sguid
        .Replace('-', '+')
        .Replace('_', '/')
        + "=="));
    }


    public override string ToString() { return this; }
  }
}
