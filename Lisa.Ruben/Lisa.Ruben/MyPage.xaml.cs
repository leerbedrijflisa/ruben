using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Media;

namespace Lisa.Ruben
{
    public partial class MyPage : ContentPage
    {
		bool removing;
		Button selectedButton = new Button();

		public MyPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}
       
		//Creates a new Button and adds it to the scrollview
		void AddNewStep(object sender, EventArgs args)
		{
			//Create a new button and set its values
			Button stepButton = new Button();
            stepButton.Clicked += OnStepSelect;
            stepButton.HeightRequest = 256;
            stepButton.WidthRequest = 300;
            stepButton.VerticalOptions = LayoutOptions.Center;

			//If removing is enabled, set the text
			if (removing) 
			{
                stepButton.Text = "Tap to remove";
			}

			//Add to the scrollview
            scrollSteps.Children.Add (stepButton);
		}

		//Runs when the user taps on a step-button
		void OnStepSelect(object sender, EventArgs args)
		{
			//If removing is disabled
			if (!removing) 
			{
				//Clear the text of all buttons
				foreach (Button but in scrollSteps.Children)
				{
					but.Text = "";
				}

				//Set the selectedButton to the current tapped button
                Button stepButton = (Button)sender;
                stepButton.Text = "Selected";
                selectedButton = stepButton;
			}
			//If removing is enabled, remove the button from the scrollview
			else
			{
				scrollSteps.Children.Remove ((View)sender);
			}
		}

		//Runs when a user clicks one of the pictos
		void OnPictoChoose(object sender, EventArgs args)
		{
			//Get the image of the sending button and apply it to the selectedbutton in the stepView
			if (sender is Button) 
			{
				Button stepButton = (Button)sender;
                FileImageSource stepImage = stepButton.Image;
                selectedButton.Image = stepImage;
				selectedButton.Text = "";
				selectedButton = new Button ();
			}
		}

		//Runs when the user taps the remove button
		void OnRemoveButtonClick(object sender, EventArgs args)
		{
			//Negate removing and store the removeButton
			removing = !removing;
			Button removeButton = (Button)sender;

			//If removing is enabled, change the text and backgroundcolor according to the value and update all button texts
			if (removing) 
			{
				removeButton.Text = "Removing true";
				removeButton.BackgroundColor = Color.Red;

				foreach (Button stepButton in scrollSteps.Children)
				{
					stepButton.Text = "Click to remove";
				}
			}
			else 
			{
				removeButton.Text = "Removing false";
				removeButton.BackgroundColor = Color.Default;

				foreach (Button stepButton in scrollSteps.Children)
				{
					stepButton.Text = "";
				}
			}
		}

		//Runs when the user taps the add new picto button
		async void AddNewPicto(object sender, EventArgs args)
		{
			await CrossMedia.Current.Initialize();

			//Check if the device can take pictures
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			//Wait for the user to pick out his file
			var file = await CrossMedia.Current.PickPhotoAsync ();

			if (file == null)
				return;

			//Create a new image
			Image i = new Image();
			i.HeightRequest = 256;
			i.WidthRequest = 300;
			i.VerticalOptions = LayoutOptions.Center;

			//Set the image soure to the file the user just picked
			var pickedImage = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					file.Dispose();
					return stream;
				}); 

			i.Source = pickedImage;

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			i.GestureRecognizers.Add(tapGestureRecognizer);
			//handle the tap
			tapGestureRecognizer.Tapped += (s, e) => 
			{
				//OnPictoChoose(i,e);
			};

			//Add the image to the pictotheek scrollview
			pictoTheek.Children.Add (i);
		}

		async void CreateNewPicto(object sender, EventArgs args)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions {
				Directory = "Ruben",
				Name = "test.jpg",
				SaveToAlbum = true
			});

			if (file == null)
				return;

			await DisplayAlert("File Location", file.Path, "OK");

			//Create a new image
			Image i = new Image();
			i.HeightRequest = 256;
			i.WidthRequest = 300;
			i.VerticalOptions = LayoutOptions.Center;

			i.Source = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					file.Dispose();
					return stream;
				});

			pictoTheek.Children.Add (i);
		}
    }
}