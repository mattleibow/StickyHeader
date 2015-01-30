using System;
using Android.Content;
using Android.Views;
using StickyHeader.Animator;

namespace StickyHeader
{
	public abstract class StickyHeaderView
	{
		protected readonly Context context;
		protected readonly View header;
		protected readonly HeaderAnimator headerAnimator;
		protected readonly int minHeightHeader;

		protected View fakeHeader;
		protected int heightHeader;
		protected int maxHeaderTransaction;

		protected StickyHeaderView(Context context, View header, int minHeightHeader, HeaderAnimator headerAnimator)
		{
			this.context = context;
			this.header = header;
			this.minHeightHeader = minHeightHeader;
			this.headerAnimator = headerAnimator;

			MeasureHeaderHeight();

			headerAnimator.SetupAnimator(header, minHeightHeader, heightHeader, maxHeaderTransaction);
		}

		protected virtual void SetHeightHeader(int value)
		{
			heightHeader = value;

			// some implementations don't use a fake header
			if (fakeHeader != null)
			{
				ViewGroup.LayoutParams lpFakeHeader = fakeHeader.LayoutParameters;
				lpFakeHeader.Height = heightHeader;
				fakeHeader.LayoutParameters = lpFakeHeader;
			}

			ViewGroup.LayoutParams lpHeader = header.LayoutParameters;
			lpHeader.Height = heightHeader;
			header.LayoutParameters = lpHeader;

			maxHeaderTransaction = minHeightHeader - heightHeader;

			// update heights
			headerAnimator.SetupAnimator(header, minHeightHeader, heightHeader, maxHeaderTransaction);
		}

		private void MeasureHeaderHeight()
		{
			int height = header.Height;
			if (height == 0)
			{
				// attach, wait for the height, detach
				EventHandler handler = null;
				handler = (sender, e) =>
				{
					int h = header.Height;
					if (h > 0)
					{
						header.ViewTreeObserver.GlobalLayout -= handler;
						SetHeightHeader(h);
					}
				};
				header.ViewTreeObserver.GlobalLayout += handler;
			}
			else
			{
				SetHeightHeader(height);
			}
		}
	}
}