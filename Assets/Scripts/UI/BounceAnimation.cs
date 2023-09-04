using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BounceAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 ZoominAmount;
    public Vector3 ZoomoutAmount;

    public float Animspeed;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.Lerp(ZoomoutAmount, ZoominAmount, Time.deltaTime * Animspeed);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.Lerp(ZoominAmount, ZoomoutAmount, Time.deltaTime * Animspeed);
    }

    private void OnDisable()
    {
        GetComponent<RectTransform>().localScale = Vector3.Lerp(ZoominAmount, ZoomoutAmount, Time.deltaTime * Animspeed);
    }
}
