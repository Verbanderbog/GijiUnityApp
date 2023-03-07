using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DestinationIdentifier { A, B, C, D, E, F, G, H, I, J, K, L }

public class ScenePortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier portalID;
    [SerializeField] DestinationIdentifier targetPortal;
    [SerializeField] bool offsetSpawn = false;
    [SerializeField] Transform spawnPoint;
    PlayerController player;

    public Transform SpawnPoint => spawnPoint;

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        GameController.i.SceneState(true);
        yield return GameController.i.BlackScreen.FadeIn(0.5f);
        if (SceneManager.sceneCountInBuildSettings != sceneToLoad)
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
        var destPortal = FindObjectsOfType<ScenePortal>().First(x => x!= this && this.targetPortal == x.portalID);
        var pos = (offsetSpawn) ? destPortal.transform.position + (player.transform.position - this.transform.position) : destPortal.transform.position;
        player.Character.SetPostitionAndSnapToTile(pos);
        pos = destPortal.SpawnPoint.position - destPortal.transform.position;
        StartCoroutine(player.Character.Move(pos));
        yield return GameController.i.BlackScreen.FadeOut(0.5f);
        GameController.i.SceneState(false);
        Destroy(gameObject);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        var pos = transform.position;
        var collider = GetComponent<BoxCollider2D>();

        pos.x = Mathf.Floor(pos.x);
        if (collider.size.x % 2 == 1)
            pos.x += 0.5f;
        pos.y = Mathf.Floor(pos.y);
        if (collider.size.y % 2 == 1)
            pos.y += 0.5f;

        transform.position = pos;
    }
#endif
}
