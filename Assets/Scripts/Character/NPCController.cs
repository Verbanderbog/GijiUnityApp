using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] List<Dialog> dialogs;
    [SerializeField] int dialogIndex;
    
    public void Interact()
    {
        
        StartCoroutine(DialogManager.Instance.ShowDialog(dialogs[dialogIndex]));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object CaptureState()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new NotImplementedException();
    }
}
