using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTakeScript : MonoBehaviour, IPointerDownHandler{

    public GameObject ItemManager;

    // Use this for initialization
    void Start () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        //eventData.pointerEnter;
        ItemManager.GetComponent<ItemManagement>().TakeItem();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
