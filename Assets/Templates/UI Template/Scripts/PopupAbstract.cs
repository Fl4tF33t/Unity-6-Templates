using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class PopupAbstract : MonoBehaviour
{
    //Intended as an abstract class for all ui elements that will appear and disappear
    //Should be placed at the parent of the component that is going to appear and disappear

    #region Tweens
    protected RectTransform rectTransform;

    protected CanvasGroup canvasGroup;
    private Tween scaleTween;
    private Tween alphaTween;
    private Tween bounceTween;
    protected float longerDuration;

    [Header("Canvas Group")]
    [Min(0f)]
    [SerializeField]
    protected float alphaDuration = .25f;
    [SerializeField]
    protected Ease alphaEase = Ease.Linear; 

    [Header("Rect Transform")]
    [Min(0f)]
    [SerializeField]
    protected float scaleDuration = 0.25f;
    [SerializeField]
    protected Ease scaleEase = Ease.OutBack;
    [SerializeField]
    protected float bounceSpeed = 0.2f;
    #endregion

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        Init();
    }

    protected virtual void Init()
    {
        scaleTween.SetAutoKill(false);
        alphaTween.SetAutoKill(false);
        bounceTween.SetAutoKill(false);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        rectTransform.localScale = Vector2.zero;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;

        // Create reusable tweens
        scaleTween = rectTransform.DOScale(Vector2.one, scaleDuration)
            .SetEase(scaleEase)
            .SetAutoKill(false)
            .Pause();

        bounceTween = rectTransform.DOScale(Vector2.one * 1.15f, bounceSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .Pause();
                

        alphaTween = canvasGroup.DOFade(1f, alphaDuration)
            .SetEase(alphaEase)
            .SetAutoKill(false)
            .Pause();

        longerDuration = Mathf.Max(scaleDuration, alphaDuration);
    }

    public virtual void OnOpen()
    {
        if (scaleTween.IsPlaying() || alphaTween.IsPlaying())
            return;

        gameObject.SetActive(true);

        scaleTween.PlayForward();
        alphaTween.PlayForward();

        DOTween.Sequence()
            .AppendInterval(longerDuration)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
            });
    }
    public virtual void OnClose()
    {
        if (scaleTween.IsPlaying() || alphaTween.IsPlaying())
            return;

        canvasGroup.interactable = false;

        scaleTween.PlayBackwards();
        alphaTween.PlayBackwards();

        // Schedule deactivation after the longer duration
        DOTween.Sequence()
            .AppendInterval(longerDuration)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                StopBounce(); // Deactivate the object
            });
    }
    public virtual void OnBounce()
    {
        bounceTween.Play();
    }

    private void StopBounce()
    {
        if (!bounceTween.IsPlaying())
            return;

        bounceTween.Pause();
        bounceTween.Rewind();
    }
}
