using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace ClipToPng {
    class Clip2Png {

        bool RepeatSave = false;
        string FilenamePrefix = "image";
        ImageUtils.ImageFormatType SaveFormat = ImageUtils.ImageFormatType.Png;

        public void Run(string[] args) {
            var opt = new OptionParser("Clip2Png", new List<OptionInfo>() {
                new OptionInfo() { Option = "-f", Description = "フォーマット指定 png(default),jpg,gif", ArgumentCount = 1, Handler = FormatSetting  },
                new OptionInfo() { Option = "-p", Description = "ファイル名接頭辞 default: image", ArgumentCount = 1, Handler = PrefixSetting  },
                new OptionInfo() { Option = "-c", Description = "繰り返し保存", ArgumentCount = 0, Handler = arg => {
                    RepeatSave = true;
                } } 
            });
            if (!opt.Parse(args)) return;

            RunMain(args);
        }

        private void PrefixSetting(string[] Argument) {
            if (Argument.Length == 0) return;
            FilenamePrefix = Argument[0];
        }

        private void FormatSetting(string[] Argument) {
            if (Argument.Length == 0) return;
            var Format = Argument[0];
            SaveFormat = ImageUtils.GetFormatTypeFromText(Format);
        }


        int ClipSequenceNo = -1;
        int LastClipSequenceNo = -1;
        private void RunMain(string[] args) {
            Console.WriteLine("Clip2Png");
            Console.WriteLine("Waiting Image...");

            string OutputFilename = FilenamePrefix + ImageUtils.GetExtensionFromFormatType(SaveFormat);

            System.Drawing.Imaging.ImageFormat OutputClipFormat = ImageUtils.GetImageFormatFromFormatType(SaveFormat);



            while (true) {
                ClipSequenceNo = (int)ImageUtils.GetClipboardSequenceNumber();

                if (!ClipImageCheck(OutputFilename, OutputClipFormat)) return;

                System.Threading.Thread.Sleep(200);
            }
        }

        private bool ClipImageCheck(string OutputFilename, System.Drawing.Imaging.ImageFormat OutputClipFormat) {
            if (LastClipSequenceNo == ClipSequenceNo) {
                return true;
            }

            LastClipSequenceNo = ClipSequenceNo;

            System.Drawing.Image ClipImage = Clipboard.GetImage();
            if (ClipImage == null) return true;

            var OutputPath = PathUtils.OutputFilename(OutputFilename);

            if (OutputPath == null) {
                Console.WriteLine("Wrong Output Filename!");
                return false;
            }

            Console.WriteLine($"Output: {OutputPath} ClipSequenceNo:{ClipSequenceNo} DateTime:{DateTime.Now}");
            ClipImage.Save(OutputPath, OutputClipFormat);
            ClipImage.Dispose();
            return RepeatSave;
        }
    }
}
