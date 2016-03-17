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
			StackLayout stack = new StackLayout ();
			Image stepImage = new Image();
			Label stepLabel = new Label ();

			TapGestureRecognizer tap = new TapGestureRecognizer ();
			tap.Tapped += OnStepSelect;

			stepImage.GestureRecognizers.Add(tap) ;

			stepImage.HeightRequest = 256;
			stepImage.WidthRequest = 300;
			stepImage.VerticalOptions = LayoutOptions.Center;

			selectedImage = stepImage;
//			foreach (Image img in scrollSteps.Children) 
//			{
//				img.Opacity = 1;
//				img.BackgroundColor = Color.Transparent;
//			}

			foreach (StackLayout stack2 in scrollSteps.Children)
			{
				foreach (var child in stack2.Children) 
				{
					if (child is Image)
					{
						child.Opacity = 1;
						child.BackgroundColor = Color.Transparent;
					}	
				}
			}

			selectedImage.Opacity = 0.5;
			selectedImage.BackgroundColor = Color.White;

			stepLabel.Text = "haloo";
			stack.Children.Add (stepImage);
			stack.Children.Add (stepLabel);
			//Add to the scrollview
			scrollSteps.Children.Add (stack);
		}

		void OnStepSelect(object sender, EventArgs args)
		{
			//If removing is disabled
			if (!removing) 
			{
				//Set the selectedImage to the current tapped button
				Image stepButton = (Image)sender;

				foreach (StackLayout stack in scrollSteps.Children)
				{
					foreach (var child in stack.Children) 
					{
						if (child is Image)
						{
							child.Opacity = 1;
							child.BackgroundColor = Color.Transparent;
						}	
					}
				}

				if ((Image)sender == selectedImage)
				{
					stepButton.Opacity = 1;
					selectedImage = new Image ();
				} 
				else 
				{
					stepButton.Opacity = 0.5;
					selectedImage = stepButton;

					if (stepButton.Source == null) 
					{
						stepButton.BackgroundColor = Color.White;
					}
					else 
					{
						stepButton.BackgroundColor = Color.Transparent;
					}
				}
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
            Image pictoImage = (Image)sender;		

			selectedImage.Source = pictoImage.Source;
			selectedImage.BackgroundColor = Color.Transparent;
			 
			foreach (StackLayout stack in scrollSteps.Children)
			{
				foreach (var child in stack.Children) 
				{
					if (child is Image)
					{
						child.Opacity = 1;
					}	
				}
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
			}
			else 
			{
				removeButton.Text = "Remove Step";
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
			StackLayout stack = new StackLayout();
			Image newPicto = new Image();
			Label pictoLabel = new Label ();
			pictoLabel.Text = "new picto test";
			pictoLabel.BackgroundColor = Color.Black;
			pictoLabel.TextColor = Color.White;
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

			stack.Children.Add (newPicto);
			stack.Children.Add (pictoLabel);

			//Add the image to the pictotheek scrollview
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

			//Create a new image
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