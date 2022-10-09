using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    private Gamepad gamepad;
    [SerializeField] List<GameObject> selectionArrows;
    [SerializeField] List<Button> menuButtons;
    [SerializeField] GameObject menu;
    private int selection = 0;
    DateTime lastStickMove;
    private static readonly TimeSpan STICK_DELAY = TimeSpan.FromMilliseconds(350);
    private void Awake()
    {
        gamepad = Gamepad.current;
        lastStickMove = DateTime.Now;
    }
    public void HandleUpdate()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            menu.SetActive(false);
            GameController.Instance.MenuState(false);
        }
        else
        {
            Vector2 stick = gamepad.leftStick.ReadValue();
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                menuButtons[selection].onClick.Invoke();
            }
            else if (stick.y > 0.3 && DateTime.Now - lastStickMove > STICK_DELAY)
            {
                lastStickMove = DateTime.Now;
                selectionArrows[selection--].SetActive(false);
                if (selection < 0)
                    selection = selectionArrows.Count - 1;
                selectionArrows[selection].SetActive(true);
            }
            else if (stick.y < -0.3 && DateTime.Now - lastStickMove > STICK_DELAY)
            {
                lastStickMove = DateTime.Now;
                selectionArrows[selection++].SetActive(false);
                if (selection > selectionArrows.Count - 1)
                    selection = 0;
                selectionArrows[selection].SetActive(true);
            }
        }
    }
}