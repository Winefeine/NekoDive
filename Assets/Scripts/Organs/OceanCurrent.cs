using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCurrent : MonoBehaviour
{
    public float Force = 5f; //-weight
    public float CurrentVacancy = 10f;
    public Vector3 CurrentDirection = Vector3.right;



    public Vector3 GetVacancy()
    {
        Vector3 Current = Vector3.zero;
        Current = CurrentDirection * CurrentVacancy;
        return Current;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if(Force >= player.Weight)
            {
                player.AddCurrent(GetVacancy());
                Debug.Log("In" + col.gameObject);
            }
        }
        if(col.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            BubbleController bubble = col.GetComponent<BubbleController>();
            if(Force >= bubble.Weight)
            {
                bubble.AddCurrent(GetVacancy());
                Debug.Log("In" + col.gameObject.name);
            }
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if(Force >= player.Weight)
            {
                player.RemoveCurrent(GetVacancy());
            }
        }
        if(col.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            BubbleController bubble = col.GetComponent<BubbleController>();
            if(Force >= bubble.Weight)
            {
                bubble.RemoveCurrent(GetVacancy());
            }
        }

    }

}
