using Utils;

namespace ClipToPng {
  class ClipSequence {
    public int No = -1;
    public ClipSequence() {
      No = (int)ImageUtils.GetClipboardSequenceNumber();
    }

    internal void WaitNext() {
      while(true) {
        var NextNo = (int)ImageUtils.GetClipboardSequenceNumber();

        // 異なるシーケンス番号
        if (No != NextNo) {
          No = NextNo;
          break;
        }
        System.Threading.Thread.Sleep(200);
      }

    }
  }
}
