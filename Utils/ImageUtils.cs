using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils {
    class ImageUtils {

        [DllImport("user32.dll")]
        public extern static int GetClipboardSequenceNumber();

        public enum ImageFormatType {
            Png,
            Jpeg,
            Bmp,
            Tiff,
            Gif,
            Ico
        }


        public static string GetExtensionFromFormatType(ImageFormatType FormatType) {
            switch (FormatType) {
                default: case ImageFormatType.Png: return ".png";
                case ImageFormatType.Jpeg: return ".jpg";
                case ImageFormatType.Ico: return ".ico";
                case ImageFormatType.Bmp: return ".bmp";
                case ImageFormatType.Tiff: return ".tiff";
                case ImageFormatType.Gif: return ".gif";
            }
        }

        public static ImageFormat GetImageFormatFromFormatType(ImageFormatType FormatType) {
            switch (FormatType) {
                default: case ImageFormatType.Png: return ImageFormat.Png;
                case ImageFormatType.Jpeg: return ImageFormat.Jpeg;
                case ImageFormatType.Ico: return ImageFormat.Icon;
                case ImageFormatType.Bmp: return ImageFormat.Bmp;
                case ImageFormatType.Tiff: return ImageFormat.Tiff;
                case ImageFormatType.Gif: return ImageFormat.Gif;
            }
        }

        public static ImageFormatType GetFormatTypeFromText(string Text) {
            switch(Text.ToLower()) {
                default: case "png": return ImageFormatType.Png;
                case "jpg": return ImageFormatType.Jpeg;
                case "ico": return ImageFormatType.Ico;
                case "bmp": return ImageFormatType.Bmp;
                case "tiff": return ImageFormatType.Tiff;
                case "gif": return ImageFormatType.Gif;
            }
        }


        public static ImageFormat GetFormatFromExtension(string Filename, ImageFormat Default = null) {
            string ext = Path.GetExtension(Filename).ToLower();
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

            return Default;
        }
    }
}
