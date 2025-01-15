using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

   
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); 
    }
}