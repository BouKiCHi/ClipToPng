using System;

namespace ClipToPng {

  class Program {

    [STAThread]
    static void Main(string[] args) {
      new Clip2Png().Run(args);
    }
  }
}
