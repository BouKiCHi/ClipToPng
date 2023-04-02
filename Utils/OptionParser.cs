using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
  public class OptionParser {

    public List<string> FileList;

    public class OptionSetting {
      /// <summary>
      /// ヘルプ表示機能の使用
      /// </summary>
      public bool HelpOption = true;

      public string CommandName { get; set; }

      public string RequiredCommand { get; set; }

      public List<OptionInfo> OptionList { get; set; }
      public int RequiredFileCount { get; set; } = 0;

      public void ShowUsage() {
        if (CommandName != null) {
          var FilePart = RequiredFileCount > 0 ? "File" : "[File]";

          var Options = "[Options...]";
          if (RequiredCommand != null) {
            Options = $"<{RequiredCommand}> {Options}";
          }

          Console.WriteLine($"Usage {CommandName} {Options} {FilePart}");
        }

        Console.WriteLine(" Options...");
        foreach (var o in OptionList) {
          if (o.Description == null) continue;
          Console.WriteLine($" {o.OptionKey,-15} : {o.Description}");
        }

        if (HelpOption) {
          var Help = "-h, --help, -?";
          Console.WriteLine($" {Help,-15} : ヘルプ表示");
        }
      }
    }


    public OptionSetting Setting { get; }

    public OptionParser(OptionSetting Setting) {
      this.Setting = Setting;
    }

    /// <summary>
    /// 使用方法の表示
    /// </summary>
    public void ShowUsage() => Setting.ShowUsage();

    /// <summary>
    /// オプションのパース
    /// </summary>
    public bool Parse(string[] CommandArgument) {
      FileList = new List<string>();
      var OptionDictionary = Setting.OptionList.ToDictionary(x => x.OptionKey);

      for (int i = 0; i < CommandArgument.Length; i++) {
        var a = CommandArgument[i];
        if (!a.StartsWith("-")) {
          FileList.Add(a);
          continue;
        }

        if (Setting.HelpOption) {
          if (a == "-?" || a == "-h" || a == "--help") {
            ShowUsage();
            return false;
          }
        }

        if (!OptionDictionary.ContainsKey(a)) {
          Console.WriteLine($"Invalid Option: {a}");
          continue;
        }

        var oi = OptionDictionary[a];
        var args = CommandArgument.Skip(i + 1).TakeWhile(n => !n.StartsWith("-")).ToArray();

        if (0 < oi.ArgumentCount && oi.ArgumentCount < args.Length) args = args.Take(oi.ArgumentCount).ToArray();

        oi.Handler(args);
        i += args.Length;
      }

      if (FileList.Count < Setting.RequiredFileCount) ShowUsage();
      return true;
    }
  }

  public class OptionInfo {
    /// <summary>
    /// オプション指定 -a --mail など
    /// </summary>
    public string OptionKey;

    /// <summary>
    /// 詳細 Usageで表示される
    /// </summary>
    public string Description;

    /// <summary>
    /// 引数の数
    /// </summary>
    public int ArgumentCount;

    /// <summary>
    /// 最大でArgumentCountで指定した数だけパラメータが渡される
    /// </summary>
    public delegate void OptionHandler(string[] Argument);

    public OptionHandler Handler;
  }
}
