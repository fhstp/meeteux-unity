using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheaSayingManager : AbstractPersonSayingManager {
    internal override void ExecuteActionForCurrentSaying(int SayingId)
    {
        StateManagement STM = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManagement>();
        if (SayingId == 100) { 
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    obj_Activity.CallStatic("takeItem", new object[] { "Brief" });
                }
            }
            STM.SetMoneyToRoommateDone(true);
        }
    }

    internal override bool StartTalking()
    {
        return UpdateCurrentSayings(LatestSaying);
    }

    internal override bool UpdateCurrentSayings(int SayingId)
    {
        StateManagement STM = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManagement>();
        DestroyOldSayings();
        if (!STM.GetMoneyToRoommateDone())
        {

            Dictionary<int, string> DynamicSayings1 = new Dictionary<int, string>() { { 1, "Ich erinnere mich gerade einfach nicht..." } };
            if (SayingId == 1 && SayingsDoneChecklist.ContainsKey(SayingId))
            {
                DynamicSayings1 = new Dictionary<int, string>();
                switch (SayingsDoneChecklist[SayingId])
                {
                    case 1:
                        DynamicSayings1.Add(1, "NOCH IMMER nicht.");
                        DynamicSayings1.Add(2, "Sorry.. aber mein Gedächtnis ist so schlecht.");

                        break;
                    case 2:
                        DynamicSayings1.Add(1, "Auch beim 3. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Ich erinnere mich gerade einfach so schlecht.");
                        break;
                    case 3:
                        DynamicSayings1.Add(1, "Auch beim 4. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Ich kann mich immer noch nicht erinnern...");
                        break;
                    case 4:
                        DynamicSayings1.Add(1, "Auch beim 5. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Wenn es doch nur etwas gäbe, das mir beim Erinnern hilft.");
                        break;
                    case 5:
                        DynamicSayings1.Add(1, "Auch beim 6. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Wenn es doch nur etwas gäbe, das mir beim Erinnern hilft.");
                        break;
                    case 6:
                        DynamicSayings1.Add(1, "Auch beim 7. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Wenn es doch nur etwas gäbe, das mir beim Erinnern hilft.");
                        break;
                    case 7:
                        DynamicSayings1.Add(1, "Auch beim 8. Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Wenn es doch nur etwas gäbe, das mir beim Erinnern hilft.");
                        break;
                    case 8:
                        DynamicSayings1.Add(1, "MEINE GÜTE! So gib mir doch etwas, das meinem Gedächtnis auf die Sprünge hilft!");
                        break;
                    case 9:
                        DynamicSayings1.Add(1, "MEINE GÜTE! So gib mir doch etwas, das meinem Gedächtnis auf die Sprünge hilft!");
                        break;
                    case 10:
                        DynamicSayings1.Add(1, "MEINE GÜTE! So gib mir doch etwas, das meinem Gedächtnis auf die Sprünge hilft!");
                        break;
                    case 11:
                        DynamicSayings1.Add(1, "Noch nie einen Film gesehen!?! Ein paar grüne Scheine helfen oft bei Erinnerungsproblemen.");
                        break;
                    default:
                        DynamicSayings1.Add(1, "Auch beim " + (SayingsDoneChecklist[SayingId] + 1).ToString() + ". Mal nachfragen nicht.");
                        DynamicSayings1.Add(2, "Wenn es doch nur etwas gäbe, das mir beim Erinnern hilft.");
                        break;
                }
            }
                //new Dictionary<int, string>() { { 1, "hi" }, };

            PersonCurrentSayings[0] = CreateSayingObject(1, 1, DynamicSayings1);
            PersonCurrentSayings[1] = CreateSayingObject(2, 2);
            PersonCurrentSayings[2] = CreateSayingObject(3, 3);
        }
        else
        {
            PersonCurrentSayings[0] = CreateSayingObject(1, 4);
            PersonCurrentSayings[1] = CreateSayingObject(2, 5);
            PersonCurrentSayings[2] = CreateSayingObject(3, 3);
        }

        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
