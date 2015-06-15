using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Device;

namespace GraphyPCL.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : XLabs.Forms.XFormsApplicationDelegate
    {
//        UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            // Setup resolver for XLabs.Forms
            if (!Resolver.IsSet)
            {
                SetIoc();
            }

            LoadApplication(new App());

            // Pre Xamarin.Forms 1.3
//            _window = new UIWindow(UIScreen.MainScreen.Bounds);
//            _window.RootViewController = App.GetMainPage().CreateViewController();
//            _window.MakeKeyAndVisible();

            return base.FinishedLaunching (app, options);
        }

        // Setup resolver for XLabs.Forms
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IMediaPicker, MediaPicker>();

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}

