using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ElementAnimate;

public class Animation_On_Click : MonoBehaviour
{
    [Header("Одиночные анимации (не обязательны)")]
    [SerializeField] public List<ElementAnimate> elementAnim;

    [Header("Группы анимаций")]
    [SerializeField] private List<AnimationGroup> groups;

    public Panel_Ui_State state;

    private void Start()
    {
        foreach (var anim in elementAnim)
        {
            anim.OnSetDefaultValue();
        }

        foreach (var group in groups)
        {
            foreach (var element in group.elements)
            {
                element.OnSetDefaultValue();
            }
        }
    }

    public void OnStartAnimationGroup(string groupName)
    {
        var group = groups.Find(g => g.name == groupName);
        if (group != null)
        {
            StartCoroutine(PlayAnimationGroupParallel(group.elements));
        }
        else
        {
            Debug.LogWarning($"Group '{groupName}' not found.");
        }
    }

    public void OnStartAnimationNoParallel()
    {
        StartCoroutine(PlayAnimationsWithoutParallel());
    }

    private IEnumerator PlayAnimationsWithoutParallel()
    {
        foreach(var anim in elementAnim)
        {
            float t_time = 0f;
            while(t_time < anim.Duration)
            {
                t_time += Time.deltaTime / anim.Duration;
                PlaySingleElement(anim);
                yield return anim;
            }
        }
    }

    private IEnumerator PlayAnimationGroupParallel(List<ElementAnimate> elements)
    {
        List<Coroutine> coroutines = new List<Coroutine>();

        foreach (var element in elements)
        {
            coroutines.Add(StartCoroutine(PlaySingleElement(element)));
        }

        foreach (var c in coroutines)
        {
            yield return c;
        }

        Debug.Log("✅ All animations in group completed.");
        elements.Clear();
        state.OnSetActivePanel();
    }

    private IEnumerator PlaySingleElement(ElementAnimate element)
    {
        switch (element.SetAnim)
        {
            case AnimationSet.AnimationOut:
                yield return AnimateOut(element);
                break;
            case AnimationSet.AnimationIn:
                yield return AnimateIn(element);
                break;
            case AnimationSet.FadeOut:
                OnFadeOut(element);
                break;
            case AnimationSet.FadeIn:
                OnFadeIn(element);
                break;
        }
    }

    private IEnumerator AnimateOut(ElementAnimate element)
    {
        float currentTime = 0f;
        while (currentTime < element.Duration)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / element.Duration);
            float curveT = element.moveEffect.Evaluate(t);

            if (element.objectUI != null)
            {
                element.objectUI.anchoredPosition = Vector2.LerpUnclamped(element.startPose, element.endPose, curveT);
            }

            yield return null;
        }

        if (element.objectUI != null)
        {
            
            element.objectUI.anchoredPosition = element.endPose;
        }
    }

    private IEnumerator AnimateIn(ElementAnimate element)
    {
        float currentTime = 1f;
        while (currentTime < element.Duration)
        {
            currentTime -= Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / element.Duration);
            float curveT = element.moveEffect.Evaluate(t);

            if (element.objectUI != null)
            {
                element.objectUI.anchoredPosition = Vector2.LerpUnclamped(element.startPose, element.endPose, curveT);
            }

            yield return null;
        }

        if (element.objectUI != null)
        {
            element.objectUI.anchoredPosition = element.startPose;
        }
    }

    private void OnFadeIn(ElementAnimate element)
    {
        if (element.canvasAlpha != null)
        {
            element.canvasAlpha.alpha = element.OriginalAlpha;
        }
    }

    private void OnFadeOut(ElementAnimate element)
    {
        if (element.canvasAlpha != null)
        {
            element.canvasAlpha.alpha = 0f;
        }
    }
}

[System.Serializable]
public class AnimationGroup
{
    public string name;
    public List<ElementAnimate> elements;
}
[System.Serializable]
public class ElementAnimate
{
    public string name = "UI Element";
    public CanvasGroup canvasAlpha;
    public AnimationCurve moveEffect = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve FadeEffect = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public RectTransform objectUI;

    public float Duration = 0.5f;

    public Vector2 startPose;
    public Vector2 endPose;

    [HideInInspector] public float OriginalAlpha;

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
        if (objectUI != null)
        {
            if (startPose == Vector2.zero) 
            {
                startPose = objectUI.anchoredPosition;
            }
        }

        if (canvasAlpha != null)
        {
            OriginalAlpha = canvasAlpha.alpha;
        }
        else
        {
            OriginalAlpha = 1f;
        }
    }
}
