using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] enableIfSavePresent;
    [SerializeField] FadeImage blackScreen;
    private void Start()
    {
        bool hasSave = true; //TODO: find a way to actually calculate this
        if (hasSave)
        {
            foreach (GameObject t in enableIfSavePresent)
            {
                t.SetActive(true);
            }
        }
    }
    public void PlayGame()
    {
        
        StartCoroutine(SwitchScene());
    }
    
    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(blackScreen.transform.parent.gameObject);
        yield return blackScreen.FadeIn(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        yield return blackScreen.FadeOut(0.5f);
        Destroy(blackScreen.transform.parent.gameObject);
        Destroy(gameObject);
    }
    

}
