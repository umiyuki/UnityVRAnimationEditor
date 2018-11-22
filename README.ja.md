# UnityVRAnimationEditor
Unity3Dエディタのプレイモードでアニメーションクリップを編集できるようにする拡張です。

![IK Sample](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/iksample.jpg)

![UI](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/ui.jpg)

![Node Sample](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/nodesample.jpg)

![Touchpad](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/touchpad.jpg)

## UnityVRAnimationEditorとは？
Unityエディタのプレイモードで、VR内でオブジェクトを掴んで動かせます。

また、VR内からエディタのアニメーションウインドウを操作できます。

つまり、VR内でアニメーションクリップを編集できます。

Unityエディタの機能を無理やり使ったハッキーな実装になってるので、今のところ色々と使用上の制約があります。（プレイモードに入らないと操作できないなど）

## 用途

Unity内でキャラクターのアニメーションを作成したい時に利用するのを想定しています。

Timelineでアニメーション動画を作成するなど。

ゲーム用のキャラクターアニメーションなども作成できると思います。

## 必要環境
OSはWindowsのみ対応してます

Unity2018.3.0以降のバージョンが必要となります。Unity2018.3.0b10で動作確認しています。

VRヘッドセットはViveに対応しています。通常版Viveで動作確認しています。  
VRTKを使っているのでOculus RiftやWindows MRデバイスでも動作するかもしれませんが、検証してません。  
PCにSteamVRもインストールしてください。

アセットストアのFinalIKアセットがあるとキャラクターのアニメーション編集で便利です。ないと不便です。バージョン1.8で動作確認しています。

## 詳細

詳細はマニュアル参照

[マニュアル](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/Manual.ja)

[自前のオブジェクトやキャラを操作する](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/Control-your-own-objects-and-characters.ja)

[アニメーションクリップの作成について](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/About-creating-animationclips.ja)

[FAQ](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/FAQ.ja)

## Credit

[VRTK](https://vrtoolkit.readme.io/) thestonefox

[SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647) VALVE CORPORATION

[Oculus Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022) OCULUS

[uTouchInjection](https://github.com/hecomi/uTouchInjection) hecomi

[uWindowCapture](https://github.com/hecomi/uWindowCapture) hecomi

[ユニティちゃん](http://unity-chan.com/)  © UTJ/UCL

[inputsimulator](https://archive.codeplex.com/?p=inputsimulator) michaelnoonan
