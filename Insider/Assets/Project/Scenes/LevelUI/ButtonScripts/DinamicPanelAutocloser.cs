using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DinamicPanelAutocloser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool locked = false;
    public GameObject LockButton;
    public Sprite[] Lock;

    public GameObject panel;
    public GameObject hoverButton;
    private bool isMouseOver = false;
    int i = 0;
    public void ToggleLock() 
    {
        locked = !locked;
    }
    void Update()
    {
        if (locked)
        {
            LockButton.GetComponent<Image>().sprite = Lock[1];
        }
        else 
        {
            LockButton.GetComponent<Image>().sprite = Lock[0];
            if (!isMouseOver && i == 1)
            {
                hoverButton.SetActive(false);
                panel.GetComponent<Animator>().SetBool("Open", false);
                i = 0;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        i = 1;
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}
