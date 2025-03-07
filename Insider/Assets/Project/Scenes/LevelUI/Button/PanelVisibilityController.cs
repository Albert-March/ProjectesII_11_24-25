using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelVisibilityController : MonoBehaviour
{
    public GameObject panel;
    public DinamicPanelAutocloser lockbutton;
    public Camera m_Camera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null) // Check if we hit something
            {
                Debug.Log(hit.transform.gameObject.layer);
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    panel.GetComponent<Animator>().SetBool("Open", false);
                }
            }
        }
    }


    public void TogglePanel()
    {
        if (panel.GetComponent<Animator>().GetBool("Open"))
        {
            panel.GetComponent<Animator>().SetBool("Open", false);
        }
        else 
        {
            panel.GetComponent<Animator>().SetBool("Open", !panel.GetComponent<Animator>().GetBool("Open"));
        }
    }
}

