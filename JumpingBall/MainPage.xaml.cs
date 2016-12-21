using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JumpingBall
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var accelerometer = Accelerometer.GetDefault();
            if (accelerometer == null)
            {
                return;
            }
            accelerometer.ReadingChanged += async (s1, e1) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var magnitude = Math.Sqrt(
                        e1.Reading.AccelerationX * e1.Reading.AccelerationX +
                        e1.Reading.AccelerationY * e1.Reading.AccelerationY +
                        e1.Reading.AccelerationZ * e1.Reading.AccelerationZ);
                    if (magnitude > 1.5)
                        JumpBall(magnitude);

                });

            };
        }

        private void JumpBall(double acceleration)
        {
            var da = new DoubleAnimation()
            {
                From = 0,
                To = -acceleration * 200,
                AutoReverse = true,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            Storyboard.SetTarget(da, Translation);
            Storyboard.SetTargetProperty(da, "Y");
            var sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }
    }
}
