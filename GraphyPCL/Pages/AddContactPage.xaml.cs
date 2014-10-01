using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AddContactPage : ContentPage
    {
        private List<string> c_phoneTypes = new List<string>() { "mobile", "home", "work", "main", "other" };
        private List<string> c_emailTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_urlTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_imTypes = new List<string>() { "skype", "hangouts", "facebook", "msn", "yahoo", "aim", "qq", "other" };

        private ContactDetailsViewModel _viewModel;

        public AddContactPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClick));

            _viewModel = new ContactDetailsViewModel();
            BindingContext = _viewModel;

//            _image.ImageSource = _viewModel.Contact.Photo;

            _phoneSection.Add(new AddMoreElementCell(_tableView, _phoneSection, c_phoneTypes, "Enter number", Keyboard.Telephone));
            _emailSection.Add(new AddMoreElementCell(_tableView, _emailSection, c_emailTypes, "Enter email", Keyboard.Email));
            _urlSection.Add(new AddMoreElementCell(_tableView, _urlSection, c_urlTypes, "Enter url", Keyboard.Url));
            _imSection.Add(new AddMoreElementCell(_tableView, _imSection, c_imTypes, "Enter nickname", Keyboard.Text));
        }


        private void OnDoneButtonClick()
        {
        }

        private async void OnImageTapped(object sender, EventArgs args)
        {
            if (sender.GetType() != typeof(ImageCell))
            {
                throw new Exception("Sender object is of type " + sender.GetType().Name + ". It shoulde be ImageCell instead.");
            }

            var mediaPicker = DependencyService.Get<IMediaPicker>();
            MediaFile mediaFile = null;
            try
            {
                mediaFile = await mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                    {
                        MaxPixelDimension = 1024
                    });
            }
            catch (TaskCanceledException)
            {
                // If TaskCanceledException is thrown: user cancel then do nothing!!
            }

            var imageSource = ImageSource.FromStream(() => mediaFile.Source);
//            await DependencyService.Get<IPhotoService>().SaveImageToDisk(imageSource, "foo");

            _viewModel.Contact.ImageName = "foo";
//            _image.ImageSource = imageSource;
        }
    }
}