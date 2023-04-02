using System.Drawing.Imaging;
using System.IO;
using Utils;

namespace ClipToPng {

  class FilenameUnit {
    public string Folder { get; }
    public string Filename { get; }
    public string FullName { get; internal set; }

    public FilenameUnit(string Foler, string Filename, string FullName) {
      this.Folder = Foler;
      this.Filename = Filename;
      this.FullName = FullName;
    }
  }

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

    public FilenameUnit GetFilePath() {
      var FilePath = GetName();

      var Folder = Path.GetDirectoryName(FilePath);
      var FilenameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
      var Extension = Path.GetExtension(FilePath);

      if (Extension.Length < 2) Extension = ".png";

      while (Count < 10000) {
        var Filename = $"{FilenameWithoutExtension}{Count:D4}{Extension}";
        var FullName = Path.Combine(Folder, Filename);

        if (!File.Exists(FullName)) return new FilenameUnit(Folder, Filename, FullName);
        Count++;
      }
      return null;
    }
  }
}
