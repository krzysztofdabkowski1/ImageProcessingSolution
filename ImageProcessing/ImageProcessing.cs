using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImgProc
{

    public static class ImageProcessing
    {
        public enum ColorPriority{
            RGB = 0,
            RBG = 1,
            GRB = 2,
            GBR = 3,
            BRG = 4,
            BGR = 5
        };
   

        public static Bitmap LoadImage(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                throw new DirectoryNotFoundException();
            }

            Bitmap img = new Bitmap(Image.FromFile(path));
            return img;
        }

        public static void SaveImage(Bitmap img, string path, bool resave = false)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                throw new DirectoryNotFoundException();
            }

            if(resave)
            {
                File.Delete(path);
            }

            ImageFormat format;
            format = ImageFormat.Png;
            if(path.EndsWith(".png"))
                format = ImageFormat.Png;
            else if(path.EndsWith(".jpg"))
                format = ImageFormat.Jpeg;
            else if(path.EndsWith(".bmp"))
                format = ImageFormat.Bmp;

            img.Save(path, format);
        }
        public static async Task<Bitmap> ToMainColorsAsync(Bitmap image, ColorPriority priority = ColorPriority.RGB)
        {
            Bitmap clone = new Bitmap(image);
            Bitmap result = await Task.Run(() => setMainColorOnMap(clone, priority));
            return result;
        }

        public static Bitmap ToMainColors(Bitmap image, ColorPriority priority = ColorPriority.RGB)
        {
            return setMainColorOnMap(image, priority);
        }

   
       

        private unsafe static Bitmap setMainColorOnMap(Bitmap img, ColorPriority priority)
        {
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(img.PixelFormat) / 8;


            byte* ptrFirstPixel = (byte*)imgData.Scan0;
            byte* R = null, G = null, B = null;
            byte** color1 = &R, color2 = &G, color3 = &B;

            SetColorPriorityPointers(priority, &R, &G, &B, ref color1, ref color2, ref color3);


            int amountInBytes = imgData.Height * imgData.Width;

            for(int x = 0; x < amountInBytes; x++)
            {
                int currentPixel = (x * bytesPerPixel);
                R = (ptrFirstPixel + currentPixel + 2);
                G = (ptrFirstPixel + currentPixel + 1);
                B = (ptrFirstPixel + currentPixel);

                SetPixelMainColor(ref **color1,
                                  ref **color2,
                                  ref **color3);
            }

            img.UnlockBits(imgData);

            return img;
        }

        private unsafe static void SetColorPriorityPointers(ColorPriority priority, byte** R, byte** G, byte** B, ref byte** color1, ref byte** color2, ref byte** color3)
        {
                switch (priority)
                {
                    case ColorPriority.RGB:
                        color1 = R;
                        color2 = G;
                        color3 = B;
                        break;
                    case ColorPriority.RBG:
                        color1 = R;
                        color2 = B;
                        color3 = G;
                        break;
                    case ColorPriority.GRB:
                        color1 = G;
                        color2 = R;
                        color3 = B;
                        break;
                    case ColorPriority.GBR:
                        color1 = G;
                        color2 = B;
                        color3 = R;
                        break;
                    case ColorPriority.BRG:
                        color1 = B;
                        color2 = R;
                        color3 = G;
                        break;
                    case ColorPriority.BGR:
                        color1 = B;
                        color2 = G;
                        color3 = R;
                        break;
                }
        }
        private unsafe static void SetPixelMainColor(ref byte color1,ref byte color2,ref byte color3)
        {
            if (color1 >= color2)
            {
                if (color1 >= color3)
                {
                    color1 = 255;
                    color2 = 0;
                    color3 = 0;
                }
                else
                {
                    color1 = 0;
                    color2 = 0;
                    color3 = 255;
                }
            }
            else
            {
                if (color2 >= color3)
                {
                    color1 = 0;
                    color2 = 255;
                    color3 = 0;
                }
                else
                {
                    color1 = 0;
                    color2 = 0;
                    color3 = 255;
                }

            }
        }
    }


}
