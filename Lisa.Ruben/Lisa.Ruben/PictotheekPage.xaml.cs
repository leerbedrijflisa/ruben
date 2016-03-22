using System;
using System.Collections.Generic;
using Plugin.Media;

using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class PictotheekPage : ContentPage
	{
		public PictotheekPage ()
		{
			InitializeComponent ();
			//NavigationPage.SetHasNavigationBar(this, false);
		}

		//Runs when a user clicks one of the pictos
		void OnPictoChoose(object sender, EventArgs args)
		{
			Image chosenImage = (Image)sender;

			MyPage stepPage = (MyPage)Navigation.NavigationStack [0];
			stepPage.SetImage (chosenImage);

			Navigation.PopAsync ();
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

			//create new stacklayout to hold the image and label
			StackLayout stack = new StackLayout();

			//Create the new image
			Image newPicto = new Image();
			newPicto.HeightRequest = 256;
			newPicto.WidthRequest = 300;
			newPicto.VerticalOptions = LayoutOptions.Center;
            
			//Create the new label
			Label pictoLabel = new Label ();
			pictoLabel.Text = "new picto test";
			pictoLabel.BackgroundColor = Color.Black;
			pictoLabel.TextColor = Color.White;

			//Set the image soure to the file the user just picked
			var pickedImage = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					//file.Dispose();
					return stream;
				}); 

            newPicto.Source = pickedImage;

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
            newPicto.GestureRecognizers.Add(tapGestureRecognizer);

			//add the image and label to the stacklayout
			stack.Children.Add (newPicto);
			stack.Children.Add (pictoLabel);

			//Add the stacklayout to the pictotheek scrollview
			pictoTheek.Children.Add (stack);
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

			//Create a new stacklayout to hold the new image and label
			StackLayout stack = new StackLayout();
			Image takenPhoto = new Image();
			Label pictoLabel = new Label ();

			pictoLabel.Text = "taken photo test";
			pictoLabel.BackgroundColor = Color.Black;
			pictoLabel.TextColor = Color.White;

			takenPhoto.HeightRequest = 256;
			takenPhoto.WidthRequest = 300;
			takenPhoto.VerticalOptions = LayoutOptions.Center;

			takenPhoto.Source = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					//file.Dispose();
					return stream;
				});

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
			takenPhoto.GestureRecognizers.Add(tapGestureRecognizer);

			stack.Children.Add (takenPhoto);
			stack.Children.Add (pictoLabel);

			pictoTheek.Children.Add (stack);
		}
	}
}