﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Weibo.ViewModels.StatusRender
{
    public static class WeiboParser
    {
        public static List<Token> Parse(this string text)
        {
            var rtn = new List<Token>();
            var begin = 0;
            var end = text.Length;
            var last = new Token();

            while (begin < end)
            {
                Token t = null;
                var b = begin;
                var c = text[begin];
                if (c > 0xdfff && c < 0xe900)//忽略自定义字符
                {
                    ++begin;
                    continue;
                }
                switch (c)
                {
                    case '/':
                        t = eat_sharedfrom(text, ref b, end);
                        break;
                    case '@':
                        t = eat_name(text, ref b, end);
                        break;
                    case 'h':
                        t = eat_hyperlink(text, ref b, end);
                        break;
                    case '#':
                        t = eat_topic(text, ref b, end, c);
                        break;
                    case '【':
                        t = eat_topic(text, ref b, end, '】');
                        break;
                    case '《':
                        t = eat_topic(text, ref b, end, '》');
                        break;
                    case '『':
                        t = eat_topic(text, ref b, end, '』');
                        break;
                    case '[':
                        t = eat_emotion(text, ref b, end);
                        break;
                    case '回':
                        t = eat_replyto(text, ref b, end);
                        break;
                    case '？':
                    case '。':
                    case '!':
                    case '！':
                    case '~':
                    case '，':
                    case ',':
                    case '；':
                    case '：':
                    case ':':
                    case '.':
                    case '-':
                        t = eat_punctuation(text, ref b, end,c);
                        break;
                    case '"':
                        t = eat_quote(text, ref b, end, '"');
                        break;
                    case '“':
                        t = eat_quote(text, ref b, end, '”');
                        break;
                    case '（':
                        t = eat_quote(text, ref b, end, '）');
                        break;
                    case '(':
                        t = eat_quote(text, ref b, end, '(');
                        break;
                    case '「':
                        t = eat_quote(text, ref b, end, '」');
                        break;
                }
                if (t == null || (string.IsNullOrEmpty(t.text))) //没有匹配上
                {
                    last.text += c;
                    ++begin;
                }
                else
                {
                    if (last.text.Length > 0)
                    {
                        rtn.Add(last);
                        last = new Token();
                    }
                    rtn.Add(t);

                    begin = b;
                }
            }
            if (last.text.Length > 0)
            {
                rtn.Add(last);
            }

            return rtn;

        }

        private static bool is_invalid_name_char(char c)
        {
            var invalid_name_chars = new[]
                                         {'」','「',
                                             ':', '：', '\t', ' ', ' ', '，', '。', '；', '】','@',
                                             ']', '/', '[', '【', '\\', ';','《','》',  '『' ,  '』','>','<',
                                             ',', '、', '\"', '\'', '）', '（', '(', ')', '\r', '\n','#'
                                         };
            return invalid_name_chars.Any(i => c == i);
        }
        private static Token eat_replyto(string text, ref int begin, int end)
        {
            var b = begin;
            if (b + 2 >= end || text[b + 1] != '复' || text[b + 2] != '@')
            {
                return null;
            }
            b += 2;
            var name = eat_name(text, ref b, end);
            if (name == null || string.IsNullOrEmpty(name.text) || b >= end || (text[b] != ':' && text[b] != '；'))
                return null;
            begin = ++b;//eat :
            name.tag = TokenTypes.ReplyTo;
            return name;
        }
        private static Token eat_name(string text, ref int begin, int end)
        {
            var rtn = new Token { text = "", tag = TokenTypes.Name };
            const string h = "http://";
            var b = begin;
            ++begin;
            while (begin < end && !char.IsWhiteSpace(text[begin]) && !is_invalid_name_char(text[begin]))
            {
                if (text[begin] == 'h' && begin + h.Length < end)
                {
                    var t = text.Substring(begin, h.Length > end - begin ? end - begin : h.Length);

                    if (String.Compare(t, h, StringComparison.OrdinalIgnoreCase) == 0)
                        break;
                }

                rtn.text += text[begin++];
            }
            if (rtn.text.Length > 16)
            {
                //最多16个字
                rtn.text = "";
                begin = b;
            }
            return rtn;
        }
        static bool is_quote(char c)
        {
            return c == '”' || c == '“' || c == '"' || c == ')' || c == '）' || c == '」' || c == '(' || c == '（' || c == '「';
        }
        private static Token eat_quote(string text, ref int begin, int end, char endchar)
        {            
            var rtn = new Token {tag = TokenTypes.Quote};
            Debug.Assert(is_quote(text[begin]));
            ++begin;
            while (begin < end && text[begin] != endchar)
            {
                rtn.text += text[begin++];
            }
            if (begin == end)
                rtn.text = string.Empty;
            else ++begin;
            rtn.flag = endchar;
            return rtn;
        }
        private static Token eat_emotion(string text, ref int begin, int end)
        {
            var rtn = new Token { text = "", tag = TokenTypes.Emotion };

            if (text[begin] == '[')
            {
                ++begin;
                while (begin < end && text[begin] != ']')
                {
                    rtn.text += text[begin++];
                }
                if (begin == end) //cann't find ]
                    rtn.text = "";
                else ++begin;
            }
            return rtn;
        }

        private static Token eat_punctuation(string text, ref int begin, int end, char punc)
        {
            var rtn = new Token { text = "", tag = TokenTypes.Punctuation };
            while (begin < end && (text[begin].IsPunctuationExt() || char.IsWhiteSpace(text[begin])))
            {
                rtn.text += text[begin++];
            }
            rtn.flag = punc;
            return rtn;
        }

        private static Token eat_topic(string text, ref int begin, int end, char endchar)
        {
            var rtn = new Token { text = "", tag = TokenTypes.Topic };
            ++begin;
            while (begin < end && text[begin] != endchar)
            {
                rtn.text += text[begin++];
            }
            if (begin == end) //cann't find #
                rtn.text = "";
            else ++begin;
            rtn.text = rtn.text.Trim();
            rtn.flag = endchar;
            return rtn;
        }

        private static Token eat_sharedfrom(string text, ref int beg, int end)
        {
            var begin = beg;
            var rtn = new Token { text = string.Empty, tag = TokenTypes.CopyedFrom };
            if (begin + 2 < end && text[begin] == '/' && text[begin + 1] == '/' && text[begin + 2] == '@')
            {
                begin += 3;
                while (begin < end && !char.IsWhiteSpace(text[begin]) && !is_invalid_name_char(text[begin]))
                {
                    rtn.text += text[begin++];
                }
            }
            if (begin + 3 < end && text[begin] == '/' && text[begin + 1] == '/' && text[begin + 2] == ' ' &&
                text[begin + 3] == '@')
            {
                begin += 4;
                while (begin < end && !char.IsWhiteSpace(text[begin]) && !is_invalid_name_char(text[begin]))
                {
                    rtn.text += text[begin++];
                }
            }
            if (begin < end && (text[begin] == '：' || text[begin] == ':'))
                ++begin;
            if (rtn.text.Length > 16)
            {//最多16个字
                rtn.text = string.Empty;
            }
            if (!string.IsNullOrEmpty(rtn.text))
                beg = begin;
            return rtn;
        }

        private static Token eat_hyperlink(string text, ref int begin, int end)
        {
            var rtn = new Token { text = "", tag = TokenTypes.Hyperlink };
            const string h = "http://";
            var i = text.Substring(begin, (h.Length < end - begin) ? h.Length : (end - begin));

            if (String.Compare(h, i, StringComparison.OrdinalIgnoreCase) == 0)
            {
                while (begin < end)
                {
                    char t = text[begin];
                    if ((char.IsLetterOrDigit(t) && t < 128) || t == '.' || t == '/' || t == ':' || t == '&' || t == '%' ||
                        t == '?' || t == '_' || t == '=' || t == '#' || t == '-')
                        rtn.text += t;
                    else break;
                    ++begin;
                }
            }
            return rtn;
        }
    }
}