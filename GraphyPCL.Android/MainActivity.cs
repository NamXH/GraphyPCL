using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Services.Geolocation;

namespace GraphyPCL.Android
{
    [Activity(Label = "Graphy", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : XLabs.Forms.XFormsApplicationDroid
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            if (!Resolver.IsSet)
            {
                SetIoc();
            }

            LoadApplication(new App());

            // Pre Xamarin.Forms 1.3
//            SetPage(App.GetMainPage());
        }

        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IMediaPicker, MediaPicker>()
                .Register<IGeolocator, Geolocator>();

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}