using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImagePresenter.Model
{
    class ImageProcessorModel
    {
        private Bitmap bitmap;
        private Bitmap unprocessedBitmap;
        public string imagePath;
        public bool isBitmapProcessed { get; private set; }
        public string elapsedTime { get; private set; }

        public ImageProcessorModel()
        {
            bitmap = null;
            unprocessedBitmap = null;
            isBitmapProcessed = false;
        }

        public BitmapImage GetBitmapImage() 
        {
            if (bitmap == null)
            {
                return null;
            }
            else
            {
                return ConvertToBitmapImage(this.bitmap);
            }
        }

    
        public bool LoadImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.BMP; *.JPG; *.PNG)| *.BMP; *.JPG; *.PNG; ";
            fileDialog.ShowDialog();

            if(!fileDialog.FileName.Equals(""))
            {
                bitmap = ImageProcessing.ImageProcessing.LoadImage(fileDialog.FileName);
                unprocessedBitmap = new Bitmap(bitmap);
                imagePath = fileDialog.FileName;
                isBitmapProcessed = false;
                elapsedTime = null;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            bitmap = new Bitmap(unprocessedBitmap);
            isBitmapProcessed = false;
            elapsedTime = null;
        }
        public void ProcessToMainColors()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bitmap = ImageProcessing.ImageProcessing.ToMainColors(bitmap, ImageProcessing.ImageProcessing.ColorPriority.BRG);
            sw.Stop();
            elapsedTime = sw.ElapsedMilliseconds.ToString();
            isBitmapProcessed = true;
        }

        public async Task ProcessToMainColorsAsync()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bitmap = await ImageProcessing.ImageProcessing.ToMainColorsAsync(bitmap, ImageProcessing.ImageProcessing.ColorPriority.RGB);
            sw.Stop();
            elapsedTime = sw.ElapsedMilliseconds.ToString();
            isBitmapProcessed = true;
        }

        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = ""; 
            saveDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPG Image (.jpg)|*.jpg|Png Image (.png)|*.png"; ; 

            saveDialog.ShowDialog();

            if (!saveDialog.FileName.Equals(""))
            {
                ImageProcessing.ImageProcessing.SaveImage(bitmap, saveDialog.FileName, saveDialog.FileName == imagePath);
                isBitmapProcessed = true;
            }
        }

        private BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
