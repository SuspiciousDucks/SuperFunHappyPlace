using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//A wrapper class that holds information about a cell.
public class CellItem
{
    private Vector3 m_CellPosition;
    private int m_Column;
    private int m_Row;

    public GameObject m_CurrentObject;

    public Vector3 CellPosition
    {
        get { return m_CellPosition; }
        set { m_CellPosition = value; }
    }

    public int CellColumn
    {
        get
        {
            return m_Column;
        }
        set
        {
            m_Column = value;
        }
    }

    public int CellRow
    {
        get
        {
            return m_Row;
        }
        set
        {
            m_Row = value;
        }
    }

    public void DebugDrawCell(Vector2 extends, float z)
    {
        Vector3 cellPosition = this.m_CellPosition;
        cellPosition.z = z;

        //don't care for inefficiency this is just debug
        Vector3 leftBottomOffset = extends * -0.5f;
        Vector3 rightTopOffset = extends * 0.5f;
        Vector3 leftTopOffset = new Vector3(leftBottomOffset.x, rightTopOffset.y, 0);
        Vector3 righBottomOffset = new Vector3(rightTopOffset.x, leftBottomOffset.y, 0);


        //using the cell position draw the grid.
        Debug.DrawLine(cellPosition + leftBottomOffset, cellPosition + leftTopOffset, Color.black, 0, false);
        Debug.DrawLine(cellPosition + leftTopOffset, cellPosition + rightTopOffset, Color.black, 0, false);
        Debug.DrawLine(cellPosition + rightTopOffset, cellPosition + righBottomOffset, Color.black, 0, false);
        Debug.DrawLine(cellPosition + righBottomOffset, cellPosition + leftBottomOffset, Color.black, 0, false);
    }
}

//namespace
public enum Corners
{
    TL,
    TR,
    BL,
    BR
}


public class GameGrid : MonoBehaviour
{
    [SerializeField]
    bool CenterGrid = true;

    [SerializeField]
    float MaximumWidth = 10.0f;
    [SerializeField]
    float MaximumHeight = 10.0f;

    [SerializeField]
    int CellCountX = 12;
    [SerializeField]
    int CellCountY = 12;

    CellItem[] m_Cells;

    Vector2 CellExtends;
    int m_CellCount;

    void Awake()
    {
        if (CenterGrid)
        {
            this.transform.position -= new Vector3(MaximumWidth * 0.5f, MaximumHeight * 0.5f, 0);
        }

        m_CellCount = CellCountX * CellCountY;
        this.CellExtends = new Vector2(MaximumWidth / CellCountX, MaximumHeight / CellCountY);

        //Heap Allocated Array of contiguous memory. It has faster access than say a List<T>, which will be helpful when we want to run
        m_Cells = new CellItem[CellCountX * CellCountY];
        for (int i = 0; i < m_CellCount; ++i)
        {
            m_Cells[i] = new CellItem();
        }
        CalculateCells();
    }

