using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AmplyfyButtonOnHover : MonoBehaviour
{
    AudioManager audioManager;
    Vector2 realSize;
    Vector2 bigSize;
    // Start is called before the first frame update
    void Start()
    {
        realSize = transform.localScale;
        bigSize = realSize * 1.2f;
    }

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	public void ChangeScale(bool big) 
    {
        if (big)
        {
            transform.localScale = bigSize;
            audioManager.PlaySFX(0, 0.2f);
        }
        else 
        {
            transform.localScale = realSize;
        }
    }

}
