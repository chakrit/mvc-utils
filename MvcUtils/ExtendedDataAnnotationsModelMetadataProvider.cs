
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcUtils
{
  public class ExtendedDataAnnotationsModelMetadataProvider :
    DataAnnotationsModelMetadataProvider
  {
    protected override ModelMetadata CreateMetadata(
      IEnumerable<Attribute> attributes, Type containerType,
      Func<object> modelAccessor, Type modelType,
      string propertyName)
    {
      var metadata = base.CreateMetadata(attributes, containerType,
        modelAccessor, modelType, propertyName);

      // apply our custom set of attributes
      var showAttrs = attributes.OfType<ShowAttribute>();
      foreach (var showAttr in showAttrs)
        showAttr.Apply(metadata);

      return metadata;
    }
  }
}
