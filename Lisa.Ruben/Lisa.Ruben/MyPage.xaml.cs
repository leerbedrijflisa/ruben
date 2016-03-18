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
		Label selectedLabel = new Label();

		public MyPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}
       
		//Creates a new Button and adds it to the scrollview
		void AddNewStep(object sender, EventArgs args)
		{
			StackLayout newStack = new StackLayout ();
			Image stepImage = new Image();
			Label stepLabel = new Label ();

			TapGestureRecognizer tap = new TapGestureRecognizer ();
			tap.Tapped += OnStepSelect;

			stepImage.GestureRecognizers.Add(tap) ;

			stepImage.HeightRequest = 256;
			stepImage.WidthRequest = 300;
			stepImage.VerticalOptions = LayoutOptions.Center;

			selectedImage = stepImage;
			selectedLabel = stepLabel;

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

			selectedImage.Opacity = 0.5;
			selectedImage.BackgroundColor = Color.White;

			stepLabel.Text = "";
			stepLabel.BackgroundColor = Color.Black;
			stepLabel.TextColor = Color.White;

			newStack.Children.Add (stepImage);
			newStack.Children.Add (stepLabel);

			//Add to the scrollview
			scrollSteps.Children.Add (newStack);
		}

		void OnStepSelect(object sender, EventArgs args)
		{
			//Set the selectedImage to the current tapped button and create a label and layout for it
			Image stepButton = (Image)sender;
			Label stepLabel = new Label ();
			StackLayout currentStack = new StackLayout();

			//find the current stacklayout in the step scrollview that contains the sender image
			foreach (StackLayout stacklayout in scrollSteps.Children)
			{
				foreach (var item in stacklayout.Children) 
				{
					if (item is Image) 
					{
						if (item == (Image)sender) 
						{
							currentStack = (StackLayout)stacklayout;
						}
					}
				}
			}

			//If removing is disabled
			if (!removing) 
			{
				//find the label that belongs to the image
				foreach (var item in currentStack.Children) 
				{
					if (item is Label) 
					{
						stepLabel = (Label)item;
					}
				}

				//set opacity of all images back to 1
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
				//when you tap the selectedbutton when it is already selected, unselect it
				if ((Image)sender == selectedImage)
				{
					stepButton.Opacity = 1;
					selectedImage = new Image ();
					selectedLabel = new Label ();
				} 
				else 
				{
					stepButton.Opacity = 0.5;
					selectedImage = stepButton;
					selectedLabel = stepLabel;

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
				scrollSteps.Children.Remove(currentStack);
			}
		}

		//Runs when a user clicks one of the pictos
		void OnPictoChoose(object sender, EventArgs args)
		{
			//store the sender image
			StackLayout currentstack = new StackLayout();
            Image pictoImage = (Image)sender;
			Label pictoLabel = new Label ();

			foreach (StackLayout stacklayout in pictoTheek.Children)
			{
				foreach (var item in stacklayout.Children)
				{
					if (item is Image && item == (Image)sender) 
					{
						currentstack = stacklayout;
					}
				}
			}

			foreach (var item in currentstack.Children)
			{
				if (item is Label) 
				{
					pictoLabel = (Label)item;
				}
			}

			//set the selected image to the stored image
			selectedImage.Source = pictoImage.Source;
			selectedImage.BackgroundColor = Color.Transparent;

			selectedLabel.Text = pictoLabel.Text;
			 
			//set opcities on all images back to 1
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
			selectedImage = new Image ();
			selectedLabel = new Label ();
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
					//file.Dispose();
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