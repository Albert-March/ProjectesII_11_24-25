using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverImage;
    public Vector2 offset = new Vector2(150, 0);

    private RectTransform hoverImageRect;

    void Start()
    {
        if (hoverImage != null)
        {
            hoverImageRect = hoverImage.GetComponent<RectTransform>();
            hoverImage.SetActive(false);
        }
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.SetActive(true);
            Vector3 buttonPosition = GetComponent<RectTransform>().position;
            hoverImageRect.position = buttonPosition + new Vector3(offset.x, offset.y, 0);
        }
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.SetActive(false);
        }
    }
}




