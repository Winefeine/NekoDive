using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSettlePanel : MonoBehaviour
{
    public bool IsFinal;
    public Button button;
    public TextMeshProUGUI CollectibleInfo;

    public GameObject C1;
    public GameObject C2;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsFinal)
        {
            button.onClick.AddListener(()=>
            {
                GameFlowManager.Instance.LoadNextScene();
            });
        }else
        {
            button.onClick.AddListener(()=>
            {
                Application.Quit();
            });
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanelInfo()
    {
        if(C1.activeSelf)
        {
            CollectibleInfo.text = "Collictable 1/2";
        }
        if(C2.activeSelf)
        {
            CollectibleInfo.text = "Collictable 2/2";
        }

    }


}
