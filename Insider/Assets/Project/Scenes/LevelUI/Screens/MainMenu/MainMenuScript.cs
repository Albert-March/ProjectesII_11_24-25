using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void Update()
    {

    }

    public void PlayGame(Animator anim)
    {
        anim.Play("LevelSelectorOpen");
    }
    public void PlayTutorial()
    {
        S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
        transition.CallPass("Tutorial");
    }
    public void PlayLvl1()
    {
        S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
        transition.CallPass("Level_1");
    }
    public void PlayLvl2()
    {
        S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
        transition.CallPass("Level_2");
    }

    public void BackToMain(Animator anim)
    {
        anim.Play("LevelSelectorClose");
    }

    public void GoMainMenu()
    {
		S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
		transition.CallPass("MainMenu");
	}
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); 
    }
}