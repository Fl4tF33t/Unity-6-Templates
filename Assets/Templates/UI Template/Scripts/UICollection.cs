using UnityEngine;
using DG.Tweening;
public class UICollection : MonoBehaviour
{
    public Transform spawnPosition;
    public Transform endPosition;
    public float moveDuration;
    public Ease moveEase;

    public int spawnAmount = 10;
    public float spawnDelay = 0.2f;
    public float totalDelay = 1f;

    public void Collect(Utils.Collectable collectable)
    {
        spawnDelay = totalDelay/spawnAmount;
        for (int i = 0; i < spawnAmount; i++)
        {
            ShowCoin(i * spawnDelay);
        }
    }
    private void ShowCoin(float delay)
    {
        if(!ObjectPool.Instance.TryGetPooledObject(Utils.Collectable.Coin, out GameObject spawnedObj))
            return;

        spawnedObj.SetActive(true);

        Vector3 offset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);
        spawnedObj.transform.position = spawnPosition.transform.position + offset;

        spawnedObj.transform.localScale = Vector3.zero;
        spawnedObj.transform.DOScale(Vector3.one, delay);

        spawnedObj.transform.DOMove(endPosition.position, moveDuration).SetEase(moveEase).SetDelay(delay).OnComplete(() => spawnedObj.SetActive(false));
    }
}
