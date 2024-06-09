using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum WallDirection
{
    Left = 1,
    Right = 2,
    Up = 3,
    Down = 4
}

public class MazeGenerator : MonoBehaviour
{

    #region Variables:
    
    [Header("Maze generation values:")]
    [Tooltip("How many cells tall is the maze. MUST be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum  value of 4.")]
    public static int mazeRows;
    [Tooltip("How many cells wide is the maze. Must be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum value of 4.")]
    public static int mazeColumns;

    [Header("Maze object variables:")]
    [Tooltip("Cell prefab object.")]
    [SerializeField]
    private List<GameObject> cellPrefabs;

    [Tooltip("If you want to disable the main sprite so the cell has no background, set to TRUE. This will create a maze with only walls.")]
    public bool disableCellSprite;

    public List<Vector2> availablePositions = new List<Vector2>();


    // Variable to store size of centre room. Hard coded to be 2.
    //private int centreSize = 2;

    // Dictionary to hold and locate all cells in maze.
    private Dictionary<Vector2, Cell> allCells = new Dictionary<Vector2, Cell>();
    public Dictionary<Vector2, Cell> AllCells
    {
        get { return allCells; }
    }

    // List to hold unvisited cells.
    private List<Cell> unvisited = new List<Cell>();
    // List to store 'stack' cells, cells being checked during generation.
    private List<Cell> stack = new List<Cell>();

    // Array will hold 4 centre room cells, from 0 -> 3 these are:
    // Top left (0), top right (1), bottom left (2), bottom right (3).
    private Cell[] centreCells = new Cell[4];

    // Cell variables to hold current and checking Cells.
    private Cell currentCell;
    private Cell checkCell;

    // Array of all possible neighbour positions.
    private Vector2[] neighbourPositions = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    public Vector2 exitPosition;  // Public so you can access it from other scripts if needed.
    [SerializeField]
    private GameObject exitPrefab;  // Reference to the exit prefab

    // Size of the cells, used to determine how far apart to place cells during generation.
    private float cellSize;

    private GameObject mazeParent;
    private List<Vector2> positions;
    #endregion

    public void GenerateMaze(int rows, int columns)
    {
        if (mazeParent != null) DeleteMaze();

        mazeRows = rows;
        mazeColumns = columns;
        CreateLayout();

      
    }

    public void ResetMazeSizeToDefault()
    {
        mazeRows = 2;
        mazeColumns = 2;
    }
    public Vector3 GetStartPosition()
    {
        // 返回中心房间的其中一个单元格的位置作为起始点
        // 这里假设 centreCells[0] 是中心房间的一个角落
        return centreCells[0].cellObject.transform.position;
    }

    // Creates the grid of cells.
    public void CreateLayout()
    {
        InitValues();

        // Initialize the positions list to ensure it's not null
        positions = new List<Vector2>();

        // Select a random prefab at the start of maze generation
        GameObject selectedPrefab = cellPrefabs[Random.Range(0, cellPrefabs.Count)];
        cellSize = selectedPrefab.transform.localScale.x;

        // Set starting point, set spawn point to start.
        Vector2 startPos = new Vector2(-(cellSize * (mazeColumns / 2)) + (cellSize / 2), -(cellSize * (mazeRows / 2)) + (cellSize / 2));
        Vector2 spawnPos = startPos;

        for (int x = 1; x <= mazeColumns; x++)
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                GenerateCell(spawnPos, new Vector2(x, y), selectedPrefab);

                // Store the position for use in other parts of the game
                positions.Add(spawnPos);

                // Increase spawnPos y.
                spawnPos.y += cellSize;
            }

