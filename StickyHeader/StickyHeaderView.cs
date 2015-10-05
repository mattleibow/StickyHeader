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
		protected readonly View view;
		protected readonly HeaderAnimator headerAnimator;
		protected readonly int minHeightHeader;

		protected int heightHeader;
		protected int maxHeaderTransaction;

		protected StickyHeaderView(Context context, View header, View view, int minHeightHeader, HeaderAnimator headerAnimator)
		{
			this.context = context;
			this.header = header;
			this.view = view;
			this.minHeightHeader = minHeightHeader;
			this.headerAnimator = headerAnimator;

			MeasureHeaderHeight();

			headerAnimator.SetupAnimator(header, minHeightHeader, heightHeader, maxHeaderTransaction);
		}

		protected virtual void SetHeightHeader(int value)
		{
			heightHeader = value;

			ViewGroup.LayoutParams lpHeader = header.LayoutParameters;
			lpHeader.Height = heightHeader;
			header.LayoutParameters = lpHeader;

			maxHeaderTransaction = minHeightHeader - heightHeader;

			// update heights
			headerAnimator.SetupAnimator(header, minHeightHeader, heightHeader, maxHeaderTransaction);
		}

		private void MeasureHeaderHeight()
		{
			// try use the existing height
			if (header.Height != 0)
			{
				SetHeightHeader(header.Height);
			}
			else
			{
				// try get the height from the layout
				var lp = header.LayoutParameters;
				if (lp != null && lp.Height > 0)
				{
					SetHeightHeader(lp.Height);
				}
				else
				{
					header.ViewTreeObserver.AddOnGlobalLayoutAction(() => 
					{
						if (heightHeader != header.Height)
						{
							SetHeightHeader(header.Height);
						}
					});
				}
			}
		}
	}
}