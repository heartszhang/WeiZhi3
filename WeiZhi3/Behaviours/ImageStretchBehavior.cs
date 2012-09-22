using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Artefact.Animation;

namespace WeiZhi3.Behaviours
{
    internal sealed class ImageStretchBehavior : Behavior<Image>
    {

        public double DesiredSize
        {
            get { return (double)GetValue(DesiredSizeProperty); }
            set { SetValue(DesiredSizeProperty, value); }
        }
        public static DependencyProperty DesiredSizeProperty = 
            DependencyProperty.Register("DesiredSize", typeof(double), typeof(ImageStretchBehavior), new PropertyMetadata(440.0));
        
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.Unloaded +=OnUnloaded;
//            AssociatedObject.NormalizeTransformGroup();
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= OnUnloaded;
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
        }

        void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AssociatedObject.ActualWidth < double.Epsilon  || AssociatedObject.ActualWidth >= AssociatedObject.ActualHeight)
                return;
            var width = AssociatedObject.Source.Width;
            var height = AssociatedObject.Source.Height;
            if (width >= height || (width * height) < double.Epsilon)//landscape不处理,没有数据的图不处理
                return;
            if (width <= DesiredSize && height < MaxHeight )//小图不处理
                return;
            var targeth = DesiredSize;
            var desiredh = height / width * DesiredSize;
            if (desiredh < MaxHeight)//没超过页面设置界限
                return;
            if (AssociatedObject.MaxHeight < desiredh)
            {
                targeth = desiredh;
            }
            ArtefactAnimator.AddEase(AssociatedObject, FrameworkElement.MaxHeightProperty, targeth, 0.2,
                                     AnimationTransitions.ExpoEaseOut, 0);
        }

        private double _max_height = 640.0;
        internal double MaxHeight { get { return _max_height; } set { _max_height = value; } }
    }
}