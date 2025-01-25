using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Oxygen : MonoBehaviour
{
    [Header("Parameters")]
    float curOxygen = 0f;
    public float CurOxygen {get{return curOxygen;} private set{curOxygen = value;}}
    public float OxygenTank = 100f;
    public float OxygenBreatheFrameCost = 0f;
    [HideInInspector]
    public float OxygenFrameCost = 0f;

    public Transform owner;
    [Header("UI")]
    public Image FillImage;
    public Image TransitionImage;
    public float LowOxygen;
    public Vector3 Offset;

    [Header("DOTween")]
    public float TransitionDuration = 0.5f;
    public Ease TransitionEase = Ease.OutQuad;
    
    float bubbleOffsetBuffer = 1.5f;
    PlayerController player;
    BubbleController bubble;
    PlayerInput input;
    bool isPlayer;
    bool isBubble;     
    float maxOxygen = 0f;


    void Start()
    {
        input = GameObject.FindObjectOfType<PlayerInput>();
        owner = transform.parent.parent;
        
        if(owner.TryGetComponent<PlayerController>(out PlayerController p))
        {
            isPlayer = true;
            player = p;
        }
        if(owner.TryGetComponent<BubbleController>(out BubbleController b))
        {
            isBubble = true;
            bubble = b;
            Offset *= bubbleOffsetBuffer;
        }
        TransitionImage = transform.GetChild(0).GetComponent<Image>();
        FillImage = transform.GetChild(1).GetComponent<Image>();

        InitOxygen();
    }


    void LateUpdate()
    {
        UpdateOxygen();
    }

    public void InitOxygen()
    {
        maxOxygen = OxygenTank;
        curOxygen = maxOxygen;

        transform.position = owner.position + Offset;
        TransitionImage.enabled = false;

        Debug.Log(owner);
        Debug.Log(transform.position);
        Debug.Log(Offset);
    }

    public void UpdateOxygen()
    {
        //default
        OxygenFrameCost += OxygenBreatheFrameCost;

        //Endup
        curOxygen -= OxygenFrameCost * Time.deltaTime;
        
        curOxygen = Math.Clamp(curOxygen,0f,OxygenTank);
        if(curOxygen == 0f)
        {
            if(isPlayer)
            {
                if(!player.IsDead)
                    player.Die();
            }else if(isBubble)
            {
                if(bubble != null)
                    bubble.BubbleDestroyed();
            }
        }

        //Update UI
        FillImage.fillAmount = curOxygen/maxOxygen; 

        OxygenFrameCost = 0f;
    }

    public void CostOxygen(float amount)
    {
        float lastOxygen = curOxygen;
        curOxygen -= amount;

        curOxygen = Math.Clamp(curOxygen,0f,OxygenTank);
        if(curOxygen <= 0f)
        {
            //Dead;
            if(isPlayer)
            {
                player.Die();
            }else if(isBubble)
            {
                bubble.BubbleDestroyed();
            }
        }

        //UI Animation
        TransitionImage.enabled = true;
        TransitionImage.fillAmount = FillImage.fillAmount;
        TransitionImage.DOFillAmount(curOxygen/maxOxygen,TransitionDuration)
            .From(lastOxygen/maxOxygen)
            .SetEase(TransitionEase)
            .OnComplete(() => {
                TransitionImage.enabled = false;
            });
    }


}
