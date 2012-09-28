using System.Windows.Controls;

namespace Bot
{
    /// <summary> 
    ///     Internal interface for elements which needs stack like measure
    /// </summary> 
    internal interface IStackMeasure
    {
        bool IsScrolling { get; }
        UIElementCollection InternalChildren { get; }
        Orientation Orientation { get; }
        bool CanVerticallyScroll { get; }
        bool CanHorizontallyScroll { get; }
        void OnScrollChange();
    }
}