using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject GemPrefab;
    [SerializeField]
    GameObject GameGridObject;

    GameGrid m_GameGridComponent;

    // Use this for initialization
    void Start()
    {
        if (GameGridObject != null)
        {
            m_GameGridComponent = GameGridObject.GetComponent<GameGrid>();

            for (int x = 0; x < m_GameGridComponent.GetRowCount(); ++x)                                        //For loop that goes round the number of rows are in the game grid
            {
                for (int y = 0; y < m_GameGridComponent.GetColCount(); ++y)
                {
                    CellItem cellItem = m_GameGridComponent.GetGridCell(x, y);
                    cellItem.m_CurrentObject = CreateGemObject(cellItem.CellPosition);
                }
            }

            StartCoroutine(LateStart());
        }
    }

    //A deferred start so we can use the components for objects this game has spawned
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);

        for (int x = 0; x < m_GameGridComponent.GetRowCount(); ++x)                                        //For loop that goes round the number of rows are in the game grid
        {
            for (int y = 0; y < m_GameGridComponent.GetColCount(); ++y)
            {
                CellItem cellItem = m_GameGridComponent.GetGridCell(x, y);
                int index = m_GameGridComponent.CellCooridnatesToIndex(x, y);
                cellItem.m_CurrentObject.GetComponent<Gem>().Register(index, this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject CreateGemObject(Vector3 location)
    {
        GameObject gemObject = GameObject.Instantiate(GemPrefab, location, Quaternion.identity);
        return gemObject;
    }

    public void ItemSelected(int id)
    {
        GameGrid gameGridComponent = GameGridObject.GetComponent<GameGrid>();
        CellItem cellItem = gameGridComponent.GetGridCell(id);
        cellItem.m_CurrentObject.GetComponent<Gem>().SetPlayerColour(true);
    }
}
