using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMantainer : MonoBehaviour
{
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

    }

    void Update()
    {
        if(transform.parent != null)
        {
            Maintain();
        }else
        {
            if(transform.localScale != originalScale)
            {
                transform.localScale = originalScale;
            }
        }
    }

    public void Maintain()
    {   
        
        Vector3 parentScale = transform.parent.lossyScale; // 父对象的全局Scale
            transform.localScale = new Vector3(
                originalScale.x / parentScale.x,
                originalScale.y / parentScale.y,
                originalScale.z / parentScale.z
            );
    }
    
}
