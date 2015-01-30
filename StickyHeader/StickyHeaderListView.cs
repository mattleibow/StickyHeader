using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using StickyHeader.Animator;

namespace StickyHeader
{
	public class StickyHeaderListView : StickyHeaderView
	{
		private readonly ListView listView;

		public StickyHeaderListView(Context context, View header, int minHeightHeader, HeaderAnimator headerAnimator, ListView listView)
			: base(context, header, minHeightHeader, headerAnimator)
		{
			this.listView = listView;

			// fake header
			var lp = new AbsListView.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0);
			fakeHeader = new View(context)
			{
				Visibility = ViewStates.Invisible,
				LayoutParameters = lp
			};
			this.listView.AddHeaderView(fakeHeader);
		
			// scroll events
			this.listView.Scroll += (sender, e) =>
			{
				var scrolledY = -CalculateScrollYList();
				headerAnimator.OnScroll(scrolledY);
			};
		}

		protected virtual int CalculateScrollYList()
		{
			View c = listView.GetChildAt(0);
			if (c == null)
			{
				return 0;
			}

			//TODO support more than 1 header?

			int firstVisiblePosition = listView.FirstVisiblePosition;
			int top = c.Top;

			int headerHeight = 0;
			if (firstVisiblePosition >= 1)
			{
				//TODO >= number of header
				headerHeight = heightHeader;
			}

			return -top + firstVisiblePosition*c.Height + headerHeight;
		}
	}
}