using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;

namespace StickyHeaderSample
{
	public class MainFragment : ListFragment
	{
		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);

			// fragments
			var fragments = new Dictionary<string, Fragment>
			{
				{"Simple Sticky Header",new ListViewFragment()},
				{"Parallax Simple Sticky Header",new ParallaxFragment()},
				{"ActionBarImage Header",new ActionBarImageFragment()},
				{"Custom Animation Header",new CustomHeaderFragment()},
				{"Recycler View Header",new RecyclerViewFragment()},
				{"Scroll View Header",new ScrollViewFragment()}
			};
			var items = fragments.Keys.ToArray();

			// items
			ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, items);

			// item selection
			ListView.ItemClick += (sender, e) =>
			{
				var fragment = fragments[items[e.Position]];
				((MainActivity)Activity).LoadFragment(fragment);
			};
		}
	}
}