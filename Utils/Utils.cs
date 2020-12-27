using System;
using System.IO;

namespace Utils {
    public class PathUtils {

        public static string GetDesktopFilepath(string Filename) {
            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(Desktop, Filename);
        }

        public static string OutputFilename(string FilePath) {
            string Folder = Path.GetDirectoryName(FilePath);
            string Name = Path.GetFileNameWithoutExtension(FilePath);
            string Extension = Path.GetExtension(FilePath);

            if (Extension.Length < 2) Extension = ".png";
            var p = Path.Combine(Folder, $"{Name}{Extension}");

            int Count = 0;
            while (Count < 1000) {
                p = $"{Name}{Count:D4}{Extension}";
                p = Path.Combine(Folder, p);

                if (!File.Exists(p)) return p;
                Count++;
            }
            return null;
        }
    }

}
