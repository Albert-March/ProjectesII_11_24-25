using UnityEngine;
using UnityEngine.UI;

public class TutorialToggle : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        toggle.isOn = true;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool value)
    {
        TutorialManager.instance.tutorialEnabled = value;
    }
}
