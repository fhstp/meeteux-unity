using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagement : MonoBehaviour {
    /* define state flags */
    public string currentTriggerName = "none";
    public bool moneyToRoommateDone = false;
    private bool freeForNextSaying = true;
    /* end state flags */


    /* define getters and setters */

    public bool IsFreeForNextSaying()
    {
        return freeForNextSaying;
    }

    public void SetFreeForNextSaying(bool yesOrNo)
    {
        freeForNextSaying = yesOrNo;
    }

    public bool GetMoneyToRoommateDone()
    {
        return moneyToRoommateDone;
    }

    public void SetMoneyToRoommateDone(bool doneState)
    {
        moneyToRoommateDone = doneState;
    }

    public string GetCurrentTriggerName()
    {
        return currentTriggerName;
    }

    public void SetCurrentTriggerName(string name)
    {
        currentTriggerName = name;
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.CallStatic("setCurrentTriggerObject", new object[] { name });
            }
        }
    }

    /* end  getters and setters */

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
