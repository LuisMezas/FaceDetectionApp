using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;

namespace FaceDetectionApp
{
    public partial class Form1 : Form
    {
        Image<Bgr, Byte> imgOrg; //image type RGB (or Bgr as we say in Open CV)
        private Capture capturecam;
        private CascadeClassifier _cascadeClassifier;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                capturecam = new Capture();
            }
            catch (NullReferenceException exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
            Application.Idle += new EventHandler(ProcessFunction);
        }

        private void ProcessFunction(object sender, EventArgs arg)
        {
            //imgOrg = capturecam.QueryFrame().ToImage<Bgr, Byte>(); // error line
            //imgCamUser.Image = imgOrg;
            _cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_alt2.xml");
            using (imgOrg = capturecam.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imgOrg != null)
                {
                    var grayframe = imgOrg.Convert<Gray, byte>();
                    var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                    foreach (var face in faces)
                    {
                        imgOrg.Draw(face, new Bgr(Color.Blue), 1); //the detected face(s) is highlighted here using a box that is drawn around it/them
                    }
                }
                imageBox1.Image = imgOrg;
            }
        }
    }
}
