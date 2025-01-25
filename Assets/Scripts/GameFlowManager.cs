using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    public int CurScene = 0;

    public int ReloadScene = 1;
    public int CollectionCount = 0;
    public GameObject GameClearCanvas;
    public GameObject GameOverCanvas;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {


    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }

    }

    public void Clear()
    {
        GameObject obj = GameObject.Instantiate(GameClearCanvas);
    }

    public void Over()
    {
        GameObject obj = GameObject.Instantiate(GameOverCanvas);
        
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(ReloadScene);

    }

    public void LoadNextScene()
    {
        CurScene++;
        SceneManager.LoadScene(CurScene);
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
        CurScene = scene;
        
    }

    public void UpdateCollection()
    {
        


    }


}
