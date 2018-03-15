using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUICore : MonoBehaviour {

	public Canvas start;
	public Button callNative;
	public Text textFromNative;

	public void callNativePart(){
		textFromNative.text = "Button clicked";

		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"))
		using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
			obj_Activity.CallStatic ("backToNative", new object[] { });
		}
	}

}
