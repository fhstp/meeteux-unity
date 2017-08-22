using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SayingScript : MonoBehaviour, IPointerDownHandler {

    public int SayingId = 0;
    public string[] DialogueStrings = new string[1]{""};
	public GameObject SayingManager = null;
    private StateManagement StateManager = null;

	// Use this for initialization
	void Start () {
        StateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManagement>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!StateManager.IsFreeForNextSaying())
        {
            return;
        }
        StateManager.SetFreeForNextSaying(false);
        if (SayingManager == null)
        {
            SayingManager = GameObject.FindGameObjectWithTag("SayingManager");
        }
		//eventData.pointerEnter;
		SayingManager.GetComponent<SayingManagement>().SomethingSaid(DialogueStrings, SayingId);
	} 
	
	// Update is called once per frame
	void Update () {
		
	}
}
