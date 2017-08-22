using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ItemManagement : MonoBehaviour {

    public GameObject CurrentItem = null;
    public GameObject DisplaySaying;
    public GameObject DisplayIcons;

    private bool TempBlocked = false;
    
    private Dictionary<string, string> ItemViewSayings = new Dictionary<string, string>();

    public void StartItemManagement (GameObject GameItem) {
        CurrentItem = GameItem;
        DisplaySaying.SetActive(false);
        TempBlocked = false;
        DisplayIcons.SetActive(true);
        //Invoke("FakeDisable", 2);
    }

    public void FakeDisable()
    {
       // CurrentItem.GetComponent<DefaultTrackableEventHandler>().RemoveFromAR();
    }

    public void ViewItem () {
        if (TempBlocked || CurrentItem == null) {
            return;
        }
        TempBlocked = true;
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
        DisplayText.text = ItemViewSayings[CurrentItem.name];
        DisplaySaying.SetActive(true);
        Invoke("SayingDone", 2.5f);
    }

    public void TakeItem (bool takeByAction = false, string takeItemByName = "") {
        if (TempBlocked || CurrentItem == null) {
            return;
        }
        TempBlocked = true;
        string itemToTake = (takeItemByName == "") ? CurrentItem.name : takeItemByName;
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                obj_Activity.CallStatic("takeItem", new object[] { itemToTake });
            }
        }
        if(takeByAction)
        {
            TempBlocked = false;
            return;
        }
        //CurrentItem.SetActive(false);
        CurrentItem.GetComponent<DefaultTrackableEventHandler>().RemoveFromAR();
        DisplaySaying.SetActive(false);
        DisplayIcons.SetActive(false);
        CurrentItem = null;
        TempBlocked = false;
        //Invoke("SayingDone", 5);
    }


    internal void CancelInvokesAndResetEverything()
    {
        CancelInvoke("SayingDone");
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
        DisplayText.text = "";
        DisplaySaying.SetActive(false);
        DisplayIcons.SetActive(false);
        CurrentItem = null;
        //DisplaySaying.SetActive(false);
        //SayingPanel.SetActive(false);
        TempBlocked = false;
    }


    private void SayingDone() {
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
        DisplayText.text = "";
        DisplaySaying.SetActive(false);
       // CurrentItem = null;
        //DisplaySaying.SetActive(false);
        //SayingPanel.SetActive(false);
        TempBlocked = false;
    }


    // Use this for initialization
    void Start () {
        //TODO: Lateron, parse this from XML
        ItemViewSayings.Add("Tagebuch", "Das scheint Tatyanas Tagebuch zu sein... Es ist verschlossen. Mist..");
        ItemViewSayings.Add("Smartphone", "Tatyanas Smarphone. Mit... Sky OS 4.0 von Moon 13? Scheinbar kontrolliert dein Arbeitgeber Moon 13 sogar den Smartphone Markt in der Zukunft. Ein Bisschen creepy.");
        ItemViewSayings.Add("Haarnadel", "Eine Haarnadel. Wenn du sie einsteckst und einen Spiegel findest, kannst du dich ein wenig aufhübschen. Vielleicht gar nicht so schlecht?");
        ItemViewSayings.Add("Schlüssel", "Ein kleiner, mysteriöser Schlüssel! Zu welchen geheimen Welten könnte er dir wohl Zutritt verschaffen?");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
