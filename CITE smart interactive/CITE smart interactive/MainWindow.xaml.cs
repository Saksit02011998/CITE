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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace CITE_smart_interactive
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		KinectSensorChooser kinectSensorChooser;
		//KinectSensor camKinect;
		Camera camera;


		public MainWindow()
		{
			InitializeComponent();
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Start();
			//camKinect = KinectSensor.KinectSensors.FirstOrDefault();
			//camKinect.Start();
			//camKinect.ColorStream.Enable();
			//camKinect.ColorFrameReady += camKinect_ColorFrameReady;
		}

		public void Start()
		{
			this.kinectSensorChooser = new KinectSensorChooser();
			this.kinectSensorChooser.KinectChanged += this.KinectSensorChooserOnKinectChanged;
			this.kinectSensorChooserUI.KinectSensorChooser = this.kinectSensorChooser;
			this.kinectSensorChooser.Start();
			KinectRegion.SetKinectRegion(this, kinectRegion);
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
		private void KinectSensorChooserOnKinectChanged(object sender, KinectChangedEventArgs e)
		{
			bool error = false;
			if (e.OldSensor != null)
			{

				e.OldSensor.DepthStream.Range = DepthRange.Default;
				e.OldSensor.DepthStream.Disable();
				e.OldSensor.SkeletonStream.Disable();
			}
			if (e.NewSensor != null)
			{
				try
				{

					e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
					e.NewSensor.SkeletonStream.Enable();
					e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;

				}
				catch (InvalidOperationException) { error = true; }
			}
			if (!error) { kinectRegion.KinectSensor = e.NewSensor; }
		}

		private void Btn_Camera_Click(object sender, RoutedEventArgs e)
		{
			camera = new Camera();
			StopKinect();
			this.Close();
			camera.Show();
		}

		void camKinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
		{
			using (ColorImageFrame frameImagen = e.OpenColorImageFrame())
			{
				if (frameImagen == null) return;
				byte[] datosColor = new byte[frameImagen.PixelDataLength];

				frameImagen.CopyPixelDataTo(datosColor);

				//camera.Source = BitmapSource.Create(
				//	frameImagen.Width, frameImagen.Height,
				//	96,
				//	96,
				//	PixelFormats.Bgr32,
				//	null,
				//	datosColor,
				//	frameImagen.Width * frameImagen.BytesPerPixel
				//	);
			}

		}
	}
}
