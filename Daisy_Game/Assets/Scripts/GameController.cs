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
    bool isGameOver;


    private void Awake()
    {
        m_PlayerManager = new Assets.Scripts.PlayerManager();
    }
    // Use this for initialization
    void Start()
    {
        isGameOver = false;
        if (GameGridObject != null)
        {
            m_GameGridComponent = GameGridObject.GetComponent<GameGrid>();

            for (int x = 0; x < m_GameGridComponent.GetRowLength(); ++x)                                        //For loop that goes round the number of rows are in the game grid
            {
                for (int y = 0; y < m_GameGridComponent.GetColLength(); ++y)
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

        for (int x = 0; x < m_GameGridComponent.GetRowLength(); ++x)
        {
            for (int y = 0; y < m_GameGridComponent.GetColLength(); ++y)
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
        CellItem clickSelectedCell = GetCellToPlaceCounter(id);
        if (clickSelectedCell != null && !isGameOver)
        {
            CellItem changedCell = ProcessPlayerMove(clickSelectedCell, id);
            if (changedCell != null)
            {
                EvaluateGameState(changedCell);
                EndTurn();
            }
        }
    }

    CellItem GetCellToPlaceCounter(int originalCellId)
    {
        //TODO Do some math here to work out what cell we actually need to be working with.
        //hint use m_GameGridComponent for help it has useful functions and info.
        int targetCell = originalCellId;

        return m_GameGridComponent.GetGridCell(targetCell);
    }

    //Returns the found cell item from processing players move
    CellItem ProcessPlayerMove(CellItem cellItem , int Index)
    {
        int column = cellItem.CellColumn;
        int columnheight = m_GameGridComponent.GetColLength();

        for (int i = 0; i < columnheight; i++)
        {
            CellItem FoundCell = m_GameGridComponent.GetGridCell(column, i);
            Gem selectedItemGem = FoundCell.m_CurrentObject.GetComponent<Gem>();

            if (!selectedItemGem.IsGemOwned())
            {
                Color teamColor = m_PlayerManager.GetActivePlayersTurn().PlayerColor;
                int teamInt = m_PlayerManager.GetActivePlayersTurn().PlayerId;
                selectedItemGem.SetPlayerColour(teamColor);
                selectedItemGem.SetPlayerOwner(teamInt);
                return FoundCell;
            }
        }

        //failed to do move
        return null;
    }
    Gem GetCellGem(CellItem item)
    {
        return item.m_CurrentObject.GetComponent<Gem>();

    }
    void EvaluateGameState(CellItem item)
    {
        if (m_GameGridComponent.IsRowComplete(item.CellRow, m_PlayerManager.GetActivePlayersTurn().PlayerId, 4))
        {
            GameOver();
            return;
        }

        if (m_GameGridComponent.IsColComplete(item.CellColumn, m_PlayerManager.GetActivePlayersTurn().PlayerId, 4))
        {
            GameOver();
            return;
        }

        if (m_GameGridComponent.IsDiagonalComplete(item, m_PlayerManager.GetActivePlayersTurn().PlayerId, 4))
        {
            GameOver();
            return;
        }
    }

    void EndTurn()
    {
        m_PlayerManager.SwapTurn();
    }
    void GameOver()
    {
        isGameOver = true;
        //anything else that needs to happen on gameover

        Debug.LogFormat("Game Over");
    }
    void RestartGame()
    {
        for (int x = 0; x < m_GameGridComponent.GetRowLength(); ++x)                                        
        {
            for (int y = 0; y < m_GameGridComponent.GetColLength(); ++y)
            {
                CellItem cellItem = m_GameGridComponent.GetGridCell(x, y);
                GetCellGem(cellItem).Restart();

            }
        }
        isGameOver = false;
    }
}
