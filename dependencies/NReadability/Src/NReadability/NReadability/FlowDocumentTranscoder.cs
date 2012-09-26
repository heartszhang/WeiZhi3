using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NReadability
{
    class FlowDocumentTranscoder
    {
        private static string StripInvalidXmlChars(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var c in txt)
            {
                //Char ::= #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]
                //\u4e00-\u9fa5
                if (c == 0x9
                    || (c >= 0x20 && c < 0xd7ff)
                    || (c >= 0xe000 && c < 0xfffd)
                    || (c >= 0x4e00 && c < 0x9fa5))
                    sb.Append(c);
                else sb.Append(' ');
            }
            return sb.ToString();
        }
        private static readonly Regex _NormalizeSpacesRegex = new Regex("\\s{2,}", RegexOptions.Compiled);
        internal string GetInnerText(string result, bool preservespace)
        {
            result = (result ?? "").Trim();

            if (!preservespace)
            {
                return _NormalizeSpacesRegex.Replace(result, " ");
            }
            result = new Regex(@"<.*>", RegexOptions.Compiled).Replace(result, "");

            return result;
        }

        private static string GetElementName(XElement element)
        {
            return element.Name.LocalName ?? "";
        }
        private XElement CreateHyperlink(string href, string a)
        {
            var ele = new XElement(_ns + "Hyperlink") { Value = a };
            ele.SetAttributeValue("NavigateUri", href);
            return ele;
        }

        private XElement CreateFigure(string href)
        {
            var fig = new XElement(_ns + "Figure");
            fig.SetAttributeValue("Padding", "0");
            fig.SetAttributeValue("Margin", "12,0,0,0");

            var block = new XElement(_ns + "BlockUIContainer");
            var img = new XElement(_ns + "Image");
            img.SetAttributeValue("Source", href);
            block.Add(img);
            fig.Add(block);
            return fig;
        }
        XElement CreateParagraph()
        {
            return CreateElement("Paragraph");
        }
        XElement CreateLineBreak()
        {
            return CreateElement("LineBreak");
        }
        IEnumerable<XElement> CreateRuns(XText node,bool preservespace)
        {
            var rtn = new List<XElement>();
            if(!preservespace)
            {
                var run = CreateRun(GetInnerText(node.Value, false));
                rtn.Add(run);
                return rtn;
            }
            var lines = node.Value.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in lines)
            {
                var run = CreateRun(line);
                rtn.Add(run);
                rtn.Add(CreateLineBreak());
            }
            if (rtn.Count > 0)
                rtn.Remove(rtn.Last());
            return rtn;
        } 
        IEnumerable<XElement> ConvertToParagraphs(XNode node, bool preservespace)
        {
            var rtn = new List<XElement>();
            if (node.NodeType == XmlNodeType.Text)
            {
                var lines = CreateRuns((XText)node, preservespace);
                var p = CreateParagraph();
                foreach(var line in lines)
                    p.Add(line);
                rtn.Add(p);
                return rtn;
            }
            if (node.NodeType != XmlNodeType.Element)
            {
                Debug.WriteLine("encounter unknown node {0}", node.NodeType);
                return rtn;
            }
            var n = (XElement)node;
            /*            if(!n.HasElements && !string.IsNullOrEmpty(n.Value))
                        {
                            var txt = GetInnerText(node);
                            var p = CreateParagraph();
                            p.Value = txt;
                            rtn.Add(p);
                            return rtn;
                        }
             * */
            var temp = CreateParagraph();
            var last_is_line_in_temp = false;
            for (var x = n.FirstNode; x != null; x = x.NextNode)
            {
                if (IsBlockLevel(x))
                {
                    if (temp.HasElements)
                    {
                        rtn.Add(temp);
                        temp = CreateParagraph();
                    }
                    var nm = ((XElement) x).Name.LocalName;
                    var paras = ConvertToParagraphs(x , "pre" == nm || preservespace);
                    rtn.AddRange(paras.Where(p => p.HasElements));
                }
                else
                {
                    var inlines = ConvertToInlines(x, preservespace);
                    if (inlines.Count == 0)
                        continue;
                    var islinelevel = IsLineLevelElement(x);//不是blocklevel也不是linelevel，直接认为是inlinelevel
                    if (islinelevel)
                    {
                        //每段开始的空行需要去掉，段尾的空行后面会去掉
                        MergeIntoParagraph(temp.HasElements, inlines, temp);
                        last_is_line_in_temp = true;
                    }
                    else
                    {
                        MergeIntoParagraph(last_is_line_in_temp, inlines, temp);
                        last_is_line_in_temp = false;
                    }
                }
            }
            if (temp.HasElements)
                rtn.Add(temp);
            if ("pre" == n.Name.LocalName)
            {
                foreach (var p in rtn)
                {
                    p.SetAttributeValue("Padding","12");
                    p.SetAttributeValue("Background", "WhiteSmoke");
                    p.SetAttributeValue("TextIndent", "0");
                    p.SetAttributeValue(XNamespace.Xml + "space", "preserve");//new XAttribute(XNamespace.Xml + "space", "preserve")
                }
            }
            return rtn;
        }
        void MergeIntoParagraph(bool insertlinebreak, IEnumerable<XElement> inlines, XElement paragraph)
        {
            if (insertlinebreak)
            {
                paragraph.Add(CreateLineBreak());
            }
            var breakahead = true;

            foreach (var i in inlines)
            {
                var b = IsElementBreak(i);
                if (!IsElementBreak(i))
                {
                    paragraph.Add(i);
                    breakahead = false;
                }
                else if (!breakahead)
                {
                    paragraph.Add(i);
                    breakahead = true;
                }
            }
            //删除段尾的换行
            if (IsElementBreak((XElement)paragraph.LastNode))
            {
                paragraph.LastNode.Remove();
            }
        }
        bool IsElementBreak(XElement x)
        {
            return x.Name.LocalName == "LineBreak";
        }
        XElement CreateRun(string text)
        {
            var rtn = CreateElement("Run", text);
            return rtn;
        }
        List<XElement> ConvertToInlines(XNode node, bool preservespace)
        {
            var rtn = new List<XElement>();
            if (node.NodeType == XmlNodeType.Text)
            {
                var lines = CreateRuns((XText) node, preservespace);
                rtn.AddRange(lines);

                return rtn;
            }
            if (node.NodeType != XmlNodeType.Element)
            {
                Debug.WriteLine("encounter unknown node {0}", node.NodeType);
                return rtn;
            }
            var n = (XElement)node;
            var name = n.Name.LocalName;
            if ("br" == name)
            {
                rtn.Add(CreateLineBreak());
                //return rtn;
            }
            else if ("img" == name)
            {
                var href = n.GetAttributeValue("src", string.Empty);
                rtn.Add(CreateFigure(href));
            }
            else if ("a" == name)
            {
                var href = n.GetAttributeValue("href", string.Empty);
                //var t = n.GetAttributeValue("title", string.Empty);
                var cont = GetInnerText(n.Value, false);
                if (!string.IsNullOrEmpty(cont))
                {
                    var a = CreateHyperlink(href, cont);
                    rtn.Add(a);
                }
            }
            else
            {
                for (var x = n.FirstNode; x != null; x = x.NextNode)
                {
                    var inlines = ConvertToInlines(x, preservespace);
                    rtn.AddRange(inlines);
                }
            }
            if ("strong" == name)
            {
                foreach (var i in rtn)
                {
                    i.SetAttributeValue("FontWeight", "SemiBold");
                }
            }else if("span" == name)
            {
                foreach(var i in rtn)
                {
                    i.SetAttributeValue("Foreground", "#FF008000");
                }
            }
            return rtn;
        }

        private static bool IsBlockLevel(XNode n)
        {
            if (n.NodeType != XmlNodeType.Element)
                return false;
            var node = (XElement)n;
            var name = GetElementName(node);
            var style = node.GetAttributeValue("style", string.Empty);
            if (style.Contains("display:inline"))
                return false;
            return "div" == name || "h1" == name || "h2" == name || "h3" == name
                   || "h4" == name || "h5" == name || "ul" == name || "ol" == name || "dl" == name
                   || "table" == name || "body" == name || name == "pre";
        }

        private static bool IsLineLevelElement(XNode n)
        {
            if (n.NodeType != XmlNodeType.Element)
                return false;
            var node = (XElement)n;
            var name = GetElementName(node);
            return name == "li" || name == "dt" || name == "br" || name == "p";
        }

        private static XNamespace _ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        private XElement CreateElement(string name)
        {
            return new XElement(_ns + name);
        }
        private XElement CreateElement(string name, string val)
        {
            return new XElement(_ns + name){Value = val};
        }
        public XDocument Transcode(XElement article, XElement content)
        {
            var doc = XDocument.Parse(_readingFlowStyle);
            var root = doc.Root;
            Debug.Assert(root != null);
            if (article != null)
            {
                var title = CreateParagraph();
                title.Value = GetInnerText(article.Value,false);
                root.Add(title);
            }
            var paras = ConvertToParagraphs(content,false);
            foreach (var p in paras.Where(p => p.HasElements))
            {
                root.Add(p);
            }
            return doc;
        }
        private static string _readingFlowStyle =
            @"<FlowDocument xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" ColumnWidth=""400"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
  <FlowDocument.Resources>
    <Style x:Key=""{x:Type FlowDocument}"" TargetType=""{x:Type FlowDocument}"">
      <Setter Property=""PagePadding"" Value=""0"" />
      <Setter Property=""FontSize"" Value=""14.667"" />
      <Setter Property=""FontFamily"" Value=""Segoe UI, Microsoft YaHei UI, Microsoft YaHei,Microsoft JhengHei UI,Microsoft JhengHei, Segoe UI Light, Lucida Sans Unicode, Verdana"" />
      <Setter Property=""IsOptimalParagraphEnabled"" Value=""True"" />
      <Setter Property=""IsHyphenationEnabled"" Value=""True"" />
      <Setter Property=""Block.LineHeight"" Value=""26"" />
      <Setter Property=""IsColumnWidthFlexible"" Value=""True"" />
      <Setter Property=""IsHyphenationEnabled"" Value=""True"" />
    </Style>
    <Style x:Key=""DocTitleStyle"" TargetType=""{x:Type Paragraph}"">
      <Setter Property=""FontSize"" Value=""18.667"" />
      <Setter Property=""TextAlignment"" Value=""Center"" />
      <Setter Property=""Block.LineHeight"" Value=""24"" />
    </Style>
    <Style TargetType=""{x:Type Paragraph}"">
      <Setter Property=""Margin"" Value=""0,14"" />
<Setter Property=""TextIndent"" Value=""36""/>
    </Style>
    <Style x:Key=""{x:Type Image}"" TargetType=""{x:Type Image}"">
      <Setter Property=""MaxHeight"" Value=""280"" />
      <Setter Property=""StretchDirection"" Value=""DownOnly"" />
      <Setter Property=""Stretch"" Value=""Uniform"" />
    </Style>
    <Style x:Key=""DocFootnoteStyle"" TargetType=""{x:Type Paragraph}"">
      <Setter Property=""TextAlignment"" Value=""Right"" />
      <Setter Property=""FontSize"" Value=""12"" />
    </Style>
    <Style x:Key=""{x:Type Figure}"" TargetType=""{x:Type Figure}"">
      <Setter Property=""Width"" Value=""0.25 content"" />
      <Setter Property=""FontSize"" Value=""12"" />
    </Style>
  </FlowDocument.Resources>
</FlowDocument>";
    }
}