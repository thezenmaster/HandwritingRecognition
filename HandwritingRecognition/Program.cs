using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using AForge.Imaging;

namespace HandwritingRecognition
{
    class Program
    {
        static void Main(string[] args)
        {
            TrainNetwork();
            BackPropagation.BackPropagation.Test(1024, 3, 4);
        }

        private static void TrainNetwork()
        {
            GrayscaleInputImages();
            ImageFFT();
            BackPropagation.BackPropagation.Train(1024, 3, 4);
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
    }
}
