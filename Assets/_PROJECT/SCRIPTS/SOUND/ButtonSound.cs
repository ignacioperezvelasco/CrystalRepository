using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public string HoverSound;
    public string PressSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(HoverSound);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(PressSound);
    }
}
