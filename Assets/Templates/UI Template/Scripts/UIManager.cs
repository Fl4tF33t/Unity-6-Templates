using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header("Collectable")]
    public Transform endPosition;
    private float moveDuration = 0.25f;
    private float totalTweenDuration = 0.75f;

    public void Collect(Utils.Collectable collectable, Transform spawnPosition, int amount = 15)
    { 
        float delay = totalTweenDuration/amount;

        for (int i = 0; i < amount; i++)
        {
            if(ObjectPool.Instance.TryGetPooledObject(collectable, out GameObject spawnedObj))
            {
                spawnedObj.SetActive(true);
                spawnedObj.transform.position = spawnPosition.position + ((Vector3)Random.insideUnitCircle * 50f);
                spawnedObj.transform.localScale = Vector3.zero;

                spawnedObj.transform.DOScale(Vector3.one, delay).SetEase(Ease.OutBack).SetDelay(i * (delay * 0.5f))
                    .OnComplete(() => spawnedObj.transform.DOMove(endPosition.position, moveDuration).SetEase(Ease.InBack)
                    .OnComplete(() => spawnedObj.SetActive(false)));
            }
        }
    }
}
