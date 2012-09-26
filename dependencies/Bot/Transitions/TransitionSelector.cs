using System.Windows;

namespace Bot.Transitions
{
    public class TransitionSelector : DependencyObject
    {
        public virtual Transition SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
}