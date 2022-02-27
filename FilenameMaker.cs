using System.Drawing.Imaging;
using System.IO;
using Utils;

namespace ClipToPng {

    /// <summary>
    /// ファイル名を生成
    /// </summary>
    class FilenameMaker {
        public int Count = 1;
        public string Prefix = "image";
        public ImageUtils.ImageFormatType SaveFormat = ImageUtils.ImageFormatType.Png;

        public ImageFormat GetFormat() {
            return ImageUtils.GetImageFormatFromFormatType(SaveFormat);
        }

        private string GetName() {
            return Prefix + ImageUtils.GetExtensionFromFormatType(SaveFormat);
        }

        public string GetPath() {
            var FilePath = GetName();
            string Folder = Path.GetDirectoryName(FilePath);
            string Name = Path.GetFileNameWithoutExtension(FilePath);
            string Extension = Path.GetExtension(FilePath);

            if (Extension.Length < 2) Extension = ".png";

            while (Count < 10000) {
                var p = $"{Name}{Count:D4}{Extension}";
                p = Path.Combine(Folder, p);

                if (!File.Exists(p)) return p;
                Count++;
            }
            return null;
        }
    }
}
