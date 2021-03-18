using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ImageProcessing;
class Program
{
    static async Task Main(string[] args)
    {
        const string imgPath = "C:\\Users\\Asus\\Desktop\\db5.png";
        int input = 0; 
        int input2 = 0;

        try
        {
            Stopwatch sw = new Stopwatch();

           
            Bitmap img = ImageProcessing.ImageProcessing.LoadImage(imgPath);
            sw.Start();
            img = ImageProcessing.ImageProcessing.ToMainColors(img, ImageProcessing.ImageProcessing.ColorPriority.BGR);
            sw.Stop();
            ImageProcessing.ImageProcessing.SaveImage(img, Directory.GetCurrentDirectory() + "\\nowy.png");
            
            Console.WriteLine(string.Format("Processed using ProcessUsingGetPixel method in {0} ms.", sw.ElapsedMilliseconds));
            
            
            img = ImageProcessing.ImageProcessing.LoadImage(imgPath);
            sw.Start();
            Bitmap task = await ImageProcessing.ImageProcessing.ToMainColorsAsync(img, ImageProcessing.ImageProcessing.ColorPriority.GBR);
            sw.Stop();
            ImageProcessing.ImageProcessing.SaveImage(task, Directory.GetCurrentDirectory() + "\\nowy.png");
            Console.WriteLine(string.Format("Processed using ProcessUsingGetPixel method in {0} ms.", sw.ElapsedMilliseconds));
            
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
        }

        return;
    }
}