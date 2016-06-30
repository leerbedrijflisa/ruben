using System;
using System.IO;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class LabelModalPage : ContentPage
	{
        PictotheekPage pictoPage;
		PictotheekDB database;
		string pictoPath;
        Stream stream2;

        //contructor
		public LabelModalPage (string path, PictotheekPage page, Stream stream = null)
		{
			InitializeComponent ();
			database = new PictotheekDB ();
            pictoPage = page;
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
            //check if the user has entered a name
            if (e.Text == "" || e.Text == null)
            {
                await DisplayAlert("Error", "Voer a.u.b. een naam in", "Ok");
                e.Focus();
            }
            //check if the label name alreadyexists in the database
            else if (!database.CheckLabelNameExists(e.Text))
			{
                //check if there are not more than 20 chars
                if (!CheckMore20Chars(e.Text))
                {
                    //add the picto label
                    AddNewPictoLabel(e.Text, e.Text);
                }
                else {
                    await DisplayAlert("Error", "Gebruik maximaal 20 tekens.", "Ok");
                    e.Focus();
                }
            }
            //name already exists
            else
			{
                int i = 0;
                string newText = e.Text;
                //add a number to the file name and check if it exists
                while (database.CheckLabelNameExists(newText))
                {
                    newText = e.Text + i;
                    i++;
                }
                //check if there are not more than 20 chars
                if (!CheckMore20Chars(e.Text))
                {
                    AddNewPictoLabel(newText, e.Text);
                }
                else //elsedisplay a warning
                {
                    await DisplayAlert("Error", "Gebruik maximaal 20 tekens.", "Ok");
                    e.Focus();
                }
            }
        }

        //this function runs when the user has changed thelabel into a valid name
        //it adds the new name to the database, and scrolls the page to the end
        async void AddNewPictoLabel(string newText, string oldText)
        {
            //create the picto
            Picto p = new Picto();
            p.Path = pictoPath;
            p.Label = oldText;
            p.FileName = newText;

            //when we have a stream, we are on windows phone, call depencyservice on savetolocalstorage
            if (stream2 != null)
            {
                await DependencyService.Get<ISaveToLocalStorage>().SaveToLocalFolderAsync(stream2, p.FileName);
            }

            //add it to thedatabase, update the labelText, and close the page
            database.AddPicto(p);
            PictotheekPage.labelText = oldText;
            PictotheekPage.SetLabelText();
            pictoPage.RemoveAllPictos();
            pictoPage.GetImagesFromDB();
            await Navigation.PopModalAsync();
            PictotheekPage.ScrollToEndOfStepPage(PictotheekPage.stepScrollViewWidth, PictotheekPage.tScrollView);
        }

        //this function return true when 'text' has more then 20 chars, false when it doesnt
        bool CheckMore20Chars(string text) {
            if (text.Length > 20)
            {
                return true;
            }
            return false;
        }
	}
}