    // Use this for initialization
    void Start()
    {
        //Drive the main camera to fit the content into the screen.
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = MaximumWidth / MaximumHeight;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = MaximumHeight / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = MaximumHeight / 2 * differenceInSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            //DrawLines around our grids.
            for (int i = 0; i < m_CellCount; ++i)
            {
                float z = this.transform.position.z;
                m_Cells[i].DebugDrawCell(CellExtends, z);
            }
        }
    }

    void CalculateCells()
    {
        //ensure it is empty before we create the cells.
        for (int x = 0; x < CellCountX; ++x)
        {
            for (int y = 0; y < CellCountY; ++y)
            {
                double xPosition = (CellExtends.x * x) + CellExtends.x * 0.5;
                double yPosition = (CellExtends.y * y) + CellExtends.y * 0.5;

                Vector3 cellCenterPoint = new Vector2((float)xPosition, (float)yPosition);
                //Debug.LogFormat("New Positions {0} , {1}", cellCenterPoint.x, cellCenterPoint.y);

                cellCenterPoint += this.transform.position;
                int index = CellCooridnatesToIndex(x, y);
                m_Cells[index].CellPosition = cellCenterPoint;
                m_Cells[index].CellColumn = x;
                m_Cells[index].CellRow = y;
            }
        }
    }

    public int CellCooridnatesToIndex(int x, int y)
    {
        return x + (CellCountX * y);
    }

    public int IndexToColumn(int Index)
    {
        int Remainder = (Index - 1) % CellCountX;
        return Remainder - 1;
        // TODO check this function


    }

    public Vector2 GetGridPosition(int x, int y)
    {
        int index = CellCooridnatesToIndex(x, y);
        return m_Cells[index].CellPosition;
    }

    public CellItem GetGridCell(int x, int y)
    {
        int index = CellCooridnatesToIndex(x, y);

        Debug.LogFormat("Get Grid Cell {0},{1} at index {2}", x, y, index);
        return m_Cells[index];
    }

    public int GetRowLength()
    {
        return CellCountX;
    }
    public int GetColCount()
    {
        return CellCountX;
    }

    public int GetColLength()
    {
        return CellCountY;
    }
    public int GetRowCount()
    {
        return CellCountY;
    }

    public CellItem GetGridCell(int index)
    {
        return m_Cells[index];
    }

    public bool IsRowComplete(int row, int playerid, int consecutiveCount)
    {
        int foundPlayeridCount = 0;
        int numberOfColumns = GetColCount();
        for (int i = 0; i < numberOfColumns; i++)
        {
            //Y is the row we are investigating
            Gem gem = GetGridCell(i, row).m_CurrentObject.GetComponent<Gem>();
            if (gem.GetPlayerId() == playerid)
            {
                foundPlayeridCount++;
                
            }
            else if(foundPlayeridCount < consecutiveCount)
            {
                foundPlayeridCount = 0;

            }
        }
        return foundPlayeridCount >= consecutiveCount;
    }

    public bool IsColComplete(int col, int playerid, int consecutiveCount)
    {
        int foundPlayeridCount = 0;
        int numberOfRows = GetRowCount();
        for (int i = 0; i < numberOfRows; i++)
        {
            //X is the column we are investigating
            Gem gem = GetGridCell(col, i).m_CurrentObject.GetComponent<Gem>();
            if (gem.GetPlayerId() == playerid)
            {
                foundPlayeridCount++;

            }
            else if (foundPlayeridCount < consecutiveCount)
            {
                foundPlayeridCount = 0;

            }
        }
        return foundPlayeridCount >= consecutiveCount;
    }

    public bool IsDiagonalComplete_Internal(CellItem startIndex, CellItem endIndex, int playerid, int consecutiveCount)
    {
        //ensure diagonal
        if (Debug.isDebugBuild)
        {
            UnityEngine.Assertions.Assert.AreNotEqual(startIndex.CellColumn, endIndex.CellColumn);
            UnityEngine.Assertions.Assert.AreNotEqual(startIndex.CellRow, endIndex.CellRow);
        }

        //pick increment direction and starting values
        CellItem leftMostIndex = null;
        CellItem rightMostIndex = null;
        if (startIndex.CellColumn < endIndex.CellColumn)
        {
            leftMostIndex = startIndex;
            rightMostIndex = endIndex;
        }
        else
        {
            leftMostIndex = endIndex;
            rightMostIndex = startIndex;
        }
        int persistentColIncrement = leftMostIndex.CellRow; // current Y location moving through iterations
        int gridIncrement = leftMostIndex.CellRow < rightMostIndex.CellRow ? 1 : -1;

        int foundPlayeridCount = 0;

        //moves through the effected columns
        for (int i = startIndex.CellColumn; i != endIndex.CellColumn; ++i)
        {
            //X is the column we are investigating
            Gem gem = GetGridCell(i, persistentColIncrement).m_CurrentObject.GetComponent<Gem>();
            if (gem.GetPlayerId() == playerid)
            {
                foundPlayeridCount++;

            }
            else if (foundPlayeridCount < consecutiveCount)
            {
                foundPlayeridCount = 0;
            }

            persistentColIncrement += gridIncrement;
        }

        return foundPlayeridCount >= consecutiveCount;
    }

    public int CellsBetweenPoints(CellItem a, CellItem b, bool inclusive)
    {
        int plusSelfs = inclusive ? 2 : 0;
        return System.Math.Abs(a.CellColumn - b.CellColumn) + plusSelfs;
    }

    public CellItem GetCellForCorner(CellItem initialPoint, Corners corner)
    {
        //inverse because we iterate to the other ends
        int xIncrmenet = corner == Corners.BL || corner == Corners.TL ? -1 : 1;
        int yIncrmenet = corner == Corners.BL || corner == Corners.BR ? -1 : 1;

        CellItem foundCell = initialPoint;
        int nextIterationX = foundCell.CellColumn + xIncrmenet;
        int nextIterationY = foundCell.CellRow + yIncrmenet;

        //Check still in array bounds
        while (nextIterationX >= 0 && nextIterationX < GetRowLength()
               && nextIterationY >= 0 && nextIterationY < GetColLength())
        {
            //iterate the item
            foundCell = GetGridCell(nextIterationX, nextIterationY);

            nextIterationX = foundCell.CellColumn + xIncrmenet;
            nextIterationY = foundCell.CellRow + yIncrmenet;
        }
        return foundCell;
    }

    public bool IsDiagonalComplete(CellItem selectedIndex, int playerId, int consecutiveCount)
    {
        CellItem topLeft = GetCellForCorner(selectedIndex, Corners.TL);
        CellItem bottomRight = GetCellForCorner(selectedIndex, Corners.BR); ;
        if (CellsBetweenPoints(topLeft, bottomRight, true) >= consecutiveCount)
        {
            if (IsDiagonalComplete_Internal(topLeft, bottomRight, playerId, consecutiveCount))
            {
                return true;
            }
        }

        CellItem bottomLeft = GetCellForCorner(selectedIndex, Corners.BL);
        CellItem topRight = GetCellForCorner(selectedIndex, Corners.TR);
        if (CellsBetweenPoints(bottomLeft, topRight, true) >= consecutiveCount)
        {
            if (IsDiagonalComplete_Internal(bottomLeft, topRight, playerId, consecutiveCount))
            {
                return true;
            }
        }

        return false;
    }
}

