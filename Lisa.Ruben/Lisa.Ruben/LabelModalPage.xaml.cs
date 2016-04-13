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

		async void OnEntryComplete(object sender, EventArgs args)
		{
			Entry e = (Entry)sender;
			if (!database.CheckLabelNameExists(e.Text))
			{
				string newText = e.Text;
				Picto p = new Picto ();
				p.Path = pictoPath;
				p.Label = newText;
				database.AddPicto (p);
				PictotheekPage.labelText = newText;
				PictotheekPage.SetLabelText ();
				await Navigation.PopModalAsync();
			} 
			else
			{
				await DisplayAlert ("Error","Er is al een Picto met deze naam","Ok");	
				e.Focus ();
			}
		}
	}
}