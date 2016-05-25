using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Lisa.Ruben
{
    public partial class MyPage : ContentPage
    {
		bool removing;
		public PictotheekDB database;
		Image selectedImage = new Image();
		PictotheekPage pictotheek = new PictotheekPage();

		public MyPage ()
		{
			InitializeComponent ();
			database = new PictotheekDB ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		//Creates a new Button and adds it to the scrollview
		void AddNewStep(object sender, EventArgs args)
		{
			//create the new image and label and the stacklayout to hold them
			StackLayout newStack = new StackLayout ();
			Image stepImage = new Image();
			Label stepLabel = new Label ();

			//Add a tapgesturerecognizer to the image
			TapGestureRecognizer tap = new TapGestureRecognizer ();
			tap.Tapped += OnStepSelect;
			stepImage.GestureRecognizers.Add(tap) ;

			//settings for the image
			stepImage.BackgroundColor = Color.Silver;
			stepImage.WidthRequest = 260;
            if (Device.OS == TargetPlatform.Android)
            {
                stepImage.HeightRequest = 150;
            }
            else
            {
                stepImage.HeightRequest = 226;
            }

            //settings for the label
            stepLabel.Text = "";
			stepLabel.BackgroundColor = Color.Black;
			stepLabel.TextColor = Color.White;
			stepLabel.HorizontalTextAlignment = TextAlignment.Center;

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
				removing = false;
				removeButton.BackgroundColor = Color.Default;
				removeButton.Text = "Remove Step"; 
			}
			//If removing is disabled open the pictotheek to choose a picture
			else
			{
				await Navigation.PushAsync(pictotheek,false);
			}
		}

		//sets the image and label chosen in the pictotheek
		public void SetImageAndLabel(Image pictotheekImg, Entry pictptheekLabel, Stream streampy = null)
		{
			//create stacklayout as placeholder to find the Label
			StackLayout currentStack = new StackLayout();

			//find the currentstack by looking for the selectedimage in the scrollsteps
			foreach (StackLayout child in scrollSteps.Children) 
			{
				//set the selected image to the image chosen in the pictotheek
				foreach (Image item in child.Children.OfType<Image>()) 
				{
					if (item == selectedImage) 
					{
						currentStack = child;
						item.BackgroundColor = Color.Transparent;

                        //for windows phone only
                        if (streampy != null)
                        {
                            var stream = ((StreamImageSource)pictotheekImg.Source).Stream(System.Threading.CancellationToken.None).Result;

                            item.Source = ImageSource.FromStream(() => streampy);
                        }
                        //ios and android
                        else
                        {
                            item.Source = pictotheekImg.Source;
                        }
					}
				}

				//find the label that belongs to the selectedimage in the currentStack and set the text
				foreach (Label label in currentStack.Children.OfType<Label>()) 
				{
					label.Text = pictptheekLabel.Text;
				}
			}
		}
			
		//Runs when the user taps the remove button
		void OnRemoveButtonClick(object sender, EventArgs args)
		{
			//Negate removing and store the removeButton
			removing = !removing;
			Button removeButton = (Button)sender;

			//If removing is enabled, change the text and backgroundcolor according to the value
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