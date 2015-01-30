using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using StickyHeader;

namespace StickyHeaderSample
{
	public class ListViewFragment : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.ListViewLayout, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			// header
			var listView = View.FindViewById<ListView>(Resource.Id.listview);
			StickyHeaderBuilder
				.StickTo(listView)
				.SetHeader(Resource.Id.header, (ViewGroup) View)
				.SetMinHeight(250)
				.Apply();

			// items
			var elements = new string[500];
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i] = "row " + i;
			}

			listView.Adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, elements);
		}

		public override void OnStart()
		{
			base.OnStart();
			Activity.ActionBar.Hide();
		}

		public override void OnStop()
		{
			base.OnStop();
			Activity.ActionBar.Show();
		}
	}
}