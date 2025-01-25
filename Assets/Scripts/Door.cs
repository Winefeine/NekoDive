using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsFinal;
    public GameObject Panel;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.GetComponent<PlayerController>().CanMove = false;
            
            Panel.SetActive(true);
            Panel.GetComponentInChildren<LevelSettlePanel>().UpdatePanelInfo();
            col.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
