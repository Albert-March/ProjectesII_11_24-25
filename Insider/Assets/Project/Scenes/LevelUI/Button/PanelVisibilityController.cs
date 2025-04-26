using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PanelVisibilityController : MonoBehaviour
{
    public GameObject panel;
    public Camera m_Camera;
    private Button lastButton = null;
    public GameObject bg;
    AudioManager audioManager;

    public bool open;

    private void Awake()
    {
        bg.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TogglePanel(Button button)
    {
        if (!panel.GetComponent<Animator>().GetBool("Open"))
        {
            OpenPanel(button);
            open = true;
        }
        else
        {
            if (button == lastButton)
            {
                ClosePanel();
				open = false;
			}
            else
            {
                StartCoroutine(SwapPanel(button));
            }
        }
    }

    private void OpenPanel(Button button)
    {
        panel.GetComponent<Animator>().SetBool("Open", true);
        lastButton = button;
        bg.SetActive(true);

    }

    private void ClosePanel()
    {
        panel.GetComponent<Animator>().SetBool("Open", false);
        lastButton = null;
        bg.SetActive(false);
    }

    private IEnumerator SwapPanel(Button button)
    {
        ClosePanel();
        yield return new WaitForSeconds(0.3f); // Simula animación de cierre
        OpenPanel(button);
    }

    public void CloseBGPanel()
    {
        panel.GetComponent<Animator>().SetBool("Open", false);
        lastButton = null;
        bg.SetActive(false);
        audioManager.PlaySFX(2, 0.2f);
    }
}

