using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjectManagement : MonoBehaviour {
    public GameObject SayingManager;
    public GameObject ItemManager;
    public GameObject StateManager;

    public GameObject DisplayIcons;
    public GameObject DisplaySayings;
    public GameObject SayingPanel;
    private bool SomethingTriggered = false;


    public void ObjectTriggered(GameObject TriggeredObject) {
        if (SomethingTriggered)
            return;
        SomethingTriggered = true;
        if (TriggeredObject.tag == "Person") {
            SayingManager.GetComponent<SayingManagement>().StartSayingManagement(TriggeredObject);
        }
        else if (TriggeredObject.tag == "Item") {
            ItemManager.GetComponent<ItemManagement>().StartItemManagement(TriggeredObject);
        }
        StateManager.GetComponent<StateManagement>().SetCurrentTriggerName(TriggeredObject.name);

    }

    public void ObjectLost(GameObject LostObject) {
        if (!SomethingTriggered)
            return;
        SomethingTriggered = false;
        DisplayIcons.SetActive(false);
        DisplaySayings.SetActive(false);
        SayingPanel.SetActive(false);
        if (LostObject.tag == "Person")
        {
            SayingManager.GetComponent<SayingManagement>().CancelInvokesAndResetEverything();
        }
        else if (LostObject.tag == "Item")
        {
            ItemManager.GetComponent<ItemManagement>().CancelInvokesAndResetEverything();
        }
        StateManager.GetComponent<StateManagement>().SetCurrentTriggerName("none");

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
