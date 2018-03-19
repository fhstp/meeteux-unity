using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// We need this one for importing our IOS functions
using System.Runtime.InteropServices;

public class GUICore : MonoBehaviour {

	public Canvas start;
	public Button callNative;
	public Text textFromNative;

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void switchToNative(string msg);
	#endif

	public void callNativePart(){
		textFromNative.text = "Button clicked";

		#if UNITY_IPHONE
			print ("iPhone");
			switchToNative ("testmessage");
		#endif

		#if UNITY_ANDROID
			print ("Android");
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
			obj_Activity.CallStatic ("backToNative", new object[] { });
			}
		#endif
	}
}