using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject GemPrefab;
    [SerializeField]
    GameObject GameGridObject;


    // Use this for initialization
    void Start()
    {

        if (GameGridObject != null)
        {
            GameGrid gameGridComponent = GameGridObject.GetComponent<GameGrid>();
            
            for(int x = 0; x < gameGridComponent.GetRowCount();++x)                                        //For loop that goes round the number of rows are in the game grid
            {
                for(int y = 0; y < gameGridComponent.GetColCount();++y)
                {
                    CellItem cellItem = gameGridComponent.GetGridCell(x, y);
                    int index = gameGridComponent.CellCooridnatesToIndex(x, y);
                    cellItem.m_Gem = CreateGem(cellItem.CellPosition, index);
                    

                    

                }

            }


        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    Gem CreateGem(Vector3 location,int index)
    {
        GameObject gemObject = GameObject.Instantiate(GemPrefab, location, Quaternion.identity);
       Gem gem = gameObject.GetComponent("Gem")as Gem;
       // gem.Register(index, this);
        return gem;
    }

    public void ItemSelected(int id)
    {
        GameGrid gameGridComponent = GameGridObject.GetComponent<GameGrid>();
        CellItem cellItem = gameGridComponent.GetGridCell(id);
        cellItem.m_Gem.SetPlayerColour(true);


    }
}
