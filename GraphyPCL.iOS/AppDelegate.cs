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
        UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            if (!Resolver.IsSet)
            {
                SetIoc();
            }

            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            _window.RootViewController = App.GetMainPage().CreateViewController();
            _window.MakeKeyAndVisible();

            return true;
        }

        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IMediaPicker, MediaPicker>();

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}

