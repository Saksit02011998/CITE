using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace CITE_smart_interactive
{
	/// <summary>
	/// Interaction logic for Camera.xaml
	/// </summary>
	public partial class Camera : Window
	{
		KinectSensor vikinect;
		KinectSensorChooser kinectSensorChooser;
		MainWindow mainWindow;
		public Camera()
		{
			InitializeComponent();
		}

		private void Camera_Load(object sender, RoutedEventArgs e)
		{
			vikinect = KinectSensor.KinectSensors.FirstOrDefault();
			vikinect.Start();
			vikinect.ColorStream.Enable();
			vikinect.ColorFrameReady += Vikinect_ColorFrameReady;
		}

		private void Vikinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
		{
			using (ColorImageFrame frameImagen = e.OpenColorImageFrame())
			{
				if (frameImagen == null) return;
				byte[] datosColor = new byte[frameImagen.PixelDataLength];
				frameImagen.CopyPixelDataTo(datosColor);

				VideoKinect.Source = BitmapSource.Create(
					frameImagen.Width, frameImagen.Height,
					96,
					96,
					PixelFormats.Bgr32,
					null,
					datosColor,
					frameImagen.Width * frameImagen.BytesPerPixel
					);
			}
		}
		public void StopKinect()
		{
			if (kinectSensorChooser == null)
			{
				return;
			}
			if (kinectSensorChooser.Kinect == null)
			{
				return;
			}

			if (kinectSensorChooser.Kinect.SkeletonStream.IsEnabled)
			{
				kinectSensorChooser.Kinect.SkeletonStream.Disable();
			}

			if (kinectSensorChooser.Kinect.ColorStream.IsEnabled)
			{
				kinectSensorChooser.Kinect.ColorStream.Disable();
			}

			if (kinectSensorChooser.Kinect.DepthStream.IsEnabled)
			{
				kinectSensorChooser.Kinect.DepthStream.Disable();
			}
		}
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			StopKinect();
			this.Close();
			mainWindow.Show();
		}
	}
}

