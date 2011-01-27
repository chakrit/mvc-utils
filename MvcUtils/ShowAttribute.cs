
using System;
using System.Web.Mvc;

namespace MvcUtils
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class ShowAttribute : Attribute
  {
    private bool _showForEdit;
    private bool _showForDisplay;

    public ShowAttribute(bool forEdit, bool forDisplay)
    {
      _showForEdit = forEdit;
      _showForDisplay = forDisplay;
    }

    public void Apply(ModelMetadata metadata)
    {
      metadata.ShowForDisplay = _showForDisplay;
      metadata.ShowForEdit = _showForEdit;
    }
  }
}
