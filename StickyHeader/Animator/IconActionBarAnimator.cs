using System.Drawing;
using Android.App;
using Android.Views;

namespace StickyHeader.Animator
{
	public class IconActionBarAnimator : HeaderStickyAnimator
	{
		private readonly View homeActionBar;
		private readonly int layoutResource;

		public IconActionBarAnimator(Activity activity, int layoutResource)
		{
			this.layoutResource = layoutResource;
			this.homeActionBar = activity.FindViewById(Android.Resource.Id.Home);
		}

		public override AnimatorBuilder CreateAnimatorBuilder()
		{
			var view = Header.FindViewById(layoutResource);
			var rect = new RectangleF(
				homeActionBar.Left, homeActionBar.Top,
				homeActionBar.Right, homeActionBar.Bottom);
			var point = new PointF(homeActionBar.Left, homeActionBar.Top);
			return AnimatorBuilder
				.Create()
				.ApplyScale(view, rect)
				.ApplyTranslation(view, point);
		}
	}
}