using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ElementAnimate;


public class Animation_On_Click : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] public List<ElementAnimate> elementAnim;
    public List<ElementAnimate> Element { get => elementAnim; set => elementAnim = value; }
    private void Start()
    {
        foreach (var anim in Element)
        {
            anim.OnSetDefaultValue();
        }
    }
    public void OnStartAnim()
    {
        StartCoroutine(nameof(OnAimationFadeAndPose));
    }
    private IEnumerator OnAnimateOut()
    {
        foreach (var element in Element)
        {
            float CurrentTime = 0f;
            while (CurrentTime < element.Duration)
            {
                CurrentTime += Time.deltaTime;
                float normalizedTime = CurrentTime / element.Duration;
                float CurveT = element.moveEffect.Evaluate(normalizedTime);

                element.objectUI.anchoredPosition = Vector2.LerpUnclamped(element.startPose, element.endPose, CurveT);
                Debug.Log($"Time: {CurrentTime}, CurvedT: {CurveT}, Pos: {element.objectUI.anchoredPosition}");
                yield return null;
            }
        }
    }

    private IEnumerator OnAnimateIn()
    {
        float t_Time = 1f;
        foreach (var element in Element) 
        {
            while(t_Time < element.Duration)
            {
                t_Time -= Time.deltaTime;
                float NormalizedTime = t_Time / element.Duration;
                float CurveT = element.moveEffect.Evaluate(NormalizedTime);

                element.objectUI.anchoredPosition = Vector2.LerpUnclamped(element.startPose, element.endPose, CurveT);
                Debug.Log($"Time: {t_Time}, CurvedT: {CurveT}, Pos: {element.objectUI.anchoredPosition}");
                yield return null;
            }

        }
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
            switch (element.SetAnim)
            {
                case AnimationSet.AnimationOut:
                    StartCoroutine(OnAnimateOut());
                    break;
                case AnimationSet.AnimationIn:
                    StartCoroutine(OnAnimateIn());
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
        yield return null;
        Debug.Log("Exit in coroutine");
        yield return null;
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