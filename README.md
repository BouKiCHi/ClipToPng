## ClipToPng

クリップボードの画像データを画像ファイルに出力するWindows用コンソールアプリケーションです。  
.NET Framework 4.7.2を使用しています。


### 使用方法

ClipToPng [ファイル]

### 例
```ps
# デスクトップにPicture.pngを出力
ClipToPng

# pngファイル出力
CliptoPng \directory\MyPicture.png

# jpgファイル出力
CliptoPng \directory\MyPicture.jpg


```

### 特徴
* ファイル名未指定ではDesktopにPicture.pngを出力します。
* 出力ファイルがすでに存在する場合、付番します。最大999まで。  
例: Picture.png →　Picture_000.png　→ Picture_001.png ...

* 出力の拡張子によってpngとjpegのフォーマットを切り替えます。
