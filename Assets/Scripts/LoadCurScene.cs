using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCurScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(GameFlowManager.Instance.CurScene);
    }
}
