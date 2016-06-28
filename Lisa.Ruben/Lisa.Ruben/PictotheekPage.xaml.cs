using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public partial class PictotheekPage : ContentPage
	{
		bool removing;

		public PictotheekDB database;

        public static double stepScrollViewWidth;
        public static Entry placeholdLabel;
		public static string labelText;
        public static ScrollView tScrollView;

		public PictotheekPage ()
		{
			InitializeComponent ();
			database = new PictotheekDB ();
			GetImagesFromDB ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		//Runs when a user clicks one of the pictos
		async void OnPictoChoose(object sender, EventArgs args)
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
				//close the pictotheek
				await Navigation.PopAsync ();
                //set the image and label with the method on the steppage using the chosenImage and chosenLabel

                if (chosenImage.Source is StreamImageSource)
                {
                    var stream = ((StreamImageSource)chosenImage.Source).Stream(System.Threading.CancellationToken.None).Result;
                    stepPage.SetImageAndLabel (chosenImage, chosenLabel, stream);
                }
                else
                {
                    stepPage.SetImageAndLabel(chosenImage, chosenLabel);
                }
			} 
			else
			{
				Image selectedImage = (Image)sender;
				Entry stepEntry = new Entry ();
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
				//remove from scrollview
				pictoTheek.Children.Remove (currentStack);
				removing = false;
				removePictoButton.BackgroundColor = Color.Silver;

				//remove from database
				foreach (var label in currentStack.Children.OfType<Entry>()) 
				{
					stepEntry = label;
				}
				//get the id of the picto by searching the name and delete it
				int id = database.GetIdFromName(stepEntry.Text);
				database.DeletePicto (id);
			}
		}

		//Runs when the user taps the add new picto button
		async void AddNewPicto(object sender, EventArgs args)
		{
			await CrossMedia.Current.Initialize();

			//Wait for the user to pick out his file
			var file = await CrossMedia.Current.PickPhotoAsync ();

			if (file == null)
				return;

			//create new stacklayout to hold the image and label
			StackLayout stack = new StackLayout();

            //Set the image soure to the file the user just picked
            var stream = file.GetStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);

            var pickedImage = ImageSource.FromStream(() => {
                return new MemoryStream(buffer);
            });

            //ask for the picto name and add to the database
            if (Device.OS == TargetPlatform.WinPhone)
            {
                string localPath = DependencyService.Get<ISaveToLocalStorage>().GetPath();
                var page = new LabelModalPage(localPath, stream);

                await Navigation.PushModalAsync(page);
            }
            else
            {
                var page = new LabelModalPage(file.Path);
                await Navigation.PushModalAsync(page);
            }

            //Create the new image
            Image newPicto = new Image();
            if (Device.OS == TargetPlatform.Android)
            {
                newPicto.HeightRequest = 150;
            }
            else
            {
                newPicto.HeightRequest = 226;
            }
			newPicto.WidthRequest = 260;
			newPicto.VerticalOptions = LayoutOptions.Center;
            newPicto.Source = pickedImage;

			//Create the new label
			Entry pictoLabel = new Entry();
			pictoLabel.HorizontalTextAlignment = TextAlignment.Center;
            if (Device.OS == TargetPlatform.Android)
            {
                pictoLabel.TextColor = Color.Black;
            }
            pictoLabel.TextChanged += OnEntryChanged;
            pictoLabel.Completed += OnEntryComplete;

            placeholdLabel = pictoLabel;

			//Add a tapgesturerecognizer to the image
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
            newPicto.GestureRecognizers.Add(tapGestureRecognizer);

			//add the image and label to the stacklayout
			stack.Children.Add (newPicto);
			stack.Children.Add (pictoLabel);

			//Add the stacklayout to the pictotheek scrollview
			pictoTheek.Children.Add (stack);

            tScrollView = pictoScroll;
            stepScrollViewWidth = pictoTheek.Width + stack.Width;
        }

        public async static void ScrollToEndOfStepPage(double value, ScrollView view)
        {
            await view.ScrollToAsync(value, 0, false);
        }

        public static void SetLabelText()
		{
			placeholdLabel.Text = labelText;
		}
			
		// Add new picto taken with the camera to the pictotheek
		async void CreateNewPicto(object sender, EventArgs args)
		{
			await CrossMedia.Current.Initialize();

			//Wait for the user to take his picture
			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions {
				Directory = "Ruben",
				Name = "test.jpg",
				SaveToAlbum = true
			});

			if (file == null)
				return;

            //create the new image and entry and the stacklayout to hold them
            StackLayout stack = new StackLayout();
            Image takenPhoto = new Image();
            Entry pictoLabel = new Entry();

            //turn the file into a stream
            var stream = file.GetStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);

            //store the stream in memory
            takenPhoto.Source = ImageSource.FromStream(() => {
                return new MemoryStream(buffer);
            });

            if (Device.OS == TargetPlatform.WinPhone)
            {
                string localPath = DependencyService.Get<ISaveToLocalStorage>().GetPath();
                var page = new LabelModalPage(localPath, stream);

                await Navigation.PushModalAsync(page);
            }
            else
            {
                var page = new LabelModalPage (file.Path);
			    await Navigation.PushModalAsync (page);
            }

			//settings for the label
			pictoLabel.HorizontalTextAlignment = TextAlignment.Center;
			placeholdLabel = pictoLabel;
            if (Device.OS == TargetPlatform.Android)
            {
                pictoLabel.TextColor = Color.Black;
            }

            pictoLabel.TextChanged += OnEntryChanged;
            pictoLabel.Completed += OnEntryComplete;

            //settings for the image
            if (Device.OS == TargetPlatform.Android)
            {
                takenPhoto.HeightRequest = 150;
            }
            else
            {
                takenPhoto.HeightRequest = 226;
            }
			takenPhoto.WidthRequest = 260;
			takenPhoto.VerticalOptions = LayoutOptions.Center;
 
            //Add a tapgesturerecognizer to the image
            var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnPictoChoose;
			takenPhoto.GestureRecognizers.Add(tapGestureRecognizer);

			//add the image and label to the stacklayout
			stack.Children.Add (takenPhoto);
			stack.Children.Add (pictoLabel);

			//Add the stacklayout to the pictotheek scrollview
			pictoTheek.Children.Add (stack);

            tScrollView = pictoScroll;
            stepScrollViewWidth = pictoTheek.Width + stack.Width;
        }

        //removes the picto from the pictotheek TODO also remove from local storage
		void OnRemovePictoClick(object sender, EventArgs args)
		{
			removing = !removing;

			if (removing) 
			{
				removePictoButton.BackgroundColor = Color.Red;
			}
			else
			{
				removePictoButton.BackgroundColor = Color.Silver;
			}
		}

        //gets all the stored images from the database when it first opens
		void GetImagesFromDB()
		{
			List<Picto> allImages = (List<Picto>)database.GetAllPictos (); 

			foreach (var item in allImages) 
			{
				//create new stacklayout to hold the image and label
				StackLayout stack = new StackLayout();

                //Create the new image
                Image newPicto = new Image();

                if (Device.OS == TargetPlatform.Android)
                {
                    newPicto.HeightRequest = 150;
                }
                else
                {
                    newPicto.HeightRequest = 226;
                }

                newPicto.WidthRequest = 260;
				newPicto.VerticalOptions = LayoutOptions.Center;

                //Create the new label
                Entry pictoLabel = new Entry ();
				pictoLabel.Text = item.Label;
				pictoLabel.HorizontalTextAlignment = TextAlignment.Center;
                if (Device.OS == TargetPlatform.Android)
                {
                    pictoLabel.TextColor = Color.Black;
                }
                pictoLabel.TextChanged += OnEntryChanged;
                pictoLabel.Completed += OnEntryComplete;

                //Get the image source
                if (Device.OS == TargetPlatform.WinPhone)
                {
                    newPicto.Source = item.Path + "\\" + item.FileName + ".jpg";
                }
                else
                {
                    newPicto.Source = item.Path;
                }

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
		}

        //runs when the user changes the text on the picto label
        void OnEntryChanged(object sender, TextChangedEventArgs args)
        {
            if (args.OldTextValue != null)
            {
                int id = database.GetIdFromName(args.OldTextValue);
                database.ChangeName(id, args.NewTextValue);
            }
        }

        void OnEntryComplete(object sender, EventArgs args)
        {
            Entry e = (Entry)sender;
            e.Unfocus();
        }
    }
}