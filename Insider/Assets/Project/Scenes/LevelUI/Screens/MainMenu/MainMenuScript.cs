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
        //SceneManager.LoadScene("Level_1");
    }
    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void PlayLvl1()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void PlayLvl2()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void BackToMain(Animator anim)
    {
        anim.Play("LevelSelectorClose");
        //SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); 
    }
}