using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class OnPointerEnterExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEvent onPointerEnter = new UnityEvent();
    [HideInInspector] public UnityEvent onPointerExit = new UnityEvent();

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }
}