using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    int id;
    GameController objectToNotify;
    float moveSpeed = 3.0f;
    // Use this for initialization
    void Start()
    {
    

    }

    public void SetPlayerColour(bool player1turn)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (player1turn)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update()

    {

        // transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * moveSpeed);
     
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
        //{
        //    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //    if (spriteRenderer.color == Color.blue)
        //    {
        //        spriteRenderer.color = Color.red;
        //    }
        //    else
        //    {
        //      spriteRenderer.color = Color.blue;
    }

    
}                       