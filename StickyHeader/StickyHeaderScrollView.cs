using Android.Content;
using Android.Views;
using Android.Widget;
using StickyHeader.Animator;

namespace StickyHeader
{
	public class StickyHeaderScrollView : StickyHeaderView
	{
		private readonly ScrollView scrollView;

		public StickyHeaderScrollView(Context context, View header, int minHeightHeader, HeaderAnimator headerAnimator, ScrollView scrollView)
			: base(context, header, minHeightHeader, headerAnimator)
		{
			this.scrollView = scrollView;

			// scroll events
			scrollView.ViewTreeObserver.ScrollChanged += (sender, e) => headerAnimator.OnScroll(-scrollView.ScrollY);
		}

		protected override void SetHeightHeader(int value)
		{
			base.SetHeightHeader(value);

			// creating a placeholder
			// adding a padding to the scrollview behind the header
			scrollView.SetPadding(
				scrollView.PaddingLeft,
				scrollView.PaddingTop + value,
				scrollView.PaddingRight,
				scrollView.PaddingBottom);
			scrollView.SetClipToPadding(false);
		}
	}
}