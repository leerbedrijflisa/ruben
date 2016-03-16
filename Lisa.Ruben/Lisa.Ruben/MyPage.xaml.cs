using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Media;

namespace Lisa.Ruben
{
    public partial class MyPage : ContentPage
    {
		bool removing;
		Image selectedImage = new Image();

		public MyPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}
       
		//Creates a new Button and adds it to the scrollview
		void AddNewStep(object sender, EventArgs args)
		{
			Image stepButton = new Image();

			TapGestureRecognizer tap = new TapGestureRecognizer ();
			tap.Tapped += OnStepSelect;

			stepButton.GestureRecognizers.Add(tap) ;

			stepButton.HeightRequest = 256;
			stepButton.WidthRequest = 300;
			stepButton.VerticalOptions = LayoutOptions.Center;

			//If removing is enabled, set the text
			if (removing) 
			{
            //    stepButton.Text = "Tap to remove";
			}

			//Add to the scrollview
            scrollSteps.Children.Add (stepButton);
		}

		void OnStepSelect(object sender, EventArgs args)
		{
			//If removing is disabled
			if (!removing) 
			{
				//Set the selectedImage to the current tapped button
				Image stepButton = (Image)sender;
	
				selectedImage = stepButton;
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
            if (sender is Image)
            {
                Image pictoImage = (Image)sender;

				selectedImage.Source = pictoImage.Source;
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

				foreach (Image stepImg in scrollSteps.Children)
				{
					//stepImgstepImg.Text = "Click to remove";
				}
			}
			else 
			{
				removeButton.Text = "Removing false";
				removeButton.BackgroundColor = Color.Default;
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
			Image newPicto = new Image();
            newPicto.HeightRequest = 256;
            newPicto.WidthRequest = 300;
            newPicto.VerticalOptions = LayoutOptions.Center;

			//Set the image soure to the file the user just picked
			var pickedImage = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
				//	file.Dispose();
					return stream;
				}); 

            newPicto.Source = pickedImage;

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
            newPicto.GestureRecognizers.Add(tapGestureRecognizer);

			//Add the image to the pictotheek scrollview
            pictoTheek.Children.Add (newPicto);
		}


        // Add new picto taken with the camera to the pictotheek
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
					//file.Dispose();
					return stream;
				});

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
			i.GestureRecognizers.Add(tapGestureRecognizer);

			pictoTheek.Children.Add (i);
		}
    }
}