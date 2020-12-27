using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
    public class OptionParser {

        public List<OptionInfo> OptionList;

        public Dictionary<string, OptionInfo> OptionDictionary;
        
        public List<string> FileList;

        public int MinimumFileCount;
        public string CommandName;

        public bool HelpOption = true;

        public OptionParser(string commandName, List<OptionInfo> optionList, int minimumFileCount = 0) {
            CommandName = commandName;
            OptionList = optionList;
            OptionDictionary = OptionList.ToDictionary(x => x.Option);
            MinimumFileCount = minimumFileCount;
        }

        public void ShowUsage() {
            if (CommandName != null) {
                var FilePart = MinimumFileCount > 0 ? "File" : "[File]";
                Console.WriteLine($"Usage {CommandName} [Options...] {FilePart}");
            }

            Console.WriteLine(" Options...");
            foreach (var o in OptionList) {
                if (o.Description == null) continue;
                Console.WriteLine($" {o.Option,-15} : {o.Description}");
            }

            if (HelpOption) {
                var Help = "-h, --help, -?";
                Console.WriteLine($" {Help, -15} : ヘルプ表示");
            }

        }

        public bool Parse(string[] CommandArgument) {
            FileList = new List<string>();

            for (int i = 0; i < CommandArgument.Length; i++) {
                var a = CommandArgument[i];
                if (!a.StartsWith("-")) {
                    FileList.Add(a);
                    continue;
                }

                if (HelpOption) {
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

            if (FileList.Count < MinimumFileCount) ShowUsage();
            return true;
        }
    }

    public class OptionInfo {
        /// <summary>
        /// オプション指定 -a --mail など
        /// </summary>
        public string Option;

        /// <summary>
        /// 詳細 Usageで表示される
        /// </summary>
        public string Description;

        /// <summary>
        /// 引数の数
        /// </summary>
        public int ArgumentCount;

        public delegate void OptionHandler(string[] Argument);

        public OptionHandler Handler;
    }
}
