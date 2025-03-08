using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene("Level_1");
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("Level_2");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    //public void BackToMain()
    //{
    //    SceneManager.LoadScene("MainMenu");
    //}
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); 
    }
}