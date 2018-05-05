using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{

    [SerializeField]
    bool m_ScaleBackgroundSpriteBasedOnLandscapeOrPortraitCamera = false;

    //Called on constructions
    private void Awake()
    {
        if (m_ScaleBackgroundSpriteBasedOnLandscapeOrPortraitCamera)
        {
            Vector2 backgroundImageScale = GetSpriteScaleBasedOnCamera();
            transform.localScale = backgroundImageScale;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //This function is taken from https://kylewbanks.com/blog/create-fullscreen-background-image-in-unity2d-with-spriterenderer 
    //and is used as a placeholder for future use if you decide to change your camera. It just adjusts the texture size for a change of portrait or landscape.
    Vector2 GetSpriteScaleBasedOnCamera()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2; //orthographic size returns the half height we want the full height
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        {
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        {
            scale *= cameraSize.y / spriteSize.x;
        }

        return scale;
    }
}
