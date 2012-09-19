namespace Weibo.ViewModels.StatusRender
{
    public static class CharHelper
    {
        public static bool IsPunctuationExt(this char c)
        {
            return c == '£¿' || c == '¡£' || c == '£¡' || c == '~'
                   || c == '£¬' || c == ',' || c == '£»' || c == ';' || c == '"'
                   || c == '¡°' || c == '¡±' || c == '£º' || c == ':' || c == '¡¶' || c == '¡¾'
                   || c == '¡¿' || c == '¡·' || c == '¡º' || c == '¡»' || c == ']' || c == '(' || c == ')'
                   || c == '£¨' || c == '£©';
        }
    }
}