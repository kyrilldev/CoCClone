using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BounceAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 ZoominAmount;
    public Vector3 ZoomoutAmount;

    public float Animspeed;

    public bool CanZoomIn = false;
    public bool CanZoomOut = false;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.Slerp(ZoomoutAmount, ZoominAmount, Time.deltaTime * Animspeed);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.Slerp(ZoominAmount, ZoomoutAmount, Time.deltaTime * Animspeed);
    }

    private void OnDisable()
    {
        GetComponent<RectTransform>().localScale = Vector3.Slerp(ZoominAmount, ZoomoutAmount, Time.deltaTime * Animspeed);
    }
}
