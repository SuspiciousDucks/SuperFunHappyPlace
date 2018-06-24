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
    Assets.Scripts.PlayerManager m_PlayerManager;

    private void Awake()
    {
        m_PlayerManager = new Assets.Scripts.PlayerManager();
    }
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

    //A deferred start so we can use the components for objects this game has spawned. Maybe we should use messages instead but this will do for now.
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);

        for (int x = 0; x < m_GameGridComponent.GetRowCount(); ++x)
        {
            for (int y = 0; y < m_GameGridComponent.GetColCount(); ++y)
            {
                CellItem cellItem = m_GameGridComponent.GetGridCell(x, y);
                int index = m_GameGridComponent.CellCooridnatesToIndex(x, y);
                cellItem.m_CurrentObject.GetComponent<Gem>().Register(index, this);
            }
        }
    }

    GameObject CreateGemObject(Vector3 location)
    {
        GameObject gemObject = GameObject.Instantiate(GemPrefab, location, Quaternion.identity);
        return gemObject;
    }

    public void ItemSelected(int id)
    {
        CellItem cellItem = GetCellToPlaceCounter(id);
        if (cellItem != null)
        {
            ProcessPlayerMove(cellItem);
            EvaluateGameState();
            EndTurn();
        }
    }

    CellItem GetCellToPlaceCounter(int originalCellId)
    {
        //TODO Do some math here to work out what cell we actually need to be working with.
        //hint use m_GameGridComponent for help it has useful functions and info.
        int targetCell = originalCellId;

        return m_GameGridComponent.GetGridCell(targetCell);
    }

    void ProcessPlayerMove(CellItem cellItem)
    {
        Color teamColor = m_PlayerManager.GetActivePlayersTurn().PlayerColor;
        Gem selectedItemGem = cellItem.m_CurrentObject.GetComponent<Gem>();
        selectedItemGem.SetPlayerColour(teamColor);
    }

    void EvaluateGameState()
    {
        //TODO
    }

    void EndTurn()
    {
        m_PlayerManager.SwapTurn();
    }
}
