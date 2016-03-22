using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Plugin.Media;

namespace Lisa.Ruben
{
    public partial class MyPage : ContentPage
    {
		bool removing;
		Image selectedImage = new Image();

		PictotheekPage pictotheek = new PictotheekPage();

		public MyPage ()
		{
			InitializeComponent ();
			//NavigationPage.SetHasNavigationBar(this, false);
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

			stepImage.BackgroundColor = Color.Silver;
			stepImage.HeightRequest = 256;
			stepImage.WidthRequest = 300;
			//stepImage.VerticalOptions = LayoutOptions.Center;

			stepLabel.Text = "";
			stepLabel.BackgroundColor = Color.Black;
			stepLabel.TextColor = Color.White;

			newStack.Children.Add (stepImage);
			newStack.Children.Add (stepLabel);

			//Add to the scrollview
			scrollSteps.Children.Add (newStack);
		}

		async void OnStepSelect(object sender, EventArgs args)
		{
			//Set the selectedImage to the current tapped button and create a label and layout for it
			selectedImage = (Image)sender;
			Label stepLabel = new Label ();
			StackLayout currentStack = new StackLayout();

			//find the current stacklayout in the step scrollview that contains the sender image
			foreach (StackLayout stacklayout in scrollSteps.Children)
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

			//If removing is enabled, remove the button from the scrollview
			if (removing) 
			{
				scrollSteps.Children.Remove(currentStack);
			}
			//If removing is disabled open the pictotheek to choose a picture
			else
			{
				await Navigation.PushAsync(pictotheek);
			}
		}

		//set the image chosen in the pictotheek
		public void SetImage(Image img)
		{
			foreach (StackLayout child in scrollSteps.Children) 
			{
				foreach (Image item in child.Children.OfType<Image>()) 
				{
					if (item == selectedImage) 
					{
						item.BackgroundColor = Color.Transparent;
						item.Source = img.Source;	
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
    }
}