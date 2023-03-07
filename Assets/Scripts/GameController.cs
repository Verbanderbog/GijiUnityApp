using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Dialog, Menu, SceneSwitch}

public class GameController : MonoBehaviour
{

    public PlayerController playerController;
    GameMenuController gameMenuController;
    [SerializeField] FadeImage blackScreen;
    
    public static GameController i { get; private set; }

    public HashSet<SceneDetails> loadedScenes;
    

    List<GameState> previousStates;
    GameState state;
    public GameState State { get => state; }
    public FadeImage BlackScreen { get => blackScreen; }
    // Start is called before the first frame update
    void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        state = GameState.FreeRoam;
        loadedScenes = new HashSet<SceneDetails>();
        gameMenuController = GetComponent<GameMenuController>();
        previousStates = new List<GameState>();
        DialogManager.Instance.OnShowDialog += () =>
        {
            ButtonContextController.i.ApplyContext("Next");
            ButtonContextController.i.ApplyContext("Skip");
            previousStates.Add(state);
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
        state = previousStates[^1];
        previousStates.RemoveAt(previousStates.Count - 1);
        ButtonContextController.i.ApplyContext("Gi");
        ButtonContextController.i.ApplyContext("Ji");
    }

    public void MenuState(bool menu)
    {
         if (menu)
        {
            ButtonContextController.i.ApplyContext("Back");
            ButtonContextController.i.ApplyContext("Select");
            previousStates.Add(state);
            state = GameState.Menu;
        } else
        {
            RevertState();
        }
    }

    public void SceneState(bool sceneSwitch)
    {
        if (sceneSwitch)
        {
            previousStates.Add(state);
            state = GameState.SceneSwitch;
        }
        else
        {
            RevertState();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        if (state == GameState.FreeRoam)
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
        else if (state == GameState.SceneSwitch)
        {
            playerController.Character.HandleUpdate();
        }
        
    }
}
