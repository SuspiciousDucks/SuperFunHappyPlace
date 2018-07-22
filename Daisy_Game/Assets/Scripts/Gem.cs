using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    bool IsOwned;
    int OwnerId;
    int id;
    GameController objectToNotify;

    // Use this for initialization
    void Start()
    {
        IsOwned = false;
        OwnerId = -1;
    }
    public void SetPlayerOwner(int team)
    {
        OwnerId = team;

    }
    public void SetPlayerColour(Color playerColor)
      {

        if (IsOwned == false)
        {
            IsOwned = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = playerColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Register(int index, GameController controller)
    {
        id = index;
        objectToNotify = controller;
    }



    void OnMouseDown()
    {
        if (objectToNotify)
        {
            objectToNotify.ItemSelected(id);
        }

        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //if (spriteRenderer.color == Color.blue)
        //{
        //    spriteRenderer.color = Color.red;
        //}
        //else
        //{
        //    spriteRenderer.color = Color.blue;
        //}

    }
    public bool IsGemOwned()
    {

        return IsOwned;

    }
    public int GetPlayerId()
    {
        return OwnerId;
    }


    public void Restart()
    {
        IsOwned = false;
        OwnerId = -1;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

    }
}   