using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class LabelModalPage : ContentPage
	{
		PictotheekDB database;
		string pictoPath;

		public LabelModalPage (string path)
		{
			InitializeComponent ();
			database = new PictotheekDB ();
			pictoPath = path;

		}

		protected override void OnAppearing ()
		{
			modalEntry.Focus ();
		}

		void OnEntryComplete(object sender, EventArgs args)
		{
			Entry e = (Entry)sender;
			string newText = e.Text;
			Picto p = new Picto ();
			p.Path = pictoPath;
			p.Label = newText;
			database.AddPicto (p);
			PictotheekPage.labelText = newText;
			PictotheekPage.SetLabelText ();
			Navigation.PopModalAsync();
		}
	}
}