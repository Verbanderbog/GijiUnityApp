using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Dialog, Menu}

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    GameMenuController gameMenuController;
    public static GameController Instance { get; private set; }

    GameState previousState;
    GameState state;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameMenuController = GetComponent<GameMenuController>();
        DialogManager.Instance.OnShowDialog += () =>
        {
            previousState = state;
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                RevertState();
        };

    }

    

    void RevertState()
    {
        GameState temp = state;
        state = previousState;
        previousState = temp;
    }

    public void MenuState(bool menu)
    {
         if (menu)
        {
            previousState = state;
            state = GameState.Menu;
        } else
        {
            RevertState();
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            gameMenuController.HandleUpdate();
        }
    }
}
