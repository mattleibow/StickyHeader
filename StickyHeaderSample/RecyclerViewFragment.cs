using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using StickyHeader;

namespace StickyHeaderSample
{
	public class RecyclerViewFragment : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			// header
			var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerview);
			recyclerView.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false));
			StickyHeaderBuilder
				.StickTo(recyclerView)
				.SetHeader(Resource.Id.header, (ViewGroup) View)
				.SetMinHeightDimension(Resource.Dimension.min_height_header)
				.Apply();

			// items
			var elements = new string[500];
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i] = "row " + i;
			}
			recyclerView.SetAdapter(new SimpleRecyclerAdapter(Activity, elements));
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

		private class SimpleRecyclerAdapter : RecyclerView.Adapter
		{
			private readonly Context context;
			private readonly string[] elements;

			public SimpleRecyclerAdapter(Context context, string[] elements)
			{
				this.context = context;
				this.elements = elements;
			}

			public override int ItemCount
			{
				get { return elements.Length; }
			}

			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup viewGroup, int i)
			{
				LayoutInflater layoutInflater = LayoutInflater.From(context);
				View view = layoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, viewGroup, false);
				return new SimpleViewHolder(view);
			}

			public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int i)
			{
				var holder = (SimpleViewHolder) viewHolder;
				holder.SetText(elements[i]);
			}

			private class SimpleViewHolder : RecyclerView.ViewHolder
			{
				private readonly TextView textView;

				public SimpleViewHolder(View itemView)
					: base(itemView)
				{
					textView = (TextView) itemView.FindViewById(Android.Resource.Id.Text1);
				}

				public void SetText(string text)
				{
					textView.Text = text;
				}
			}
		}
	}
}