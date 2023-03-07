using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject cam;
    private SpriteRenderer camrenderer;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");

        // get the sprite width in world space units
        camrenderer = GetComponent<SpriteRenderer>();
        float worldSpriteWidth = camrenderer.sprite.bounds.size.x;

        // get the screen height & width in world space units
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

        // initialize new scale to the current scale
        Vector3 newScale = transform.localScale;

        // divide screen width by sprite width, set to X axis scale
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;

        // apply scale change
        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(cam.transform.position.x, this.transform.position.y, this.transform.position.z);
        // get the sprite width in world space units
        float worldSpriteWidth = camrenderer.sprite.bounds.size.x;

        // get the screen height & width in world space units
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

        // initialize new scale to the current scale
        Vector3 newScale = transform.localScale;

        // divide screen width by sprite width, set to X axis scale
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;

        // apply scale change
        transform.localScale = newScale;
    }
}
