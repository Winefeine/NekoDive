using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.TryGetComponent<BubbleController>(out BubbleController b))
        {
            b.BubbleDestroyed();
        }
        if(col.gameObject.TryGetComponent<PlayerController>(out PlayerController p))
        {
            p.Die();

        }
        if(col.gameObject.TryGetComponent<PlayerFeet>(out PlayerFeet f))
        {
            PlayerController player = f.transform.parent.GetComponent<PlayerController>();
            player.Die();
        }
    }

}
