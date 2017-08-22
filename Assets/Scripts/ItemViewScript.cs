using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemViewScript : MonoBehaviour, IPointerDownHandler {

    public GameObject ItemManager;

    // Use this for initialization
    void Start () {
		
	}

    public void OnPointerDown(PointerEventData eventData) {
        //eventData.pointerEnter;
        ItemManager.GetComponent<ItemManagement>().ViewItem();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
