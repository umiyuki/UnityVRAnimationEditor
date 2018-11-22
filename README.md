# UnityVRAnimationEditor
In Unity3d editor playmode, You can edit animation clip in VR.

[日本語で読む](/README.ja.md)

![IK Sample](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/iksample.jpg)

![UI](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/ui.jpg)

![Node Sample](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/nodesample.jpg)

![Touchpad](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/images/touchpad.jpg)

## What is UnityVRAnimationEditor?
In the play mode of the Unity editor, you can grab and move objects in the VR.

You can also operate the editor's animation window from within VR.

In other words, you can edit animation clips in VR.

It is a hacky implementation that forcibly uses the function of the Unity editor, so there are various usage restrictions so far. (You can not operate without entering play mode etc.)

## Use

It is supposed to use it when you want to create character animation within Unity.

Create an animated video with Timeline.

I think that you can also create character animation for games.

## Required environment
OS supports Windows only

Unity 2018.3.0 or later version is required. Operation is confirmed with Unity 2018.3.0 b10.

VR headset is compatible with Vive. I am checking the operation with the regular version Vive.
Since VRTK is used, it may work with Oculus Rift or Windows MR device, but I have not verified it.
Please also install SteamVR on your PC.

When there is a FinalIK asset of the asset store, it is convenient for character animation editing. It is not inconvenient. Operation is confirmed with version 1.8.

## Details

Refer to manual for details

[マニュアル](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/Manual)

[自前のオブジェクトやキャラを操作する](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/Control your own objects and characters)

[アニメーションクリップの作成について](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/About creating animation clips)

[FAQ](https://github.com/umiyuki/UnityVRAnimationEditor/wiki/FAQ)

## Credit

[VRTK](https://vrtoolkit.readme.io/) thestonefox

[SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647) VALVE CORPORATION

[Oculus Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022) OCULUS

[uTouchInjection](https://github.com/hecomi/uTouchInjection) hecomi

[uWindowCapture](https://github.com/hecomi/uWindowCapture) hecomi

[Unity-chan](http://unity-chan.com/)  © UTJ/UCL

[inputsimulator](https://archive.codeplex.com/?p=inputsimulator) michaelnoonan
