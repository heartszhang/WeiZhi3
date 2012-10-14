using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weibo.ViewModels;

namespace WeiZhi3.Attached
{
    class TabCompletionInterService : TabCompletionService
    {

        #region Inteligence (Attached DependencyProperty)

        public static readonly DependencyProperty InteligenceProperty =
            DependencyProperty.RegisterAttached("Inteligence", typeof(TabCompletionInterService), typeof(TabCompletionInterService));

        public static void SetInteligence(DependencyObject o, TabCompletionInterService value)
        {
            o.SetValue(InteligenceProperty, value);
        }

        public static TabCompletionInterService GetInteligence(DependencyObject o)
        {
            return (TabCompletionInterService)o.GetValue(InteligenceProperty);
        }


        #endregion


        #region MentionOne (Attached DependencyProperty)

        public static readonly DependencyProperty MentionOneProperty =
            DependencyProperty.RegisterAttached("MentionOne", typeof(string), typeof(TabCompletionInterService), new PropertyMetadata(new PropertyChangedCallback(OnMentionOneChanged)));

        public static void SetMentionOne(DependencyObject o, string value)
        {
            o.SetValue(MentionOneProperty, value);
        }

        public static string GetMentionOne(DependencyObject o)
        {
            return (string)o.GetValue(MentionOneProperty);
        }

        private static void OnMentionOneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var v = (string)e.NewValue;
            if (v == null)
                return;
            var inteli = GetInteligence(d);
            if (inteli == null)
            {
                SetInteligence(d, inteli = new TabCompletionInterService());
                inteli.InstallTextBoxHook((TextBox)d);
            }
            inteli.AddWord("@" + v + "：");
        }

        #endregion

        #region References (Attached DependencyProperty)

        public static readonly DependencyProperty ReferencesProperty =
            DependencyProperty.RegisterAttached("References", typeof(string), typeof(TabCompletionInterService), new PropertyMetadata(OnReferencesChanged));

        public static void SetReferences(DependencyObject o, string value)
        {
            o.SetValue(ReferencesProperty, value);
        }

        public static string GetReferences(DependencyObject o)
        {
            return (string)o.GetValue(ReferencesProperty);
        }

        private static void OnReferencesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inteli = GetInteligence(d);
            if (inteli == null)
            {
                SetInteligence(d, inteli = new TabCompletionInterService());
                inteli.InstallTextBoxHook((TextBox)d);
            }
            var v = (string)e.NewValue;
            if (v == null)
                return;
            inteli.Add("//@", v);

        }

        #endregion
        
        public static readonly TabCompletionService Service = new TabCompletionService();

        public new string Suffix(string prefix)
        {
            return _entries.ContainsKey(prefix) ? _entries[prefix].First() : Service.Suffix(prefix);
        }

        public string SuffixNext(string prefix, string sufix)
        {
            var rtn = SuffixNext(prefix, sufix, false);
            if (!string.IsNullOrEmpty(rtn))
                return rtn;
            rtn= Service.SuffixNext(prefix, sufix,false);
            if (string.IsNullOrEmpty(rtn))
                rtn = Suffix(prefix);
            if (rtn == sufix)
                rtn = string.Empty;
            return rtn;
        }
        void InstallTextBoxHook(TextBox box)
        {
            box.TextChanged += TextBoxTextChanged;
            box.Unloaded += TextBoxUnloaded;
            box.KeyDown += TextBoxKeyDown;
        }

        void TextBoxUnloaded(object sender, RoutedEventArgs e)
        {
            var box = (TextBox) sender;
            box.Unloaded -= TextBoxUnloaded;
            box.TextChanged -= TextBoxTextChanged;
            box.KeyDown -= TextBoxKeyDown;
        }

        void TextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Tab) return;
            e.Handled = true;
            var box = (TextBox) sender;
            var suf = box.SelectedText;
            var prefix = GetPrefix((TextBox) sender);
            var next = SuffixNext(prefix, suf);
            if(string.IsNullOrEmpty(next))
            {
                box.SelectionStart = box.Text.Length;
            }else
            {
                box.SelectedText = next;
            }
            return;
        }

        void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var box = (TextBox)sender;
            if (box.SelectedText.Length > 0)
                return;
            var added = false;
            //如果所有的修改都是删除内容，就没有必要进行补全了
            foreach (var change in e.Changes.Where(change => change.AddedLength > 0))
            {
                added = true;
            }
            if (!added)
                return;
            if (e.Changes.Any(change => change.RemovedLength > 0))
            {
                return;
            }
            var pre = GetPrefix(box);
            var suffix = Suffix(pre);
            if (!string.IsNullOrEmpty(suffix))
                box.SelectedText = suffix;
        }
        enum prefix_statuses
        {
            normal = 0,
            slash = 1,//from any
            slash2 = 2, //from slash
            slashesat = 3,//from slash2
            at = 4,//from any except slash2
        }
        private string GetPrefix(TextBox box)
        {
            var stat = prefix_statuses.normal;

            var prefix = string.Empty;
            for (var i = Math.Max(0, box.SelectionStart - 16); i < box.SelectionStart; ++i)
            {
                var c = box.Text[i];

                if (c == '@')
                {
                    if (stat != prefix_statuses.slash2)
                    {
                        prefix = string.Empty;
                        stat = prefix_statuses.at;
                    }
                    else
                    {
                        stat = prefix_statuses.slashesat;
                    }
                    prefix += c;
                }
                else if (c == '/')
                {
                    if (stat == prefix_statuses.slash)
                    {
                        stat = prefix_statuses.slash2;
                    }
                    else if (stat == prefix_statuses.slash2)
                    {
                        prefix = "/";
                    }
                    else
                    {
                        prefix = string.Empty;
                        stat = prefix_statuses.slash;
                    }
                    prefix += c;
                }
                else prefix += c;
            }
            return prefix;
        }

    }
}