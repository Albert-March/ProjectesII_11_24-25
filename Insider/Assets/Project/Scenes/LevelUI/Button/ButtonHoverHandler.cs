using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ButtonHoverHandler : MonoBehaviour
{
    public bool OnHover = false;
    private char type = 'A';
    public SetTowerBaseInput dinamicPanel;
    public GameObject hoverImage;
    public RawImage image;
    public Sprite[] towerSpot;
    public Texture cameraTower;

    void Start()
    {
        
    }

    private void Update()
    {
        Debug.Log(hoverImage.GetComponent<Animator>().GetBool("Open"));
        if (dinamicPanel.spawnTower)
        {
            hoverImage.transform.GetChild(1).GetComponent<Image>().enabled = true;
            hoverImage.GetComponent<Animator>().SetBool("Open", true);
            image.texture = cameraTower;
        }
        else 
        {
            if (!OnHover)
            {
                hoverImage.GetComponent<Animator>().SetBool("Open", false);
            }
            else 
            { 
                hoverImage.GetComponent<Animator>().SetBool("Open", true);
            }
            hoverImage.transform.GetChild(1).GetComponent<Image>().enabled = false;

            switch (dinamicPanel.towerGrup)
            {

                case 0:
                    if (type == 'A')
                    {
                        image.texture = towerSpot[0].texture;
                    }
                    else if (type == 'B')
                    {
                        image.texture = towerSpot[1].texture;
                    }
                    break;
                case 1:
                    if (type == 'A')
                    {
                        image.texture = towerSpot[2].texture;
                    }
                    else if (type == 'B')
                    {
                        image.texture = towerSpot[3].texture;
                    }
                    break;
                case 2:
                    if (type == 'A')
                    {
                        image.texture = towerSpot[4].texture;
                    }
                    else if (type == 'B')
                    {
                        image.texture = towerSpot[5].texture;
                    }
                    break;
            }
        }
    }


    public void OnHovering(bool OnHover) 
    {
        this.OnHover = OnHover;
    }
    public void TypeA() 
    {
        type = 'A';
    }

    public void TypeB()
    {
        type = 'B';
    }
}




