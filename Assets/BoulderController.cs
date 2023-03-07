using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour, Interactable
{
    Character character;

    string Interactable.GetContextName()
    {
        return "Push";
    }

    void Interactable.Interact(PlayerController player)
    {
        StartCoroutine(character.Move(player.gameObject.transform.position));
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        character.HandleUpdate();
    }
}
