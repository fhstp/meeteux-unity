using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SayingManagement : MonoBehaviour {
	private Dictionary<string, Dictionary<int, string[]>> SayingConfig = new Dictionary<string, Dictionary<int, string[]>>();
	public AbstractPersonSayingManager CurrentPersonSayingManager = null;
	public GameObject DisplaySaying;
	public GameObject SayingPanel;
    protected StateManagement StateManager = null;

    private bool talkingActive = false;
    private bool talkingJumpPossible = false;

    private GameObject DisplayIcons;
    private int TempSayingId = -1;
    private string[] TempSayingDialogueStrings = new string[1] { "" };
	private bool TempBlocked = false;
	private GameObject[] Sayings = new GameObject[3]{null, null, null};
    private int DialogPartsDone = 0;

    internal void StartSayingManagement (GameObject TalkingPerson) {
        DialogPartsDone = 0;
        TempSayingId = -1;
        TempSayingDialogueStrings = new string[1] { "" };
        TempBlocked = false;
        talkingActive = false;
        talkingJumpPossible = false;
        StateManager.SetFreeForNextSaying(true);

        Object[] PersonComponents = TalkingPerson.GetComponents<AbstractPersonSayingManager>();
        for (int j = 0; j < PersonComponents.Length; j++)
        {
            System.Type dbgVar = PersonComponents[j].GetType();
           // if (PersonComponents[j].GetType().IsAssignableFrom(typeof(AbstractPersonSayingManager)))
            {
                CurrentPersonSayingManager = ((AbstractPersonSayingManager)PersonComponents[j]);
                CurrentPersonSayingManager.PersonModel.GetComponent<Animator>().SetBool("isTalking", false);

                bool sayingsUpdated = CurrentPersonSayingManager.StartTalking();

                /*if (sayingsUpdated)
                {
                    for (int i = 0; i < Sayings.Length; i++)
                    {
                        if (Sayings[i] != null)
                        {
                            Destroy(Sayings[i]);
                        }
                    }


                }*/
                //else
                { 
                    for (int i = 0; i < Sayings.Length; i++)
                    {
                        if (Sayings[i] != null)
                        {
                            //Sayings[i].transform.parent = SayingPanel.transform;
                            Sayings[i].SetActive(false);
                        }
                    }
                }


                Sayings = CurrentPersonSayingManager.GetCurrentSayings();


                for (int i = 0; i < Sayings.Length; i++)
                {
                    if (Sayings[i] != null)
                    {
                        //Sayings[i].transform.parent = SayingPanel.transform;
                        Sayings[i].SetActive(true);
                    }
                }
                /* MethodInfo method = PersonComponents[j].GetType().GetMethod("Initialize", new Type[] { });
                //MethodInfo genericMethod = method.MakeGenericMethod(PersonComponents[j].GetType());
                method.Invoke(PersonComponents[j], new Object[1] { this }); */
            }
        }
      



        DisplaySaying.SetActive(false);
        SayingPanel.SetActive (true);
	}

	internal void SomethingSaid (string[] DialogueStrings, int SayingId) {
		if (TempBlocked || CurrentPersonSayingManager == null) {
			return;
		}
        TempBlocked = true;
        talkingActive = true;
        talkingJumpPossible = false;
        TempSayingId = SayingId;
        TempSayingDialogueStrings = DialogueStrings;
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
		DisplayText.text = "<color=#264b8b>" + DialogueStrings[0] + "</color>";
        DialogPartsDone++;
        SayingPanel.SetActive (false);
		DisplaySaying.SetActive (true);
        string[] words = DialogueStrings[0].Split(' ');
        float dialogueDuration = 1f;
        for (int i = 0; i < words.Length; i++) {
            dialogueDuration += 0.39f;
            if (words[i].Length > 6)
            {
                dialogueDuration += 0.06f;
            }
        }
        Invoke("MakeJumpPossible", 0.61f);
        Invoke("DoNextDialoguePart", dialogueDuration);
    }

    internal void DoNextDialoguePart () {
        if (DialogPartsDone == TempSayingDialogueStrings.Length) {
            SayingDone();
            return;
        }

        // talkingJumpPossible = true;
        string sayColor = "<color=#264b8b>";
        if (DialogPartsDone%2 != 0)
        {
            CurrentPersonSayingManager.PersonModel.GetComponent<Animator>().SetBool("isTalking", true);
            sayColor = "<color=#680368>";
        }
        else
        {
            CurrentPersonSayingManager.PersonModel.GetComponent<Animator>().SetBool("isTalking", false);
        }
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
        DisplayText.text = sayColor + TempSayingDialogueStrings[DialogPartsDone] + "</color>";
        DialogPartsDone++;
        string[] words = TempSayingDialogueStrings[DialogPartsDone-1].Split(' ');
        float dialogueDuration = 0.7f;
        for (int i = 0; i < words.Length; i++)
        {
            dialogueDuration += 0.29f;
            if (words[i].Length > 6)
            {
                dialogueDuration += 0.05f;
            }
        }
        
        Invoke("DoNextDialoguePart", dialogueDuration);
    }

    internal void CancelInvokesAndResetEverything() {
        
        CancelInvoke("MakeJumpPossible");
        CancelInvoke("FreeForNextSaying");

        if (CurrentPersonSayingManager == null) return;
        CurrentPersonSayingManager.PersonModel.GetComponent<Animator>().SetBool("isTalking", false);
        DialogPartsDone = 0;
        TempSayingId = -1;
        TempSayingDialogueStrings = new string[1] { "" };
        CancelInvoke("DoNextDialoguePart");
		Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
		DisplayText.text = "";
        talkingActive = false;
        talkingJumpPossible = false;
        //DisplaySaying.SetActive (false);
        FreeForNextSaying();
        //SayingPanel.SetActive (false);
        CurrentPersonSayingManager = null;
        TempBlocked = false;
	}

    internal void GoToNextDialoguePart()
    {
      // CancelInvoke("MakeJumpPossible");
        CancelInvoke("DoNextDialoguePart");
        DoNextDialoguePart();
    }

    void MakeJumpPossible()
    {
        talkingJumpPossible = true;
    }

    public void FreeForNextSaying()
    {
        StateManager.SetFreeForNextSaying(true);
    }

    void SayingDone() {
        CurrentPersonSayingManager.PersonModel.GetComponent<Animator>().SetBool("isTalking", false);
        CancelInvoke("MakeJumpPossible");

        CurrentPersonSayingManager.SayingObjectChosen (TempSayingId);
        bool sayingsUpdated = CurrentPersonSayingManager.UpdateCurrentSayingsForNext(TempSayingId);
        Invoke("FreeForNextSaying", 0.61f);
        Text DisplayText = DisplaySaying.transform.GetChild(0).GetComponent<Text>();
		DisplayText.text = "";
		DisplaySaying.SetActive (false);

        //if (sayingsUpdated)
        {
            for (int i = 0; i < Sayings.Length; i++)
            {
                if (Sayings[i] != null)
                {
                    Sayings[i].SetActive(false);
                }
            }
        }

        Sayings = CurrentPersonSayingManager.GetCurrentSayings();


        for (int i = 0; i < Sayings.Length; i++)
        {
            if (Sayings[i] != null)
            {
                //Sayings[i].transform.parent = SayingPanel.transform;
                Sayings[i].SetActive(true);
            }
        }



        // CurrentPersonSayingManager = null;
        talkingActive = false;
        talkingJumpPossible = false;
        TempSayingId = -1;
        TempSayingDialogueStrings = new string[1] {""};
        TempBlocked = false;
        DialogPartsDone = 0;
        SayingPanel.SetActive (true);
	}



    internal string GetShortText(int SayingId, AbstractPersonSayingManager SayingManager)
    {
        if (SayingManager == null)
            return "";
        string[] RetString = new string[2] { "", "" };
        string PersonString = SayingManager.PersonName;
        Dictionary<int, string[]> PersonSayings = null;
        SayingConfig.TryGetValue(PersonString, out PersonSayings);
        if (PersonSayings == null)
            return "";
        PersonSayings.TryGetValue(SayingId, out RetString);
        return "<color=#264b8b>- " + RetString[0] + "</color>";
    }


    internal string[] GetLongText(int SayingId, AbstractPersonSayingManager SayingManager)
    {
        if (SayingManager == null)
            return new string[1] { "" };
        string[] RetString = new string[2] { "", "" };
        string PersonString = SayingManager.PersonName;
        Dictionary<int, string[]> PersonSayings = null;
        SayingConfig.TryGetValue(PersonString, out PersonSayings);
        if (PersonSayings == null)
            return new string[1] { "" };
        PersonSayings.TryGetValue(SayingId, out RetString);
        string[] RealRetString = new string[RetString.Length - 1];
        for (var i = 1; i < RetString.Length; i++)
        {
            RealRetString[i - 1] = RetString[i];
        }
        return RealRetString;
    }

    // Use this for initialization
    void Start() {
        StateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManagement>();


        //TODO later on: Parse an XML for the sayings here.
        //int starts at 0...

        //... for each person;
        Dictionary<int, string[]> BoyfriendSayings = new Dictionary<int, string[]>();
        Dictionary<int, string[]> RoommateSayings = new Dictionary<int, string[]>();

        //BOYFRIEND SAYINGS:
        BoyfriendSayings.Add(1, new string[] { "Ähm... Hallo..?", "Ähm... Hallo..?", "Hallo... Es tut gut dich zu sehen.", "Du.. kennst mich?", "Nein - aber ich kann spüren, dass du jenseits dessen bist, wo ich mich jetzt befinde."} );
        BoyfriendSayings.Add(2, new string[] { "Kanntest du Tatyana?", "Kanntest du Tatyana?", "Ob ich Tatyana kannte? Tatyana, war für mich der wichtigste Mensch überhaupt. Wir waren haben uns an meinem Geburstag ineinander verliebt.." });
        BoyfriendSayings.Add(3, new string[] { "Hast du ein Taschentuch?", "Entschuldigung. Hättest du ein Taschentuch für mich?", "Nein." });
        BoyfriendSayings.Add(4, new string[] { "Weißt du denn, wo du jetzt genau bist?", "Hast du denn eine Ahnung, wo du jetzt genau bist?", "Tatyana verschwand. Wenige Wochen später verfinsterte sich der Himmel. Und dann glitten wir alle langsam... hierher....", "Tatyana wurde ermordet.", "Ja. Das weiß ich noch. An jenem Tag gab es diese Ritualmorde rund um den Globus. Es waren um die 700.", "Und dann?", "Dann, wenig später, begann es. Zuerst stürmte es, dann setzte der Regen ein. Überall. Buchstäblich.", "Die Menschen begannen zu verschwinden, nicht wahr?", "Die Welt schien total aus den Fugen zu geraten. Stündlich wurde es schlimmer..", "Als wäre der Tag des jüngsten Gerichts angebrochen.", "Ja.. und dann.. Irgendwann.. begannen sich die Menschen und die Welt selbst in Luft aufzulösen. Mit jedem Tag mehr."});
        BoyfriendSayings.Add(5, new string[] { "Die gute Nachricht ist: Du bist nicht tot...", "Die gute Nachtricht ist: Du bist nicht tot... Ähm... Glaube ich zumindest. Naja.. wenn ich ehrlich bin - GANZ sicher weiß ich es eigentlich nicht.", "Ich glaube auch nicht, dass ich tot bin.... Aber es fühlt sich hier so an, als würde keine einzige Minute mehr verstreichen.", "So als würde man ein Helene Fischer Lied in Dauerschleife hören?", "Fast so schlimm. Es ist so, als wäre die ganze Welt plötzich an einen Ort verschwunden, wo Zeit einfach nicht mehr existiert.", "Denkst du, die Ritualmorde haben etwas damit zu tun?", "Ehrlich gesagt: Ja. Du weißt - eines der Opfer war Tatyana.... Wennn ich nur daran denke, was die mit ihr gemacht haben..." });
        BoyfriendSayings.Add(7, new string[] { "Trauerst du denn noch um Tatyana, dort wo du nun bist ...?", "Trauerst du denn noch um Tatyana? Oder ist das dort, wo du jetzt bist nicht mehr relevant für dich?", "Natürlich! Was für eine blöde Frage. Ich habe Tayana geliebt. Das wird sich auch in der Ewigkeit nicht ändern." });
        BoyfriendSayings.Add(8, new string[] { "Sorry, nein - ich kaufe dir nicht ab, dass du um Tatyana trauerst.", "Sorry, nein - ich kaufe dir nicht ab, dass du um Tatyana trauerst. Sie hat dir nichts bedeutet, denke ich! ", "Bitte was? Das nimmst du zurück.", "Ähm.. Nein. Eigentlich nicht.", "Du */$(§( einer (§%${&$ , die ^%&/§ zum &$%!!§(} isst. Mach, dass du verschwindest." });
        BoyfriendSayings.Add(9, new string[] { "Entschuldige. Ich bin wohl zu weit gegangen.", "Entschuldige. Ich bin wohl zu weit gegangen. Wahrscheinlich hat dir Tatyana doch etwas bedeutet.", "WAHRSCHEINLICH? DU &$&%?! Du hast ja keine Ahnung. Hau ab!" });
        BoyfriendSayings.Add(10, new string[] { "Denkst du, Tatyana hat dich denn geliebt?", "Denkst du, Tatyana hat dich denn geliebt? Vielleicht ja nicht...?", "Du &$(&$(/%$!(/=°?§/°)=§&°§)!//!$ !!" });
        BoyfriendSayings.Add(11, new string[] { "Es tut mir leid...", "Es tut mir leid... ich wollte da keinen Nerv treffen", ".... zieh einfach Leine.." });
        BoyfriendSayings.Add(12, new string[] { "Ihr habt euch also wirklich geliebt?", "Ihr habt euch also wirklich geliebt?", "Ja. Ich kann mich noch ganz genau erinnern. Das ist heute auf den Tag genau 6 Monaten her. Wir haben meinen Geburtstag gefeiert und..", "Ja, danke. Ich glaube, das war schon Info genug." });
        BoyfriendSayings.Add(13, new string[] { "Wie lange seid ihr nochmal zusammen gewesen?", "Wie lange seid ihr nochmal zusammen gewesen?", "Bei meiner Geburtstagsfeier sind wir zusammen gekommen. Heute wären das auf den Tag genau 6 Monate... Ich kann es immer noch nicht glauben, dass sie jetzt plötzlich tot ist." });
        BoyfriendSayings.Add(14, new string[] { "Du hattest also vor 6 Monaten Geburtstag?", "Du hattest also vor 6 Monaten Geburtstag?", "Ja. Das war auch der Tag, an dem wir uns ineinander verliebt haben." });

        //ROOOMATE SAYINGS:
        RoommateSayings.Add(1, new string[] { "Ähm... Hallo..?", "Ähm... Hallo..?", "Jaja... Hallo.", "Kannst du mir etwas über Tatya..", "Nein - Ich kann dir nichts über meine Mitbewohnerin Tatyana erzählen. {1} {2}" });
        //Nein - ich kann dir nichts über meine Mitbewohnerin Tatyana erzählen. Auch beim 10.Mal nachfragen nicht. Ich erinnere mich gerade einfach so schlecht.Immer noch.
        RoommateSayings.Add(2, new string[] { "Kanntest du Tatyana?", "Kanntest du Tatyana?", "Ja.", "Und..?", "Was und?", "Naja - kannst du mir etwas über Tatyana erzählen?", "Hm.. nein", "Warum nicht?", "Nun? Ich bin mir nicht sicher, ob ich dir das nicht schon gesagt habe: Aber ich erinnere mich kaum.", "Oh.. ja.. ich seh schon..", "Schade, oder? Ich hoffe immer noch, dass es irgendetwas gibt, das meiner Erinnerung auf die Sprünge hilft." });
        RoommateSayings.Add(3, new string[] { "Hast du ein Taschentuch?", "Entschuldigung. Hättest du ein Taschentuch?", "Ja. Ich habe Taschentücher.", "Oh.. könnte ich dann vielleicht eines ha...", "Nein." });
        RoommateSayings.Add(4, new string[] { "Ähm... Hallo..?", "Ähm... Hallo..?", "Ähm... Tschüß?."  });
        RoommateSayings.Add(5, new string[] { "Wegen dieses Briefs...", "Wegen dieses Briefs, den du mir gegeben hast...", "Ich werde ihn dir bestimmt nicht vorlesen.", "Oh..." });





        //TODO: After parsing XML, this should also happen automatically lateron.
        SayingConfig.Add("Boyfriend", BoyfriendSayings);
        SayingConfig.Add("Roommate", RoommateSayings);

        GameObject[] Persons = GameObject.FindGameObjectsWithTag("Person");
      
        for (int i = 0; i < Persons.Length; i++) {
            Object[] PersonComponents = Persons[i].GetComponents<AbstractPersonSayingManager>();
            for (int j = 0; j < PersonComponents.Length; j++) {
                //if (PersonComponents[j].GetType().IsAssignableFrom(typeof(AbstractPersonSayingManager))) 
                {
                    ((AbstractPersonSayingManager)PersonComponents[j]).Initialize(this);
                    // ANIMATION STARTS AUTOMATICALLY: ((AbstractPersonSayingManager)PersonComponents[j]).PersonModel.GetComponent<Animator>().SetTrigger("idle");

                    /* MethodInfo method = PersonComponents[j].GetType().GetMethod("Initialize", new Type[] { });
                    //MethodInfo genericMethod = method.MakeGenericMethod(PersonComponents[j].GetType());
                    method.Invoke(PersonComponents[j], new Object[1] { this }); */
                }
            }
        }
    }



	// Update is called once per frame
	void Update () {
	    if (talkingActive && Input.touchCount > 0 && talkingJumpPossible)
        {
            talkingJumpPossible = false;
            Invoke("MakeJumpPossible", 0.61f);
            GoToNextDialoguePart();
        }
	}
}
