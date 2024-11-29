using UnityEngine;
using DG.Tweening;

public class MovingPopup : PopupAbstract
{
    [SerializeField]
    private Vector2 enterPosition;
    [SerializeField]
    private Vector2 exitPosition;
    [SerializeField]
    private float moveDuration;
    [SerializeField]
    protected Ease moveInEase = Ease.OutBack;
    [SerializeField]
    protected Ease moveOutEase = Ease.OutBack;
    protected override void Init()
    {
        base.Init();
    }

    public override void OnOpen()
    {
        rectTransform.localPosition = new Vector3(enterPosition.x, enterPosition.y, 0f);
        var moveTween = rectTransform.DOLocalMove(Vector2.zero, moveDuration).SetEase(moveInEase);

        longerDuration = Mathf.Max(longerDuration, moveTween.Duration(false));
        base.OnOpen();
    }


    public override void OnClose()
    {
        rectTransform.DOLocalMove(exitPosition, moveDuration).SetEase(moveOutEase);
        base.OnClose();
    }
}
