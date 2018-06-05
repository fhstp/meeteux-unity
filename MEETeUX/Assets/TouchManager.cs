using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
	public float speed = 0.1F;
	private LineRenderer lines;
	private List<Vector3> pathNodes;

	// Use this for initialization
	void Start () {
		Input.simulateMouseWithTouches = enabled;
		lines = GetComponent<LineRenderer> ();
	}

	void Update()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			lines.positionCount = 0;
			pathNodes = new List<Vector3> ();
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			// Debug.Log (Input.GetTouch (0).deltaPosition);
			// Get movement of the finger since last frame
			Vector2 position = Input.GetTouch(0).position;

			pathNodes.Add(new Vector3(position.x, position.y, 10));
			lines.positionCount = pathNodes.Count;
			lines.SetPositions(pathNodes.ToArray());
		}
	}

}
