using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using StickyHeader.Animator;

namespace StickyHeader
{
	public class StickyHeaderRecyclerView : StickyHeaderView
	{
		private readonly RecyclerView recyclerView;

		public StickyHeaderRecyclerView(Context context, View header, int minHeightHeader, HeaderAnimator headerAnimator, RecyclerView recyclerView)
			: base(context, header, minHeightHeader, headerAnimator)
		{
			this.recyclerView = recyclerView;

			// scroll events
			recyclerView.SetOnScrollListener(new RecyclerScrollListener(this));
		}

		protected override void SetHeightHeader(int value)
		{
			base.SetHeightHeader(value);

			SetupItemDecorator();
		}

		private void SetupItemDecorator()
		{
			var layoutManager = recyclerView.GetLayoutManager();
			if (layoutManager is GridLayoutManager)
			{
				var manager = layoutManager as GridLayoutManager;
				switch (manager.Orientation)
				{
					case LinearLayoutManager.Vertical:
						recyclerView.AddItemDecoration(new GridItemDecoration(this));
						break;
					case LinearLayoutManager.Horizontal:
						//TODO
						break;
				}
			}
			else if (layoutManager is LinearLayoutManager)
			{
				var manager = layoutManager as LinearLayoutManager;
				switch (manager.Orientation)
				{
					case LinearLayoutManager.Vertical:
						recyclerView.AddItemDecoration(new LinearItemDecoration(this));
						break;
					case LinearLayoutManager.Horizontal:
						//TODO
						break;
				}
			}
		}

		private class GridItemDecoration : RecyclerView.ItemDecoration
		{
			private readonly StickyHeaderRecyclerView headerView;

			public GridItemDecoration(StickyHeaderRecyclerView headerView)
			{
				this.headerView = headerView;
			}

			public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
			{
				base.GetItemOffsets(outRect, view, parent, state);

				var layoutManager = (GridLayoutManager)parent.GetLayoutManager();
				int position = parent.GetChildPosition(view);
				if (position < layoutManager.SpanCount)
				{
					outRect.Top = headerView.heightHeader;
				}
			}
		}

		private class LinearItemDecoration : RecyclerView.ItemDecoration
		{
			private readonly StickyHeaderRecyclerView headerView;

			public LinearItemDecoration(StickyHeaderRecyclerView headerView)
			{
				this.headerView = headerView;
			}

			public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
			{
				base.GetItemOffsets(outRect, view, parent, state);

				int position = parent.GetChildPosition(view);
				if (position == 0)
				{
					outRect.Top = headerView.heightHeader;
				}
			}
		}

		private class RecyclerScrollListener : RecyclerView.OnScrollListener
		{
			private readonly StickyHeaderRecyclerView headerView;
			private int scrolledY;

			public RecyclerScrollListener(StickyHeaderRecyclerView headerView)
			{
				this.headerView = headerView;
				this.scrolledY = 0;
				this.scrolledY = 0;
			}

			public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
			{
				base.OnScrolled(recyclerView, dx, dy);

				scrolledY += dy;
				headerView.headerAnimator.OnScroll(-scrolledY);
			}
		}
	}
}