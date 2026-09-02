using System.Drawing.Imaging;

class Program
{
    static void Main()
    {
        int noOfSc = 5;
        string folder = @"D:\Test";
        Directory.CreateDirectory(folder);
        Rectangle screen = Screen.PrimaryScreen.Bounds;
        for(int i = 0; i < noOfSc; i++)
        {
            using (Bitmap screenshot = new Bitmap(screen.Width, screen.Height))
            {
                using (Graphics graphics = Graphics.FromImage(screenshot))
                {
                    graphics.CopyFromScreen(
                        screen.Left,
                        screen.Top,
                        0, 0,
                        screen.Size
                        );
                }
                string file = $"screenshot_{i:D3}.png";
                string path = Path.Combine(folder, file);
                screenshot.Save(path, ImageFormat.Png);
            }
            Thread.Sleep(2*10000);
        }
        Console.WriteLine("Finished!!!!");
    }
}
