using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using Xunit.Sdk;
using ImgProc;

namespace ImageProcessingUnitTest
{
    [TestClass]
    public class ImageProcessingUnitTest
    {
        const int size = 2;
        Bitmap red = new Bitmap(size, size);
        Bitmap green = new Bitmap(size, size);
        Bitmap blue = new Bitmap(size, size);
        Bitmap moreRed = new Bitmap(size, size);
        Bitmap moreGreen = new Bitmap(size, size);
        Bitmap moreBlue = new Bitmap(size, size);
        Bitmap moreRedAndGreen = new Bitmap(size, size);
        Bitmap moreRedAndBlue = new Bitmap(size, size);
        Bitmap moreBlueAndGreen = new Bitmap(size, size);
        Bitmap equalColors = new Bitmap(size, size);

        [TestMethod]
        public void TestToMainColorMethod()
        {
            red = SetOneColor(red, 255, 0, 0);
            green = SetOneColor(green, 0, 255, 0);
            blue = SetOneColor(blue, 0, 0, 255);
            moreRed = SetOneColor(moreRed, 255, 100, 100);
            moreGreen = SetOneColor(moreGreen, 100, 255, 100);
            moreBlue = SetOneColor(moreBlue, 100, 100, 255);
            moreRedAndGreen = SetOneColor(moreRedAndGreen, 100, 100, 0);
            moreRedAndBlue = SetOneColor(moreRedAndBlue, 100, 0, 100);
            moreBlueAndGreen = SetOneColor(moreBlueAndGreen, 0, 100, 100);
            equalColors = SetOneColor(equalColors, 100, 100, 100);


            Assert.AreEqual(red.GetPixel(1, 1), ImageProcessing.ToMainColors(moreRed).GetPixel(1, 1));
            Assert.AreEqual(green.GetPixel(1, 1), ImageProcessing.ToMainColors(moreGreen).GetPixel(1, 1));
            Assert.AreEqual(blue.GetPixel(1, 1), ImageProcessing.ToMainColors(moreBlue).GetPixel(1, 1));
            Assert.AreEqual(green.GetPixel(1, 1), ImageProcessing.ToMainColors(moreRedAndGreen, ImageProcessing.ColorPriority.GRB).GetPixel(1, 1));
            Assert.AreEqual(blue.GetPixel(1, 1), ImageProcessing.ToMainColors(moreRedAndBlue, ImageProcessing.ColorPriority.GBR).GetPixel(1, 1));
            Assert.AreEqual(blue.GetPixel(1, 1), ImageProcessing.ToMainColors(moreBlueAndGreen, ImageProcessing.ColorPriority.BRG).GetPixel(1, 1));
            Assert.AreEqual(red.GetPixel(1, 1), ImageProcessing.ToMainColors(equalColors, ImageProcessing.ColorPriority.RBG).GetPixel(1, 1));

        }

        private Bitmap SetOneColor(Bitmap bmp, int R, int G, int B)
        {
            for (int x = 0; x < bmp.Height; x++)
            {
                for (int y = 0; y < bmp.Width; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
               
            return bmp;
        }
    }
}
