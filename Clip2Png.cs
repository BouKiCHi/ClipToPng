using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace ClipToPng {

    class Clip2Png {
        private const string ProgramName = "Clip2Png ver 20220227";

        /// 出力ファイル名
        FilenameMaker OutputFilename = new FilenameMaker();

        /// <summary>
        /// コマンド実行
        /// </summary>
        bool RunCommand = false;

        /// <summary>
        /// 繰り返し保存
        /// </summary>
        bool RepeatSave = true;

        /// <summary>
        /// 詳細表示
        /// </summary>
        bool Verbose = false;


        /// <summary>
        /// エントリポイント
        /// </summary>
        public void Run(string[] args) {

            var ops = new OptionParser.OptionSetting() {
                CommandName = "Clip2Png",
                OptionList = new List<OptionInfo>() {
                    new OptionInfo() { OptionKey = "-f", Description = "フォーマット指定 png,jpg,gif (既定:png)", ArgumentCount = 1, Handler = FormatSetting  },
                    new OptionInfo() { OptionKey = "-p", Description = "ファイル名接頭辞 既定: image", ArgumentCount = 1, Handler = PrefixSetting  },
                    new OptionInfo() { OptionKey = "-c", Description = "繰り返し保存", ArgumentCount = 0, Handler = arg => { RunCommand = true;  RepeatSave = true; } },
                    new OptionInfo() { OptionKey = "-s", Description = "1枚を保存", ArgumentCount = 0, Handler = arg => { RunCommand = true; RepeatSave = false; } },
                    new OptionInfo() { OptionKey = "-v", Description = "詳細表示", ArgumentCount = 0, Handler = arg => { Verbose = true; } },
                },
                RequiredCommand = "-c|-s",
            };

            var opt = new OptionParser(ops);

            if (!opt.Parse(args)) return;

            RunMain(opt, args);
        }

        /// <summary>
        /// 接頭辞の指定
        /// </summary>
        private void PrefixSetting(string[] Argument) {
            if (Argument.Length == 0) return;
            OutputFilename.Prefix = Argument[0];
        }

        /// <summary>
        /// フォーマット設定
        /// </summary>
        private void FormatSetting(string[] Argument) {
            if (Argument.Length == 0) return;
            var Format = Argument[0];
            OutputFilename.SaveFormat = ImageUtils.GetFormatTypeFromText(Format);
        }

        int ClipSequenceNo = -1;
        int LastClipSequenceNo = -1;
        private void RunMain(OptionParser opt, string[] args) {
            Console.WriteLine(ProgramName);

            // 実行
            if (!RunCommand) {
                opt.ShowUsage();
                return;
            }

            // 繰り返し保存
            if (RepeatSave) {
                RepeatSaveImage();
            } else {
                SaveImage();
            }
        }

        private void RepeatSaveImage() {
            Console.WriteLine("新しい画像イメージを待っています...");

            LastClipSequenceNo = (int)ImageUtils.GetClipboardSequenceNumber();

            while (true) {
                ClipSequenceNo = (int)ImageUtils.GetClipboardSequenceNumber();

                // 同じシーケンス番号の場合は処理しない
                if (LastClipSequenceNo == ClipSequenceNo) {
                    System.Threading.Thread.Sleep(200);
                    continue;
                }

                LastClipSequenceNo = ClipSequenceNo;

                if (!SaveImage()) return;

                // 繰り返さない
                if (!RepeatSave) { return; }
            }
        }

        /// <summary>
        /// 画像の保存
        /// </summary>
        private bool SaveImage() {
            // イメージ取得
            System.Drawing.Image ClipImage = Clipboard.GetImage();
            if (ClipImage == null) {
                Console.WriteLine("イメージが取得できませんでした。");
                return true;
            }


            var Path = OutputFilename.GetPath();
            if (Path == null) {
                Console.WriteLine("出力ファイル名が生成できませんでした");
                return false;
            }

            var ClipFormat = OutputFilename.GetFormat();
            Console.WriteLine($"出力: {Path} 日時:{DateTime.Now}");
            if (Verbose) {
                Console.WriteLine($"シーケンス番号: {ClipSequenceNo}");
            }

            ClipImage.Save(Path, ClipFormat);
            ClipImage.Dispose();
            return true;
        }
    }
}
