using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static PauseMenu pause;

	public GameObject pauseMenu;
	public bool active;


	void Awake()
	{
		if (pause == null)
		{
			pause = this;
		}
		else if (pause != this)
		{
			Destroy(gameObject);
		}
	}


	void Start()
	{
		pauseMenu.SetActive(false);
		active = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			active = !active;
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu()
	{
		if (active)
		{
			pauseMenu.SetActive(true);
			//Time.timeScale = 0f;
		}
		else
		{
			pauseMenu.SetActive(false);
			//Time.timeScale = 1.0f;
		}
	}

	public void Resume()
	{
		pause.active = false;
		pause.TogglePauseMenu();
	}

	public void BackToMain()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
