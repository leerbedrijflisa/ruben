﻿using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Media;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class PictotheekPage : ContentPage
	{
		bool removing;

		public PictotheekPage ()
		{
			InitializeComponent ();
			//NavigationPage.SetHasNavigationBar(this, false);
		}

		//Runs when a user clicks one of the pictos
		void OnPictoChoose(object sender, EventArgs args)
		{
			if (!removing) 
			{
				//create the new image and entry and the stacklayout to hold them
				Image chosenImage = (Image)sender;
				Entry chosenLabel = new Entry ();
				StackLayout currentStack = new StackLayout ();
				
				//find the stacklayout that holds the chosen image and store in currentStack, we need this to find the correct label/entry
				foreach (StackLayout stack in pictoTheek.Children) 
				{
					foreach (var child in stack.Children.OfType<Image>()) 
					{
						if (child == chosenImage) 
						{
							currentStack = stack;
						}
					}
				}
				
				//find the entry that belongs to the image in the currentStack and store it in chosenLabel
				foreach (var child in currentStack.Children.OfType<Entry>()) 
				{
					chosenLabel = child;
				}
				
				//find the root page at the top of the navigationStack
				MyPage stepPage = (MyPage)Navigation.NavigationStack [0];
				//set the image and label with the method on the steppage using the chosenImage and chosenLabel
				stepPage.SetImageAndLabel (chosenImage, chosenLabel);
				//close the pictotheek
				Navigation.PopAsync ();
			} 
			else
			{
				Image selectedImage = (Image)sender;
				Label stepLabel = new Label ();
				StackLayout currentStack = new StackLayout();

				//find the current stacklayout in the step scrollview that contains the sender image
				foreach (StackLayout stacklayout in pictoTheek.Children)
				{
					foreach (var item in stacklayout.Children) 
					{
						if (item is Image) 
						{
							if (item == selectedImage) 
							{
								currentStack = (StackLayout)stacklayout;
							}
						}
					}
				}

				pictoTheek.Children.Remove (currentStack);
				removing = false;
				removePictoButton.Text="Remove Picto";
				removePictoButton.BackgroundColor = Color.Default;
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

			//create new stacklayout to hold the image and label
			StackLayout stack = new StackLayout();

			//Create the new image
			Image newPicto = new Image();
			newPicto.HeightRequest = 256;
			newPicto.WidthRequest = 300;
			newPicto.VerticalOptions = LayoutOptions.Center;
            
			//Create the new label
			Entry pictoLabel = new Entry ();
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

			//Check if the device can take pictures
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			//Wait for the user to take his picture
			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions {
				Directory = "Ruben",
				Name = "test.jpg",
				SaveToAlbum = true
			});

			if (file == null)
				return;

			//await DisplayAlert("File Location", file.Path, "OK");

			//create the new image and entry and the stacklayout to hold them
			StackLayout stack = new StackLayout();
			Image takenPhoto = new Image();
			Entry pictoLabel = new Entry ();

			//settings for the label
			pictoLabel.Text = "taken photo test";
			pictoLabel.BackgroundColor = Color.Black;
			pictoLabel.TextColor = Color.White;

			//settings for the image
			takenPhoto.HeightRequest = 256;
			takenPhoto.WidthRequest = 300;
			takenPhoto.VerticalOptions = LayoutOptions.Center;

			//Set the image soure to the file the user just picked
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

			//add the image and label to the stacklayout
			stack.Children.Add (takenPhoto);
			stack.Children.Add (pictoLabel);

			//Add the stacklayout to the pictotheek scrollview
			pictoTheek.Children.Add (stack);
		}

		void OnRemovePictoClick(object sender, EventArgs args)
		{
			removing = !removing;

			if (removing) 
			{
				removePictoButton.Text = "Removing True";
				removePictoButton.BackgroundColor = Color.Red;
			}
			else
			{
				removePictoButton.Text = "Removing False";
				removePictoButton.BackgroundColor = Color.Default;
			}
		}

		void OnEntryFocus(object sender, EventArgs args)
		{
			buttonBar.IsVisible = false;
		}

		void OnEntryUnfocused(object sender, EventArgs args)
		{
			buttonBar.IsVisible = true;
		}
	}
}