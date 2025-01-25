using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button StartButton;
    public int NextScene;


    void Start()
    {
        NextScene = GameFlowManager.Instance.CurScene += 1;
        StartButton.onClick.AddListener(()=>
        {
            GameFlowManager.Instance.CurScene = 2;
            SceneManager.LoadScene(NextScene);
            //SwitchToNextScene();
        });

    }

    void SwitchToNextScene()
    {
        GameFlowManager.Instance.LoadNextScene();
    }

}