            // Reset spawnPos y and increase spawnPos x.
            spawnPos.y = startPos.y;
            spawnPos.x += cellSize;
        }

        CreateCentre();
        RunAlgorithm();
        MakeExit();

        // Now initialize PositionManager with the filled positions list
        if (PositionManager.Instance != null)
        {
            PositionManager.Instance.Initialize(positions);
        }
        else
        {
            Debug.LogError("PositionManager instance is not found or not initialized correctly.");
        }
    }

    // This is where the fun stuff happens.
    public void RunAlgorithm()
    {
        // Get start cell, make it visited (i.e. remove from unvisited list).
        unvisited.Remove(currentCell);

        // While we have unvisited cells.
        while (unvisited.Count > 0)
        {
            List<Cell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);
            if (unvisitedNeighbours.Count > 0)
            {
                // Get a random unvisited neighbour.
                checkCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                // Add current cell to stack.
                stack.Add(currentCell);
                // Compare and remove walls.
                CompareWalls(currentCell, checkCell);
                // Make currentCell the neighbour cell.
                currentCell = checkCell;
                // Mark new current cell as visited.
                unvisited.Remove(currentCell);
            }
            else if (stack.Count > 0)
            {
                // Make current cell the most recently added Cell from the stack.
                currentCell = stack[stack.Count - 1];
                // Remove it from stack.
                stack.Remove(currentCell);
            }
        }
    }

    public void MakeExit()
{
    List<Cell> edgeCells = new List<Cell>();
    foreach (KeyValuePair<Vector2, Cell> cell in allCells)
    {
        if (cell.Key.x == 0 || cell.Key.x == mazeColumns || cell.Key.y == 0 || cell.Key.y == mazeRows)
        {
            edgeCells.Add(cell.Value);
        }
    }

    Cell exitCell = edgeCells[Random.Range(0, edgeCells.Count)];
    exitPosition = exitCell.gridPos;  // Store the exit position.
    exitCell.isExit = true;  // Mark this cell as the exit

    WallDirection wallToRemove;
    if (exitCell.gridPos.x == 0) { wallToRemove = WallDirection.Left; }
    else if (exitCell.gridPos.x == mazeColumns) { wallToRemove = WallDirection.Right; }
    else if (exitCell.gridPos.y == mazeRows) { wallToRemove = WallDirection.Up; }
    else { wallToRemove = WallDirection.Down; }

    RemoveWall(exitCell.cScript, wallToRemove);

        // Instantiate the exit prefab if needed and if assigned
        if (exitPrefab != null && exitCell.isExit)
        {
            // Adjust the prefab position to match the maze grid positioning
            Vector3 prefabPosition = new Vector3(exitCell.gridPos.x * cellSize, exitCell.gridPos.y * cellSize, 0);
            prefabPosition -= new Vector3((mazeColumns * cellSize) / 2, (mazeRows * cellSize) / 2, 0); // Center the maze
            Instantiate(exitPrefab, prefabPosition, Quaternion.identity, mazeParent.transform);  // Parent to maze for organization
        }


        Debug.Log("Maze generation finished. Exit created at: " + exitPosition);
}



    public List<Cell> GetUnvisitedNeighbours(Cell curCell)
    {
        // Create a list to return.
        List<Cell> neighbours = new List<Cell>();
        // Create a Cell object.
        Cell nCell = curCell;
        // Store current cell grid pos.
        Vector2 cPos = curCell.gridPos;

        foreach (Vector2 p in neighbourPositions)
        {
            // Find position of neighbour on grid, relative to current.
            Vector2 nPos = cPos + p;
            // If cell exists.
            if (allCells.ContainsKey(nPos)) nCell = allCells[nPos];
            // If cell is unvisited.
            if (unvisited.Contains(nCell)) neighbours.Add(nCell);
        }

        return neighbours;
    }

    // Compare neighbour with current and remove appropriate walls.
    public void CompareWalls(Cell cCell, Cell nCell)
    {
        if (nCell.gridPos.x < cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, WallDirection.Right);
            RemoveWall(cCell.cScript, WallDirection.Left);
        }
        else if (nCell.gridPos.x > cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, WallDirection.Left);
            RemoveWall(cCell.cScript, WallDirection.Right);
        }
        else if (nCell.gridPos.y > cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, WallDirection.Down);
            RemoveWall(cCell.cScript, WallDirection.Up);
        }
        else if (nCell.gridPos.y < cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, WallDirection.Up);
            RemoveWall(cCell.cScript, WallDirection.Down);
        }
    }


    // Function disables wall of your choosing, pass it the script attached to the desired cell
    // and a direction, where the direction is the enum value.
    public void RemoveWall(CellScript cScript, WallDirection wallID)
    {
        switch (wallID)
        {
            case WallDirection.Left:
                cScript.wallL.SetActive(false);
                break;
            case WallDirection.Right:
                cScript.wallR.SetActive(false);
                break;
            case WallDirection.Up:
                cScript.wallU.SetActive(false);
                break;
            case WallDirection.Down:
                cScript.wallD.SetActive(false);
                break;
        }
    }


    public void CreateCentre()
    {
        // Initialize the center cells based on the maze dimensions
        centreCells[0] = allCells[new Vector2(mazeColumns / 2, mazeRows / 2 + 1)];
        centreCells[1] = allCells[new Vector2(mazeColumns / 2 + 1, mazeRows / 2 + 1)];
        centreCells[2] = allCells[new Vector2(mazeColumns / 2, mazeRows / 2)];
        centreCells[3] = allCells[new Vector2(mazeColumns / 2 + 1, mazeRows / 2)];

        // Remove walls to create a single room. Using enum for clarity.
        RemoveWall(centreCells[0].cScript, WallDirection.Down);
        RemoveWall(centreCells[0].cScript, WallDirection.Right);
        RemoveWall(centreCells[1].cScript, WallDirection.Down);
        RemoveWall(centreCells[1].cScript, WallDirection.Left);
        RemoveWall(centreCells[2].cScript, WallDirection.Up);
        RemoveWall(centreCells[2].cScript, WallDirection.Right);
        RemoveWall(centreCells[3].cScript, WallDirection.Up);
        RemoveWall(centreCells[3].cScript, WallDirection.Left);

        // Randomly select one of the center cells to be the entrance/exit.
        List<int> rndList = new List<int> { 0, 1, 2, 3 };
        int startCellIndex = Random.Range(0, rndList.Count);
        currentCell = centreCells[rndList[startCellIndex]];
        rndList.RemoveAt(startCellIndex);

        // Ensure the other center cells do not connect to the maze.
        foreach (int index in rndList)
        {
            unvisited.Remove(centreCells[index]);
        }
    }

    public void GenerateCell(Vector2 pos, Vector2 keyPos, GameObject prefab)
    {
        // Create new Cell object.
        Cell newCell = new Cell();

        // Store reference to position in grid.
        newCell.gridPos = keyPos;
        // Set and instantiate cell GameObject using the given prefab.
        newCell.cellObject = Instantiate(prefab, pos, prefab.transform.rotation);
        // Child new cell to parent.
        if (mazeParent != null) newCell.cellObject.transform.parent = mazeParent.transform;
        // Set name of cellObject.
        newCell.cellObject.name = "Cell - X:" + keyPos.x + " Y:" + keyPos.y;
        // Get reference to attached CellScript.
        newCell.cScript = newCell.cellObject.GetComponent<CellScript>();
        // Disable Cell sprite, if applicable.
        if (disableCellSprite) newCell.cellObject.GetComponent<SpriteRenderer>().enabled = false;

        // Add to Lists.
        allCells[keyPos] = newCell;
        unvisited.Add(newCell);

        availablePositions.Add(new Vector2(pos.x, pos.y));
    }

    public void DeleteMaze()
    {
        if (mazeParent != null) Destroy(mazeParent);
    }

    public void InitValues()
    {
        // Check generation values to prevent generation failing.
        if (IsOdd(mazeRows)) mazeRows--;
        if (IsOdd(mazeColumns)) mazeColumns--;

        if (mazeRows <= 3) mazeRows = 4;
        if (mazeColumns <= 3) mazeColumns = 4;

        GameObject cellPrefab = cellPrefabs[Random.Range(0, cellPrefabs.Count)];
        // Determine size of cell using localScale.
        cellSize = cellPrefab.transform.localScale.x;

        // Create an empty parent object to hold the maze in the scene.
        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";
    }


    public bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    public class Cell
    {
        public Vector2 gridPos;
        public GameObject cellObject;
        public CellScript cScript;
        public bool isExit = false; 
    }

    
}