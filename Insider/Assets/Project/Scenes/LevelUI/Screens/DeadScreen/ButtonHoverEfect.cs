using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;
    public float hoverScale = 1.2f;
    public Button button;

    void Start()
    {
      
        originalScale = transform.localScale;

      
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

  
    public void OnMouseEnter()
    {
        transform.localScale = originalScale * hoverScale;
    }

   
    public void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}

