using System;
using System.IO;

namespace Utils {
    public class PathUtils {

        public static string GetDesktopFilepath(string Filename) {
            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(Desktop, Filename);
        }

    }

}
