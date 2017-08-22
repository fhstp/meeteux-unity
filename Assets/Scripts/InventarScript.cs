using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventarScript : MonoBehaviour, IPointerDownHandler {

	// Use this for initialization
	void Start () {
		
	}

    public void OnPointerDown(PointerEventData eventData) {
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
        using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
        {
            obj_Activity.CallStatic("openInventar", new object[] {});
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
