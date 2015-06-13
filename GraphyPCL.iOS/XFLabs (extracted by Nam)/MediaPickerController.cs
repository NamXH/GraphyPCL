using System;
using System.Threading.Tasks;
using UIKit;

namespace GraphyPCL.iOS
{
    public sealed class MediaPickerController
        : UIImagePickerController
    {
        internal MediaPickerController(MediaPickerDelegate mpDelegate)
        {
            base.Delegate = mpDelegate;
        }

        public override Foundation.NSObject Delegate
        {
            get { return base.Delegate; }
            set { throw new NotSupportedException(); }
        }

        public Task<MediaFile> GetResultAsync()
        {
            return ((MediaPickerDelegate)Delegate).Task;
        }
    }
}

