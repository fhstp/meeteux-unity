using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class SwipeTrail : MonoBehaviour {

	private LineRenderer lines;
	private Vector3 pathNode;
	public SocketIOClient socketClient;
	private string guid;

	public Text framerateText;

	public Button btn_red;
	public Button btn_blue;
	public Button btn_purple;
	public Button btn_yellow;
	public Button btn_green;

	private string color;
	private Color red = new Color (1,0.47f,0.47f);
	private Color blue = new Color(0.3f,0.54f,0.82f);
	private Color yellow = new Color(1,0.9f,0);
	private Color purple = new Color(0.81f,0.32f,0.8f);
	private Color green = new Color(0.2f,0.83f,0.26f);

	// Use this for initialization
	void Start () {
		Camera mainCamera = Camera.main;
		if( mainCamera )
		{
			if( mainCamera.GetComponent<VuforiaBehaviour>() != null )
			{
				mainCamera.GetComponent<VuforiaBehaviour>().enabled = false;
			}
			if( mainCamera.GetComponent<VideoBackgroundBehaviour>( ) != null )
			{
				mainCamera.GetComponent<VideoBackgroundBehaviour>().enabled = false;
			}
			if( mainCamera.GetComponent<DefaultInitializationErrorHandler>( ) != null )
			{
				mainCamera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
			}

			//mainCamera.clearFlags = CameraClearFlags.Skybox;
		} 

		lines = GetComponent<LineRenderer> ();
		lines.positionCount = 0;
		pathNode = new Vector3 ();
		guid = System.Guid.NewGuid ().ToString();

		btn_yellow.onClick.AddListener(yellowClicked);
		btn_red.onClick.AddListener(redClicked);
		btn_green.onClick.AddListener(greenClicked);
		btn_purple.onClick.AddListener(purpleClicked);
		btn_blue.onClick.AddListener(blueClicked);

		float random = Random.Range (0, 50.0f);
		if (random <= 10f)
			blueClicked ();
		else if (random > 10f && random <= 20f)
			greenClicked ();
		else if (random > 20f && random <= 30f)
			yellowClicked ();
		else if (random > 30f && random <= 40f)
			redClicked ();
		else if (random > 40f)
			purpleClicked ();
	}
	
	// Update is called once per frame
	void Update () {
		float frameRate = 1.0f / Time.deltaTime;
		framerateText.text = "Framerate: " + frameRate;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			lines.positionCount = 0;
			guid = System.Guid.NewGuid ().ToString();
		}

		if (((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) || Input.GetMouseButton (0))) 
		{
			Plane objPlane = new Plane (Camera.main.transform.forward * -1, this.transform.position);

			Ray mRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			float rayDistance;
			if (objPlane.Raycast (mRay, out rayDistance)) 
			{
				pathNode = mRay.GetPoint (rayDistance);
				lines.positionCount++;
				lines.SetPosition(lines.positionCount-1, pathNode);

				socketClient.transmitDrawingData (guid, pathNode, color);
			}
		}
	}

	public void yellowClicked()
	{
		//Debug.Log ("yellow clicked");
		color = "yellow";
		lines.startColor = yellow;
		lines.endColor = yellow;
	}

	public void blueClicked()
	{
		//Debug.Log ("blue clicked");
		color = "blue";
		lines.startColor = blue;
		lines.endColor = blue;
	}

	public void greenClicked()
	{
		//Debug.Log ("green clicked");
		color = "green";
		lines.startColor = green;
		lines.endColor = green;
	}

	public void purpleClicked()
	{
		//Debug.Log ("purple clicked");
		color = "purple";
		lines.startColor = purple;
		lines.endColor = purple;
	}

	public void redClicked()
	{
		//Debug.Log ("red clicked");
		color = "red";
		lines.startColor = red;
		lines.endColor = red;
	}
}
