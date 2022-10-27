using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] enableIfSavePresent;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
