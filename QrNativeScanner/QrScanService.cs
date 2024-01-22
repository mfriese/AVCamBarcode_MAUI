using AVCamBarcode;
using CommunityToolkit.Maui.Views;
using CoreGraphics;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;

namespace WorkApp.Services
{
    public class QrScannerService // : IQrScannerService
    {
        /// <summary>
        /// Keep it simple, the region of interest will be remembered as long
        /// as the application runs. If there are problems one can restart the
        /// app and problems are solved.
        /// </summary>
        private static CGRect CamSetup { get; set; } = CGRect.Null;

        public static void StartScanning(Action<string> continuation, ContentPage cp)
        {
            // Load the storyboard ...
            var cameraUi = UIStoryboard.FromName("Camera", null);

            // ... and load the associated controller
            var controller = cameraUi.InstantiateInitialViewController();

            var cameraViewController = controller as CameraViewController;

            TryRestoreSettings(cameraViewController);

            // Forms has an extension that lets us convert to a forms view
            var wrappedFormsView = cameraViewController?.View.ToView();

            // Create a page so we can add it to the navigation stack
            var contentPage = new Microsoft.Maui.Controls.ContentPage()
            {
                Content = wrappedFormsView,
                Background = Brush.Green
            };

            if (cameraViewController is not null)
                // listen to barcode events
                cameraViewController.BarcodeSelected += (s, e)
                    => MainThread.BeginInvokeOnMainThread(() =>
                    {
                        TryPersistSettings(cameraViewController);

                        // run this on the main thread ...
                        _ = Shell.Current.Navigation.PopAsync();

                        // Launch callback with result
                        continuation?.Invoke(e);
                    });

            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    // run this on the main thread ...
                    Shell.Current.Navigation.PushAsync(contentPage);

                    //cp.Content = wrappedFormsView;
                }
                catch(Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    Console.WriteLine(exp.StackTrace);
                }
            });
        }

        /// <summary>
        /// Remember last setting of region of interest.
        /// </summary>
        private static void TryPersistSettings(CameraViewController? controller)
        {
            CamSetup = controller?.GetCurrentRegionOfInterest() ?? CGRect.Null;
        }

        /// <summary>
        /// Restore last setting of region of interest.
        /// </summary>
        private static void TryRestoreSettings(CameraViewController? controller)
        {
            if (CamSetup == CGRect.Null)
                return;

            controller?.OverrideInitialRegionOfInterest(CamSetup);
        }
    }
}

