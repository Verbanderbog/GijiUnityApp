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
    int textSpeed=50;

    Gamepad gamepad;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    //public bool IsShowing { get; private set; }

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        gamepad = Gamepad.current;
    }
    Action onDialogFinished;
    Dialog dialog;
    int currentLine = 0;
    bool isTyping = false;
    bool endTyping = false;

    public IEnumerator ShowDialog(Dialog dialog, Action onDialogFinished=null)
    {
        UpdateTextSpeed();
        if (gamepad == null)
            gamepad = Gamepad.current;
        this.onDialogFinished = onDialogFinished;
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        //IsShowing = true;
        this.dialog = dialog;
        
        dialogContainer.SetActive(true);
        dialogReaction.sprite = (dialog.Lines[0].Reaction != Reaction.Empty && dialog.Lines[0].ParticipantIndex >=0) ? dialog.Participants[dialog.Lines[0].ParticipantIndex].images[(int)dialog.Lines[0].Reaction] : null;
        if (dialog.Lines[0].Reaction != Reaction.Empty && dialog.Lines[0].ParticipantIndex >= 0)
        {
            dialogReaction.enabled = true;
            dialogReaction.sprite = dialog.Participants[dialog.Lines[0].ParticipantIndex].images[(int)dialog.Lines[0].Reaction];
        }
        else
        {
            dialogReaction.enabled = false;
        }
        string type = dialog.Participants[dialog.Lines[0].ParticipantIndex].Name;
        type = (type == null || type == "") ? dialog.Lines[0].Line : type + ": " + dialog.Lines[0].Line;
        StartCoroutine(TypeDialog(type));

    }

    public void UpdateTextSpeed()
    {
        var temp = PlayerPrefs.GetInt("Text Speed");
        textSpeed = (temp != 0) ? temp : 50;
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
            yield return new WaitForSeconds(1f / textSpeed * speedMultiplier);
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
                if (dialog.Lines[currentLine].Reaction == Reaction.Empty || dialog.Lines[currentLine].ParticipantIndex < 0)
                {
                    dialogReaction.enabled = false;
                }
                else if (!dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].images[(int)dialog.Lines[currentLine].Reaction].Equals(dialogReaction.sprite))
                {
                    dialogReaction.enabled = true;
                    dialogReaction.sprite = dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].images[(int)dialog.Lines[currentLine].Reaction];
                }
                string type = (dialog.Lines[currentLine].ParticipantIndex<0) ? null : dialog.Participants[dialog.Lines[currentLine].ParticipantIndex].Name ;
                type = (type == null || type == "") ? dialog.Lines[currentLine].Line : type + ": " + dialog.Lines[currentLine].Line;
                StartCoroutine(TypeDialog(type));
            }
            else
            {
                currentLine = 0;
                //IsShowing = false;
                dialogContainer.SetActive(false);
                onDialogFinished?.Invoke();
                OnCloseDialog?.Invoke();
            }
        }
    }
}
