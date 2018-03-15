using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExternalCallScript : MonoBehaviour {

	public Text textFromNative;

	public void calledFromNative (string message) {
		textFromNative.text = message;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
