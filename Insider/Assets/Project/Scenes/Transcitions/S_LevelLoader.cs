using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_LevelLoader : MonoBehaviour
{
    public Animator transcition;
    public float  ttime = 1;
    // Update is called once per frame
    public void CallPass(string Level)
    {
        StartCoroutine(LoadLevel(Level));
    }
    IEnumerator LoadLevel(string levelIndex)
    {
        transcition.SetTrigger("Start");
        yield return new WaitForSeconds(ttime);
        SceneManager.LoadScene(levelIndex);
    }
}
