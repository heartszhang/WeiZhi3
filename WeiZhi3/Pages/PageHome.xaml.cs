using Artefact.Animation;

namespace WeiZhi3.Pages
{
	public partial class PageHome
	{
		public PageHome()
		{
			this.InitializeComponent();
		}

		private void ToolsetPopup(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(0, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
		}
        private void ToolsetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(_tool_container.ActualWidth, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
		}
        private void NavisetPopup(object sender, System.Windows.Input.MouseEventArgs e)
        {
			_navi_container.OffsetTo(0, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
        }
        private void NavisetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _navi_container.OffsetTo(-_navi_container.ActualWidth, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
      }
	}
}