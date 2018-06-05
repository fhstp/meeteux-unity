using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class SocketIOClient : MonoBehaviour 
{
	public string serverURL = "";
	protected Socket socket = null;
	public Text connectionText;
	private string status;

	private byte[] trashData;
	private byte[] bigTrashData;
	private int dataSize;

	public Slider dataSlider;
	public Text dataText;
	public Toggle bigDataToggle;

	// Use this for initialization
	void Start () 
	{
		loadAndOpenConnection ();
		dataSlider.onValueChanged.AddListener (updateTrashData);
		bigDataToggle.onValueChanged.AddListener (invokeSendBigData);

		this.bigTrashData = new byte[1024 * 1024 * 2];
	}

	void Update ()
	{
		this.connectionText.text = this.status;
	}

	void Destroy () 
	{
		closeConnection ();
	}

	void openConnection()
	{
		if (socket != null)
			return;

		this.status = "Trying to connect to " + serverURL;
		socket = IO.Socket (serverURL);

		socket.On ("connected", (data) => {
			// Debug.Log(data);
			this.status = "Connected";
		});

		socket.On ("reconnecting", () => {
			this.status = "Trying to reconnect";
		});

		socket.On ("reconnect_error", () => {
			this.status = "Failed to reconnect";
		});

		socket.On ("connect_error", () => {
			this.status = "Connection lost";
		});



	}

	void closeConnection()
	{
		if (socket != null) 
		{
			socket.Disconnect ();
			socket = null;
		}
	}

	public void transmitDrawingData(string id, Vector3 node, string color)
	{
		Drawing data = new Drawing ();
		data.pathNode = node;
		data.id = id;
		data.color = color;

		float frameRate = 1.0f / Time.deltaTime;

		dataText.text = "Sending: " + this.dataSlider.value * frameRate + "KB/s";

		data.trash = this.trashData;
		string stringData = JsonUtility.ToJson (data);
		// Debug.Log (stringData);
		socket.Emit ("transmitDrawingData", stringData);
	}

	public void loadAndOpenConnection()
	{
		string filePath = Application.streamingAssetsPath + "/Drawing/serverConfig.json";
		string jsonString;

		if(Application.platform == RuntimePlatform.Android) //Need to extract file from apk first
		{
			WWW reader = new WWW(filePath);
			while (!reader.isDone) { }

			jsonString= reader.text;
		}
		else
		{
			jsonString= File.ReadAllText(filePath);
		}

		ServerConfiguration config = JsonUtility.FromJson<ServerConfiguration>(jsonString);
		this.serverURL = config.serverUrl;

		// Debug.Log (config.serverUrl);

		openConnection ();
	}

	void updateTrashData(float value)
	{
		this.dataSize = (int)(1024 * value);
		float frameRate = 1.0f / Time.deltaTime;

		this.trashData = new byte[dataSize];
	}

	void invokeSendBigData(bool active)
	{
		if (active) {
			InvokeRepeating("sendBigData", 2, 10);	
		} else {
			CancelInvoke();
		}
	}

	void sendBigData()
	{
		Debug.Log ("send big data");
		BigData bd = new BigData ();
		bd.trash = this.bigTrashData;
		bd.timestamp = System.DateTime.Now;
		socket.Emit ("transmitBigData", JsonUtility.ToJson(bd));
	}
}
