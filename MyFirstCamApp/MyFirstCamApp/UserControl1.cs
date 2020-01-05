using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Threading;
using System.Threading;

namespace MyFirstCamApp
{
    public partial class UserControl1: UserControl
    {
        private FilterInfoCollection CaptureDevices;

        private VideoCaptureDevice VideoSource;
        Dispatcher dispatcher=Dispatcher.CurrentDispatcher;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in CaptureDevices)
            {
                DeviceListCombo.Items.Add(device.Name);
            }
            DeviceListCombo.SelectedIndex = 0;
            this.VideoSource = new VideoCaptureDevice();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
           
                VideoSource = new VideoCaptureDevice(CaptureDevices[DeviceListCombo.SelectedIndex].MonikerString);
                VideoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                VideoSource.Start();
               
          
        }
        private  void VideoSource_NewFrame(object sender,NewFrameEventArgs eventArgs)
        {
            Action a = () =>
            {
                try
                {
                    pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                }
                catch(Exception e)
                {
                    //MessageBox.Show(e.Message);
                    //PopUpAsync(e);
                }
            };

            SafeInvoke(a);
        }

        //private  void PopUpAsync(Exception e)
        //{
        //  Thread t= new Thread(() => 
        //  {
        //        MessageBox.Show(e.Message);
        //  });
        //    t.Start();
        //}

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            VideoSource.Stop();
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
            pictureBox2.Image = null;
            pictureBox2.Invalidate();
        }



        private void PauseBtn_Click(object sender, EventArgs e)
        {
            VideoSource.Stop();
        }



        private void CapBtn_Click(object sender, EventArgs e)
        {
            try
            {

                pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            }
            catch (Exception)
            {

            }       
        }

        private void SafeInvoke(Action action)
        {
            if (Thread.CurrentThread == Dispatcher.CurrentDispatcher.Thread)
            {
                action.Invoke();
            }
            dispatcher.BeginInvoke(action);
        }


        private void ExtBtn_Click(object sender, EventArgs e)
        {
            if (VideoSource.IsRunning)
            {
                VideoSource.Stop();
            }
            Application.Exit();
        }
    }
}
