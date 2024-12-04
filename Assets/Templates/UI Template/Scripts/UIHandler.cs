using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerClickHandler
{
    private ObjectPool objectPool;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Collect(Utils.Collectable.Coin, this.transform);
    }
}
