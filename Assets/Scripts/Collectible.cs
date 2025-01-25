
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject C1;
    public GameObject C2;
    

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!C1.activeSelf)
            {
                C1.SetActive(true);
            }else
            {
                C2.SetActive(true);
            }

            Destroy(this.gameObject);
        }
    }

}
