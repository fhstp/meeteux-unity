using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialExternalCallScript : MonoBehaviour {

    public GameObject SayingManager;
    public GameObject StateManager;
    public GameObject ItemManager;
    
    public void GiveMoneyToRoommate (string message) {
        SayingManagement SM = SayingManager.GetComponent<SayingManagement>();
        StateManagement STM = StateManager.GetComponent<StateManagement>();
        if (STM.GetCurrentTriggerName() == "Roommate" && !STM.GetMoneyToRoommateDone())
        {
            SM.SomethingSaid(new string[2] { "Erinnerst du dich hiermit an etwas mehr?", "Oha... Da kommt plötzlich tatsächlich etwas ins Gedächtnis zurück. Tatyana hat kürzlich diesen Brief bekommen. Hier. Du kannst ihn haben." }, 100);
        }
        else if (StateManager.GetComponent<StateManagement>().GetCurrentTriggerName() == "Roommate" && StateManager.GetComponent<StateManagement>().GetMoneyToRoommateDone())
        {
            SM.SomethingSaid(new string[2] { "Erinnerst du dich hiermit an etwas mehr?", "Was willst du noch? Ich hab dir den Brief doch schon gegeben. Mehr weiß ich echt nicht. Das Geld nehm ich trotzdem gern." }, 101);
        }
    }

    public string GetCurrentTriggerName()
    {
        return StateManager.GetComponent<StateManagement>().GetCurrentTriggerName();
    }

    public void ReloadScene(string message)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
