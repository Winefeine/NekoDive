using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBorner : MonoBehaviour
{
    public GameObject BubblePrefab;
    public bool IsLoop;
    public int BurnCount;
    public float BurnInterval;
    public Transform BurnRoot;
    public float BurnRandom;
        
    public bool IsGenerating;

    float elaspedTime = 0f;
    

    void Update()
    {
        if(IsLoop)
        {
            if(elaspedTime >= BurnInterval)
            {
                GenerateBubble();
                elaspedTime = 0f;
            }
        }else
        {
            if(BurnCount > 0)
            {
                if(elaspedTime >= BurnInterval)
                {
                    GenerateBubble();
                    elaspedTime = 0f;
                    BurnCount--;
                }
            }
        }
        elaspedTime += Time.deltaTime;
    }



    public void GenerateBubble()
    {
        GameObject obj = GameObject.Instantiate(BubblePrefab);
        obj.transform.position = BurnRoot.position;
    }

}
