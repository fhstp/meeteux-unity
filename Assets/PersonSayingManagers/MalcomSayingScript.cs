using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalcomSayingScript : AbstractPersonSayingManager {
    internal override void ExecuteActionForCurrentSaying(int SayingId)
    {
       // throw new NotImplementedException();
    }

    internal override bool StartTalking()
    {

        return UpdateCurrentSayings(LatestSaying);
       // throw new NotImplementedException();
    }

    internal override bool UpdateCurrentSayings(int SayingId)
    {
        switch (SayingId)
        {
            case -1:
                //already done by initialization
                return false;
                break;
            case 1:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 4);
                PersonCurrentSayings[1] = CreateSayingObject(2, 5);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 2:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 7);
                PersonCurrentSayings[1] = CreateSayingObject(2, 5);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 3:
                //no update
                return false;
                break;
            case 4:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 5);
                PersonCurrentSayings[1] = CreateSayingObject(2, 7);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 5:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 2);
                PersonCurrentSayings[1] = CreateSayingObject(2, 7);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 7:
                DestroyOldSayings();
                int fourOrFive = 4;
                if (SayingsDoneChecklist.ContainsKey(4) && SayingsDoneChecklist.ContainsKey(5))
                    fourOrFive = Math.Min(SayingsDoneChecklist[4], SayingsDoneChecklist[5]);
                else if (SayingsDoneChecklist.ContainsKey(4) && !SayingsDoneChecklist.ContainsKey(5))
                    fourOrFive = 5;
                PersonCurrentSayings[0] = CreateSayingObject(1, fourOrFive);
                PersonCurrentSayings[1] = CreateSayingObject(2, 8);
                PersonCurrentSayings[2] = CreateSayingObject(3, 12);
                break;
            case 8:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 9);
                PersonCurrentSayings[1] = CreateSayingObject(2, 10);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 9:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 11);
                PersonCurrentSayings[1] = CreateSayingObject(2, 10);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 10:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 11);
                PersonCurrentSayings[1] = CreateSayingObject(2, 9);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 11:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 9);
                PersonCurrentSayings[1] = CreateSayingObject(2, 10);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 12:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 13);
                PersonCurrentSayings[1] = CreateSayingObject(2, 14);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 13:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 14);
                PersonCurrentSayings[1] = CreateSayingObject(2, 7);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
            case 14:
                DestroyOldSayings();
                PersonCurrentSayings[0] = CreateSayingObject(1, 13);
                PersonCurrentSayings[1] = CreateSayingObject(2, 7);
                PersonCurrentSayings[2] = CreateSayingObject(3, 3);
                break;
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
