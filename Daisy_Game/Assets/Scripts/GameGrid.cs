using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A wrapper class that holds information about a cell.
public class CellItem
{
    private Vector2 m_CellPosition;

    public Vector2 CellPosition
    {
        get { return m_CellPosition; }
        set { m_CellPosition = value; }
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

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    bool CenterGrid = true;

    [SerializeField]
    float MaximumWidth = 600.0f;
    [SerializeField]
    float MaximumHeight = 600.0f;

    [SerializeField]
    int CellCountX = 12;
    [SerializeField]
    int CellCountY = 12;

    CellItem[] m_Cells;

    Vector2 CellExtends;
    int m_CellCount;

    void Awake()
    {
        if(CenterGrid)
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
        for(int x = 0; x < CellCountX; ++x)
        {
            for (int y = 0; y < CellCountY; ++y)
            {
                double xPosition = (CellExtends.x * x) + CellExtends.x * 0.5;
                double yPosition = (CellExtends.y * y) + CellExtends.y * 0.5;

                Vector3 cellCenterPoint = new Vector2((float)xPosition, (float)yPosition);
                //Debug.LogFormat("New Positions {0} , {1}", cellCenterPoint.x, cellCenterPoint.y);

                cellCenterPoint += this.transform.localPosition;
                int index = CellCooridnatesToIndex(x, y);
                m_Cells[index].CellPosition = cellCenterPoint;
            }
        }
    }

    int CellCooridnatesToIndex(int x, int y)
    {
        return x + (CellCountX * y);
    }

    public Vector2 GetGridPosition(int x, int y)
    {
        int index = CellCooridnatesToIndex(x, y);
        return m_Cells[index].CellPosition;
    }
}
