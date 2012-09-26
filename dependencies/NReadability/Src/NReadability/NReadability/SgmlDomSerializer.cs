﻿/*
 * NReadability
 * http://code.google.com/p/nreadability/
 * 
 * Copyright 2010 Marek Stój
 * http://immortal.pl/
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.Xml.Linq;

namespace NReadability
{
  /// <summary>
  /// A class for serializing a DOM to string.
  /// </summary>
  public class SgmlDomSerializer
  {
    #region Public methods

    /// <summary>
    /// Serializes given DOM (System.Xml.Linq.XDocument object) to a string.
    /// </summary>
    /// <param name="document">System.Xml.Linq.XDocument instance containing the DOM to be serialized.</param>
    /// <param name="domSerializationParams">Contains parameters that modify the behaviour of the output serialization.</param>
    /// <returns>Serialized representation of the DOM.</returns>
    public string SerializeDocument(XDocument document, DomSerializationParams domSerializationParams)
    {
      var result = document.ToString(domSerializationParams.PrettyPrint ? SaveOptions.None : SaveOptions.DisableFormatting);
      return result;
    }

    /// <summary>
    /// Serializes given DOM (System.Xml.Linq.XDocument object) to a string.
    /// </summary>
    /// <param name="document">System.Xml.Linq.XDocument instance containing the DOM to be serialized.</param>
    /// <returns>Serialized representation of the DOM.</returns>
    public string SerializeDocument(XDocument document)
    {
      return SerializeDocument(document, DomSerializationParams.CreateDefault());
    }

    #endregion

    #region Private helper methods

    private static void ProcessMetaElements(XElement headElement, DomSerializationParams domSerializationParams)
    {
      ProcessMetaContentTypeElement(headElement, domSerializationParams);
      ProcessMobileSpecificMetaElements(headElement, domSerializationParams);
      ProcessMetaGeneratorElement(headElement, domSerializationParams);
    }

    private static void ProcessMetaContentTypeElement(XElement headElement, DomSerializationParams domSerializationParams)
    {
      if (!domSerializationParams.DontIncludeContentTypeMetaElement)
      {
        XElement metaContentTypeElement =
          (from metaElement in headElement.GetChildrenByTagName("meta")
           where "content-type".Equals(metaElement.GetAttributeValue("http-equiv", ""), StringComparison.OrdinalIgnoreCase)
           select metaElement).FirstOrDefault();

        // remove meta 'http-equiv' element if present
        if (metaContentTypeElement != null)
        {
          metaContentTypeElement.Remove();
        }

        // add <meta name="http-equiv" ... /> element
        metaContentTypeElement =
          new XElement(
            XName.Get("meta", headElement.Name != null ? (headElement.Name.NamespaceName ?? "") : ""),
            new XAttribute("http-equiv", "Content-Type"),
            new XAttribute("content", "text/html; charset=utf-8"));

        headElement.AddFirst(metaContentTypeElement);
      }
    }

    private static void ProcessMobileSpecificMetaElements(XElement headElement, DomSerializationParams domSerializationParams)
    {
      XElement metaViewportElement =
        (from metaElement in headElement.GetChildrenByTagName("meta")
         where "viewport".Equals(metaElement.GetAttributeValue("name", ""), StringComparison.OrdinalIgnoreCase)
         select metaElement).FirstOrDefault();

      // remove meta 'viewport' element if present
      if (metaViewportElement != null)
      {
        metaViewportElement.Remove();
      }

      XElement metaHandheldFriendlyElement =
        (from metaElement in headElement.GetChildrenByTagName("meta")
         where "HandheldFriendly".Equals(metaElement.GetAttributeValue("name", ""), StringComparison.OrdinalIgnoreCase)
         select metaElement).FirstOrDefault();

      // remove meta 'HandheldFriendly' element if present
      if (metaHandheldFriendlyElement != null)
      {
        metaHandheldFriendlyElement.Remove();
      }

      if (!domSerializationParams.DontIncludeMobileSpecificMetaElements)
      {
        // add <meta name="HandheldFriendly" ... /> element
        metaHandheldFriendlyElement = new XElement(
          XName.Get("meta", headElement.Name != null ? (headElement.Name.NamespaceName ?? "") : ""),
          new XAttribute("name", "HandheldFriendly"),
          new XAttribute("content", "true"));

        headElement.AddFirst(metaHandheldFriendlyElement);
      }
    }

    private static void ProcessMetaGeneratorElement(XElement headElement, DomSerializationParams domSerializationParams)
    {
      if (!domSerializationParams.DontIncludeGeneratorMetaElement)
      {
        XElement metaGeneratorElement =
          (from metaElement in headElement.GetChildrenByTagName("meta")
           where "Generator".Equals(metaElement.GetAttributeValue("name", ""), StringComparison.OrdinalIgnoreCase)
           select metaElement).FirstOrDefault();

        // remove meta 'generator' element if present
        if (metaGeneratorElement != null)
        {
          metaGeneratorElement.Remove();
        }

        headElement.AddFirst(metaGeneratorElement);
      }
    }

    #endregion
  }
}
