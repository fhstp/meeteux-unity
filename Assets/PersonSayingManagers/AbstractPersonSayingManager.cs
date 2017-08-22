using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public abstract class AbstractPersonSayingManager : MonoBehaviour {

    public GameObject PersonModel;
	//ACHTUNG: SETZEN DER SMC UND DES NAMENS VERMUTLICH DOCH AM BESTEN MANUELL... ODER TAG HINZUFÜGEN UND DOCH VOM SMC BEI INITIALISIERUNG VERSUCHEN
	protected GameObject[] PersonCurrentSayings = new GameObject[3] {null, null, null};
	protected SayingManagement SayingManagementComponent = null;
	public string PersonName;

    internal abstract bool UpdateCurrentSayings (int SayingId);
    internal abstract bool StartTalking ();
    internal abstract void ExecuteActionForCurrentSaying (int SayingId);

	protected bool HasBeenInitialized = false;
	protected int LatestSaying = -1;
    protected Dictionary<int, int> SayingsDoneChecklist = new Dictionary<int, int>();
    //bool IsActive = false;


    internal bool UpdateCurrentSayingsForNext(int SayingId)
    {
        
        return UpdateCurrentSayings(SayingId);
    }



    internal bool GetHasBeenInitialized() {
		return HasBeenInitialized;
	}

    internal void SetHasBeenInitialized() {
		HasBeenInitialized = true;
	}

    internal void Initialize(SayingManagement SMC) {
		if (HasBeenInitialized) {
			return;
		}
        SetSayingManagement(SMC);
		SetPersonName (gameObject.name);
        SetHasBeenInitialized();
        for (int i = 0; i < PersonCurrentSayings.Length; i++) {
            PersonCurrentSayings[i] = CreateSayingObject(i+1, i+1);
        }
    }
    internal void SetPersonName(string name) {
		PersonName = name;
	}

    internal void SetSayingManagement(SayingManagement SMC) {
		SayingManagementComponent = SMC;
	}

    internal void SayingObjectChosen(int SayingId) {
        LatestSaying = SayingId;
        if (SayingsDoneChecklist.ContainsKey(SayingId)) {
            SayingsDoneChecklist[SayingId] += 1;
        }
        else {
            SayingsDoneChecklist.Add(SayingId, 1);
        }
        ExecuteActionForCurrentSaying (SayingId);
	}


    protected void DestroyOldSayings()
    {
        for (int i = 0; i < PersonCurrentSayings.Length; i++)
        {
            Destroy(PersonCurrentSayings[i]);
        }
    }
	protected GameObject CreateSayingObject (int Position, int SayingId, Dictionary<int, string> DynamicSayings = null) { //(int Position, string SayingStringShort, string SayingStringLong, int SayingId) {
                                                                                                                          /*if (Position == null) {
                                                                                                                              return null;
                                                                                                                          }*/
        
		string SayingStringShort = SayingManagementComponent.GetShortText(SayingId, this);
		string[] SayingStringLong = SayingManagementComponent.GetLongText(SayingId, this);

        if (DynamicSayings != null)
        {
            int DictionaryLoopCountUp = 1;
            foreach (KeyValuePair<int, string> singleEntry in DynamicSayings)
            {
                for (int i = 0; i < SayingStringLong.Length; i++)
                {
                    bool lastDynamicSaying = (DynamicSayings.Count == DictionaryLoopCountUp);
                    SayingStringLong[i] = ExecuteSayingDynamic(SayingStringLong[i], singleEntry.Key, singleEntry.Value, lastDynamicSaying);
                }
                DictionaryLoopCountUp++;
            }
        }
         GameObject NewSayingObject = (GameObject)(Instantiate(Resources.Load("Prefabs/Sayings/Saying" + Position.ToString()), SayingManagementComponent.SayingPanel.transform));
         NewSayingObject.SetActive(false);
         //GameObject NewSayingObject = (GameObject)(Instantiate(Resources.Load("Saying" + Position.ToString())));

         Text SayingText = NewSayingObject.GetComponent<Text>();
		SayingText.text = SayingStringShort;
		SayingScript SayingComponent = NewSayingObject.GetComponent<SayingScript> ();
		SayingComponent.SayingId = SayingId;
		SayingComponent.DialogueStrings = SayingStringLong;

		return NewSayingObject;
	}

    protected string ExecuteSayingDynamic(string saying, int index, string dynamicString, bool removeLeftSayingDynamics = false)
    {
        saying = saying.Replace("{" + index.ToString() + "}", dynamicString);
        if (removeLeftSayingDynamics)
        {
            String matchpattern = @"{[1-9]+}";
            String replacementpattern = @"";
            saying = System.Text.RegularExpressions.Regex.Replace(saying, matchpattern, replacementpattern);
        }
        return saying;
    }

    internal GameObject[] GetCurrentSayings () {
		return PersonCurrentSayings;
	}
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
     
		
	}
}
