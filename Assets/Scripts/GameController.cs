using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Dialog, Menu, SceneSwitch}

public class GameController : MonoBehaviour, ISavable
{

    public PlayerController playerController;
    GameMenuController gameMenuController;
    [SerializeField] FadeImage blackScreen;
    Dictionary<string, int> flags;
    TimeSpan gametime;
    public static GameController i { get; private set; }

    public HashSet<SceneDetails> loadedScenes;
    public Dictionary<string,Character> characters;


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
        if (gametime == null)
            gametime = new TimeSpan();
        if (flags == null)
            flags = new Dictionary<string, int>();
        state = GameState.FreeRoam;
        loadedScenes = new HashSet<SceneDetails>();
        gameMenuController = GetComponent<GameMenuController>();
        previousStates = new List<GameState>();
        CutsceneManager.i.OnStartCutscene += () =>
        {
            ButtonContextController.i.ApplyContext("Next");
            ButtonContextController.i.ApplyContext("Skip");
            previousStates.Add(state);
            state = GameState.Dialog;
        };
        CutsceneManager.i.OnEndCutscene += () =>
        {
            if (state == GameState.Dialog)
                RevertState();
        };

    }

    public void SetFlag(string name, int value)
    {
        if (flags.ContainsKey(name))
        {
            flags[name] = value;
        }
        else
        {
            flags.Add(name, value);
        }
    }

    public void SetFlag(string name, bool value)
    {

        if (value)
        {
            SetFlag(name, 1);
        }
        else
        {
            SetFlag(name, 0);
        }
    }

    public void AddToFlag(string name, int delta)
    {
        if (flags.ContainsKey(name))
        {
            flags[name] += delta;
        }
        else
        {
            flags.Add(name, delta);
        }
    }

    public int GetFlag(string name)
    {
        if (flags.ContainsKey(name))
        {
            return flags[name];
        }
        else
        {
            return 0;
        }
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
        gametime.Add(TimeSpan.FromSeconds((double) Time.deltaTime));
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            playerController.Character.HandleUpdate();
            CutsceneManager.i.HandleUpdate();
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

    public object CaptureState()
    {
        return new GameControllerSave(flags, gametime);
    }

    public void RestoreState(object state)
    {
        GameControllerSave save = (GameControllerSave)state;
        flags = save.flags;
        gametime = save.gametime;
    }
}
