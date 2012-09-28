using System.Windows;

namespace Bot
{
    /// <summary>
    ///     Internal interface for scrolling information of elements which 
    ///     need stack like measure.
    /// </summary>
    internal interface IStackMeasureScrollData
    {
        Vector Offset { get; set; }
        Size Viewport { get; set; }
        Size Extent { get; set; }
        Vector ComputedOffset { get; set; }
        void SetPhysicalViewport(double value);
    }
}