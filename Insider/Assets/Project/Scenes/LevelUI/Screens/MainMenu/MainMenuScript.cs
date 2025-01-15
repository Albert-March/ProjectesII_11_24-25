using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                Application.Quit();
            }
            else 
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); 
    }
}