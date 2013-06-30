using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandwritingRecognitionUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TrainBtn_Click(object sender, EventArgs e)
        {
            if (TrainingBox.Text != null && WeightsBox.Text != null)
            {
                TrainNetwork(TrainingBox.Text, WeightsBox.Text);
            }
        }

        private static void TrainNetwork(string path, string weightsPath)
        {
            GrayscaleInputImages();
            ImageFFT();
            BackPropagation.BackPropagation.Train(1024, 3, 4, path, weightsPath);
        }

        private static void GrayscaleInputImages()
        {
            string grayscalePath = @"..\..\..\Grayscale\";
            DirectoryInfo dir = new DirectoryInfo(grayscalePath);
            FileInfo[] files = dir.GetFiles();
            if (files.Length == 0)
            {
                string path = @"..\..\..\InputImages";
                dir = new DirectoryInfo(path);
                files = dir.GetFiles();
                foreach (var file in files)
                {
                    if (file.Extension == ".jpg")
                    {
                        Bitmap bitmap = new Bitmap(file.Open(FileMode.Open));
                        // create grayscale filter (BT709)
                        Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                        Bitmap grayImage = filter.Apply(bitmap);
                        grayImage.Save(grayscalePath + file.Name.Replace(".jpg", ".bmp"), ImageFormat.Bmp);
                    }
                }
            }
        }

        private static void ImageFFT()
        {
            string trainPath = @"..\..\..\Train\";
            DirectoryInfo dir = new DirectoryInfo(trainPath);
            FileInfo[] files = dir.GetFiles();
            if (files.Length == 0)
            {
                string path = @"..\..\..\Grayscale";
                dir = new DirectoryInfo(path);
                files = dir.GetFiles();
                foreach (var file in files)
                {
                    if (file.Extension == ".bmp")
                    {
                        Bitmap bitmap = new Bitmap(file.Open(FileMode.Open));
                        // create complex image
                        ComplexImage complexImage = ComplexImage.FromBitmap(bitmap);
                        // do forward Fourier transformation
                        complexImage.ForwardFourierTransform();
                        // get complex image as bitmat
                        Bitmap fourierImage = complexImage.ToBitmap();
                        fourierImage.Save(trainPath + file.Name, ImageFormat.Bmp);
                    }
                }
            }
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            if (TestBox.Text != null)
            {
                string result = BackPropagation.BackPropagation.Test(1024, 3, 4, TestBox.Text);
                ResultBox.Text = result;
            }
        }
    }
}
