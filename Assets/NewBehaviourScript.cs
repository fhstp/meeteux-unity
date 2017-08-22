using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public int testVar = 0;
    public List<GameObject> li = new List<GameObject>();
    public bool act = false;

	// Use this for initialization
	void Start () {
        if (this.gameObject.name == "GameObject")
        {
            GameObject zz = (GameObject)Instantiate(this.gameObject, this.gameObject.transform);
            zz.GetComponent<NewBehaviourScript>().act = true;
        }
  
		
	}
	
	// Update is called once per frame
	void Update () {
        if (testVar<1000 && act)
        {
            testVar++;
            li.Add(new GameObject());
        }
		
	}
}
