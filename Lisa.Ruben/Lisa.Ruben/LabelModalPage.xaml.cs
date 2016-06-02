using System;
using System.IO;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class LabelModalPage : ContentPage
	{
		PictotheekDB database;
		string pictoPath;
        Stream stream2;

        //contructor
		public LabelModalPage (string path, Stream stream = null)
		{
			InitializeComponent ();
			database = new PictotheekDB ();
			pictoPath = path;
            stream2 = stream;
		}

        //give the entry focus immediately
		protected override void OnAppearing ()
		{
			modalEntry.Focus ();
		}

        //when the user click the "enter" button on the keyboard, save the label to the database
		async void OnEntryComplete(object sender, EventArgs args)
		{
			Entry e = (Entry)sender;
            //check if the label name alreadyexists in the database
			if (!database.CheckLabelNameExists(e.Text))
			{
                //create the picto
				string newText = e.Text;
				Picto p = new Picto ();
				p.Path = pictoPath;
				p.Label = newText;
                p.FileName = newText;

                //when we have a stream, we are on windows phone, call depencyservice on savetolocalstorage
                if (stream2 != null)
                {
                    await DependencyService.Get<ISaveToLocalStorage>().SaveToLocalFolderAsync(stream2, p.Label);
                }

                //add it to thedatabase, update the labelText, and close the page
                database.AddPicto (p);
				PictotheekPage.labelText = newText;
				PictotheekPage.SetLabelText ();
				await Navigation.PopModalAsync();
			} 
            //name already exists
			else
			{
				await DisplayAlert ("Error","Er is al een Picto met deze naam","Ok");	
				e.Focus ();
			}
		}
	}
}