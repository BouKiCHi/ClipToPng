using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ClipToPng {
    class Program {

        [STAThread]
        static void Main(string[] args) {

            var OutputPath = args.Length > 0 ? args[0] : null;
            if(OutputPath == null) OutputPath = GetDesktopFilepath("Picture.png");

            OutputPath = OutputFilename(OutputPath);
            if (OutputPath == null) {
                Console.WriteLine("Wrong Output Filename!");
                return;
            }

            Console.WriteLine($"Output: {OutputPath}");
            var Image = Clipboard.GetImage();
            if (Image == null) {
                Console.WriteLine("No Image!");
                return;
            }
            Image.Save(OutputPath, GetFormatFromExtension(OutputPath));
        }

        private static ImageFormat GetFormatFromExtension(string p) {
            string ext = Path.GetExtension(p).ToLower();
            switch(ext) {
                case ".png":
                    return ImageFormat.Png;
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".ico":
                    return ImageFormat.Icon;
                case ".gif":
                    return ImageFormat.Gif;
            }

            return ImageFormat.Png;
        }

        private static string GetDesktopFilepath(string Filename) {
            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(Desktop, Filename);
        }

        static string OutputFilename(string FilePath) {
            string Folder = Path.GetDirectoryName(FilePath);
            string Name = Path.GetFileNameWithoutExtension(FilePath);
            string Extension = Path.GetExtension(FilePath);

            if(Extension.Length < 2) Extension = ".png";
            var p = Path.Combine(Folder,$"{Name}{Extension}");

            if(!File.Exists(p)) return p;
            int Count = 0;
            while(Count < 1000) {
                p = $"{Name}_{Count:000}{Extension}";
                p = Path.Combine(Folder,p);

                if(!File.Exists(p)) return p;
                Count++;
            }
            return null;
        }
    }
}
