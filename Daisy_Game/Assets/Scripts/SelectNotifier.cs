using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNotifier : MonoBehaviour {
    int id;
    GameController objectToNotify;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnMouseDown()
    {
        if (objectToNotify)
        {
            objectToNotify.ItemSelected(id);
        
            
        }
        
    }
    public void Register(int index, GameController controller)
    {
        id = index;
        objectToNotify = controller;
    }
}  
