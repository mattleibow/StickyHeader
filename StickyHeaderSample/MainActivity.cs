using Android.App;
using Android.OS;
using StickyHeaderSample;

namespace StickyHeaderSample
{
	[Activity(Label = "Sticky Header", Icon = "@drawable/icon", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MainLayout);

			if (savedInstanceState == null)
			{
				FragmentManager
					.BeginTransaction()
					.Add(Resource.Id.layout_container, new MainFragment())
					.Commit();
			}
		}

		public void LoadFragment(Fragment fragment)
		{
			FragmentManager
				.BeginTransaction()
				.Replace(Resource.Id.layout_container, fragment)
				.AddToBackStack(null)
				.Commit();
		}
	}
}