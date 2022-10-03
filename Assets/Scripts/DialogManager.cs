using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogContainer;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Image dialogReaction;

    Gamepad gamepad;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        gamepad = Gamepad.current;
    }

    Dialog dialog;
    int currentLine = 0;
    bool isTyping = false;
    bool endTyping = false;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;

        dialogContainer.SetActive(true);
        dialogReaction.sprite = dialog.Participants[dialog.Lines[0].ParticipantIndex].images[(int)dialog.Lines[0].Reaction];
        string type = dialog.Participants[dialog.Lines[0].ParticipantIndex].Name;
        type = (type == null || type == "") ? dialog.Lines[0].Line : type + ": " + dialog.Lines[0].Line;
        StartCoroutine(TypeDialog(type));

    }



    public IEnumerator TypeDialog(string line)
    {

        isTyping = true;
        dialogText.SetText("");
        foreach (var letter in line.ToCharArray())
        {
            if (endTyping)
            {
                endTyping = false;
                break;
            }
            dialogText.text += letter;
            float speedMultiplier = (gamepad.buttonSouth.isPressed) ? 2f: 1f;
            yield return new WaitForSeconds(1f / ((PlayerPrefs.GetInt("Letters per second") != 0) ? PlayerPrefs.GetInt("Letters per second") * speedMultiplier : 50f * speedMultiplier));
        }
        isTyping = false;

    }

    public IEnumerator ChangeAvatar(Sprite image)
    {
        if (!dialogReaction.sprite.Equals(image))
        {
            if (dialogReaction.color.a == 1f)
                for (int i = 254; dialogReaction.color.a > 0f; i--)
                {
                    dialogReaction.color = new Color(1f, 1f, 1f, i / 255f);
                    yield return null;
                }
            
            dialogReaction.sprite = image;
            for (int i = 1; dialogReaction.color.a < 255f; i++)
            {
                dialogReaction.color = new Color(1f, 1f, 1f, i / 255f);
                yield return null;
            }
        }

    }

    public void HandleUpdate()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
        if (gamepad.buttonEast.wasPressedThisFrame && isTyping)
        {
            endTyping = true;
            string type = dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].Name;
            type = (type == null || type == "") ? dialog.Lines[currentLine].Line : type + ": " + dialog.Lines[currentLine].Line;
            dialogText.text = type;
        }
        if ((gamepad.buttonSouth.wasPressedThisFrame || gamepad.buttonEast.wasPressedThisFrame) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                if (!dialogReaction.sprite.Equals(dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].images[(int)dialog.Lines[currentLine].Reaction]))
                {
                    
                    dialogReaction.sprite = (dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].images[(int)dialog.Lines[currentLine].Reaction]);
                }
                string type = dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].Name;
                type = (type == null || type == "") ? dialog.Lines[currentLine].Line : type + ": " + dialog.Lines[currentLine].Line;
                StartCoroutine(TypeDialog(type));
            }
            else
            {
                currentLine = 0;
                dialogContainer.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }
}
