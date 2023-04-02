using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Utils;

namespace ClipToPng {

  class Clip2Png {
    private const string ProgramName = "Clip2Png ver 20230402";

    /// <summary>出力ファイル名</summary>
    FilenameMaker OutputFilename = new FilenameMaker();

    /// <summary>コマンド実行</summary>
    bool RunCommand = false;

    /// <summary>繰り返し保存</summary>
    bool RepeatSave = true;

    /// <summary>詳細表示</summary>
    bool Verbose = false;

    /// <summary>クリップ番号管理</summary>
    private ClipSequence ClipSequence = null;

    /// <summary>
    /// エントリポイント
    /// </summary>
    public void Run(string[] args) {

      var ops = new OptionParser.OptionSetting() {
        CommandName = "Clip2Png",
        OptionList = new List<OptionInfo>() {
              new OptionInfo() { OptionKey = "-c", Description = "繰り返し保存", ArgumentCount = 0, Handler = arg => { RunCommand = true;  RepeatSave = true; } },
              new OptionInfo() { OptionKey = "-s", Description = "1枚を保存", ArgumentCount = 0, Handler = arg => { RunCommand = true; RepeatSave = false; } },
              new OptionInfo() { OptionKey = "-p", Description = "ファイル名接頭辞 既定: image", ArgumentCount = 1, Handler = PrefixSetting  },
              new OptionInfo() { OptionKey = "-f", Description = "フォーマット指定 png, jpg, gif (既定:png)", ArgumentCount = 1, Handler = FormatSetting  },
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


    private void RunMain(OptionParser opt, string[] args) {
      Console.WriteLine(ProgramName);

      // 実行
      if (!RunCommand) {
        opt.ShowUsage();
        return;
      }

      Save();
    }

    private void Save() {
      ClipSequence = new ClipSequence();

      if (RepeatSave) {
        RepeatSaveImage();
      } else {
        var ImageFile = OutputFilename.GetFilePath();
        SaveImage(ImageFile);
      }
    }

    /// <summary>
    /// 繰り返し保存
    /// </summary>
    private void RepeatSaveImage() {
      var ImageFile = OutputFilename.GetFilePath();

      Console.WriteLine("新しい画像イメージを待っています...");
      Console.WriteLine($"最初に保存する画像ファイル:{ImageFile.Filename}");

      while (true) {
        ClipSequence.WaitNext();

        if (!SaveImage(ImageFile)) return;

        // 繰り返さない
        if (!RepeatSave) { return; }

        // 次のファイル
        ImageFile = OutputFilename.GetFilePath();
      }
    }

    /// <summary>
    /// 画像の保存
    /// </summary>
    private bool SaveImage(FilenameUnit ImageFile) {
      if (ImageFile == null) {
        Console.WriteLine("出力ファイル名が生成できませんでした");
        return false;
      }

      // イメージ取得
      using var ClipImage = Clipboard.GetImage();
      if (ClipImage == null) {
        Console.WriteLine("イメージが取得できませんでした。");
        return true;
      }

      WriteImageToPath(ClipImage, ImageFile.FullName);

      return true;
    }

    private void WriteImageToPath(System.Drawing.Image ClipImage, string FullName) {
      using (var ms = new MemoryStream()) {

        var ClipFormat = OutputFilename.GetFormat();
        ClipImage.Save(ms, ClipFormat);

        var ImageBytes = ms.Length / 1000;
        Console.WriteLine($"出力: {FullName} 日時:{DateTime.Now} サイズ:{ImageBytes}KB");
        if (Verbose) {
          Console.WriteLine($"シーケンス番号: {ClipSequence.No}");
        }

        File.WriteAllBytes(FullName, ms.ToArray());
      }
    }
  }
}
