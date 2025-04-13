using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ElementAnimate;


public class Animation_On_Click : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] public List<ElementAnimate> elementAnim;

    float CurrentTime = 0;
    public List<ElementAnimate> Element { get => elementAnim; set => elementAnim = value; }
    bool isStart = false;
    private void Start()
    {
        foreach(var anim in Element)
        {
            anim.OnSetDefaultValue();
        }
    }

    public void Update()
    {
        if(isStart)
        {
            CurrentTime += Time.deltaTime;
        }
    }
    public void OnStartAnim()
    {
        StartCoroutine(nameof(OnAimationFadeAndPose));
    }

    private void OnAnimateOut()
    {
        foreach (var element in Element) 
        {
            element.startPose = element.objectUI.anchoredPosition;
            element.startPose = Vector2.LerpUnclamped(element.startPose, element.endPose, element.moveEffect.Evaluate(CurrentTime) * element.Duration);
            Debug.Log("Animation Out");
        }
    }

    private void OnAnimateIn()
    {

    }

    private void OnFadeIn()
    {

    }
    private void OnFadeOut()
    {

    }

    IEnumerator OnAimationFadeAndPose()
    {
        foreach (var element in Element) 
        {
            isStart = true;
            while (element.objectUI.anchoredPosition != element.endPose)
            {
                switch (element.SetAnim)
                {
                    case AnimationSet.AnimationOut:
                        OnAnimateOut();
                        break;
                    case AnimationSet.AnimationIn:
                        OnAnimateIn();
                        Debug.Log("Done Animation In");
                        break;
                    case AnimationSet.FadeOut:
                        OnFadeOut();
                        Debug.Log("Animation FadeOut");
                        break;
                    case AnimationSet.FadeIn:
                        OnFadeIn();
                        Debug.Log("Animation FadeIn");
                        break;
                }
            }
            if(element.objectUI.anchoredPosition == element.endPose)
            {
                yield return null;
            }
        }
    }
}

[System.Serializable]
public class ElementAnimate
{
    public string name;
    public CanvasGroup canvasAlpha;
    public AnimationCurve moveEffect;
    public AnimationCurve FadeEffect;
    public RectTransform objectUI;
    //For Private

    public float Duration = 0.5f;
    [HideInInspector]
    public Vector2 startPose;
    public Vector2 endPose;
    [HideInInspector]
    public float CurrentTime;
    [HideInInspector]
    public float OriginalAlpha;
    public enum AnimationSet
    {
        AnimationIn,
        AnimationOut,
        FadeIn,
        FadeOut
    }

    public AnimationSet SetAnim;
    public void OnSetDefaultValue()
    {
        startPose = objectUI.anchoredPosition;
        OriginalAlpha = canvasAlpha.alpha;
        //contiune
    }
}