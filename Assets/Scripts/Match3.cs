using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Search;

public static class Match3Config
{
    public const int MinPopNum = 3;
}

public class Match3 : MonoBehaviour
{
    // Components
    public Match3Grid<Vegetable> grid;
    private Camera mainCamera;
    [SerializeField] private InputReader inputReader;

    // Elements
    [SerializeField] private  VegetableSetData vegetableSetData;
    private SerializedDictionary<VegetableType, GameObject> VegetableList;
    private Dictionary<VegetableType, ObjectPool> VegetableObjPool = new Dictionary<VegetableType, ObjectPool>();
    [SerializeField] private GameObject highlighter;

    // Swap
    private Vector2Int? swapTarget;

    // DFS
    private bool[,] visited;
    private Stack<Vector2Int> DFSStack = new Stack<Vector2Int>(10);
    private List<Vector2Int> PopList = new List<Vector2Int>(10);
    private Vector2Int[] adjVector = { Vector2Int.down, Vector2Int.right, Vector2Int.left, Vector2Int.up };

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        mainCamera = Camera.main;
        //GameManager.OnGameStateChanged += OnGameStateChanged;
        inputReader.Click += OnSelectVeg;
        VegetableList = vegetableSetData.VegetableList;

        grid = new Match3Grid<Vegetable>(Match3GridConfig.Width,
            Match3GridConfig.Height,
            Match3GridConfig.CellSize,
            new Vector3(Match3GridConfig.PivotX, Match3GridConfig.PivotY, 0));
        //grid.DrawDebugLines();

        foreach (KeyValuePair<VegetableType, GameObject> veg in VegetableList)
        {
            VegetableObjPool.Add(veg.Key, new ObjectPool(veg.Value, 30));
        }

        visited = new bool[Match3GridConfig.Width, Match3GridConfig.Height];

        highlighter = Instantiate(highlighter);
        highlighter.gameObject.SetActive(false);
    }

    private void Start()
    {
        FillEmpty();
    }

    private void Update()
    {
        // If Selected Vegetable gets riped on Select, Deselect
        if (swapTarget != null && grid.gridArray[swapTarget.Value.x, swapTarget.Value.y].state == VegetableState.Riped)
        {
            swapTarget = null;
            highlighter.SetActive(false);
        }

        FillEmpty();

        if (GameManager.Instance.gameState == GameState.Playing && NoMatch3Check())
        {
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

    // Not used
    //private void OnGameStateChanged(GameState state)
    //{
    //    //switch (state)
    //    //{
    //    //    //case GameState.Playing:
    //    //    //    break;
    //    //    case GameState.Caught:
    //    //        OnCaught();
    //    //        break;
    //    //    case GameState.GameOver:
    //    //        OnGameOver();
    //    //        break;
    //    //    default:
    //    //        break;
    //    //}
    //}
    //private void OnDestroy()
    //{
    //    GameManager.OnGameStateChanged -= OnGameStateChanged;
    //}

    private void OnSelectVeg()
    {
        Vector2Int? nullableGridPos = grid.WorldToGrid(mainCamera.ScreenToWorldPoint(inputReader.Selected));
        
        if (nullableGridPos == null)
        {
            return;
        }

        Vector2Int gridPos = nullableGridPos.Value;
        Vegetable clickedVeg = grid.gridArray[gridPos.x, gridPos.y];

        // Clicked Invalid Position
        if (clickedVeg == null)
        {
            return;
        }
        // If Catcher is Watching but clicked
        else if (GameManager.Instance.catcher.state == CatcherState.Watching)
        {
            GameManager.Instance.UpdateGameState(GameState.Caught);
            return;
        }
        // Clicked Riped
        else if (clickedVeg.state == VegetableState.Riped)
        {
            // Not Missed Click
            if (swapTarget == null)
            {
                HarvestTarget(gridPos);
            }
        }
        // CLicked Not Riped
        else
        {
            // First Selection
            if (swapTarget == null)
            {
                SelectSwapTarget(gridPos);
            }
            // Clicked Again
            else if (swapTarget.Value == gridPos)
            {
                DeselectSwapTarget(gridPos);
            }
            // Second Selection
            else
            {
                SwapTarget(swapTarget.Value, gridPos);
            }
        }
    }

    private void SelectSwapTarget(Vector2Int targetA)
    {
        swapTarget = targetA;

        highlighter.SetActive(true);
        highlighter.transform.position = grid.GridToWorld(targetA.x, targetA.y, GridPoint.Center);
        SoundManager.Instance.PlaySound(SoundType.dig);
    }

    private void DeselectSwapTarget(Vector2Int targetA)
    {
        swapTarget = null;
        highlighter.SetActive(false);
        SoundManager.Instance.PlaySound(SoundType.dig);
    }

    private void SwapTarget(Vector2Int targetA, Vector2Int targetB)
    {
        if (grid.IsAdjacent(targetA, targetB))
        {
            var tempA = grid.gridArray[targetA.x, targetA.y];
            var tempAPos = grid.gridArray[targetA.x, targetA.y].transform.position;

            var tempB = grid.gridArray[targetB.x, targetB.y];
            var tempBPos = grid.gridArray[targetB.x, targetB.y].transform.position;

            grid.gridArray[targetA.x, targetA.y] = tempB;
            grid.gridArray[targetB.x, targetB.y] = tempA;

            grid.gridArray[targetA.x, targetA.y].transform.position = tempAPos;
            grid.gridArray[targetB.x, targetB.y].transform.position = tempBPos;
            
            SoundManager.Instance.PlaySound(SoundType.dig);
        }

        highlighter.SetActive(false);
        swapTarget = null;
    }

    private void HarvestTarget(Vector2Int targetA)
    {
        ResetDFS();

        VegetableType type = grid.gridArray[targetA.x, targetA.y].type;
        DFSStack.Push(targetA);

        while (DFSStack.Count > 0)
        {
            Vector2Int current = DFSStack.Pop();

            if (!visited[current.x, current.y])
            {
                visited[current.x, current.y] = true;
                PopList.Add(current);

                foreach (Vector2Int dir in adjVector)
                {
                    Vector2Int temp = current + dir;

                    if (grid.IsValidPos(temp)
                        && grid.gridArray[temp.x, temp.y] != null
                        && !visited[temp.x, temp.y]
                        && grid.gridArray[temp.x, temp.y].type == type
                        && grid.gridArray[temp.x, temp.y].state == VegetableState.Riped)
                    {
                        DFSStack.Push(temp);                        
                    }
                }
            }
        }

        // Not enough vegetables
        if (PopList.Count < Match3Config.MinPopNum)
        {
            return;
        }
        else
        {
            GameManager.Instance.AddScore(PopList.Count * PopList.Count * 10);
            foreach (Vector2Int popItem in PopList)
            {
                DestroyElement(popItem.x, popItem.y);
            }
            SoundManager.Instance.PlaySound(SoundType.harvest);
        }
    }

    private bool NoMatch3Check()
    {
        // Check Every Vegetable is riped
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                Vector2Int temp = new Vector2Int(i, j);
                if (grid.gridArray[temp.x, temp.y] is null
                    || grid.gridArray[temp.x, temp.y].state != VegetableState.Riped)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                // DFS for Possible Match3
                ResetDFS();

                VegetableType type = grid.gridArray[i, j].type;
                DFSStack.Push(new Vector2Int(i, j));

                while (DFSStack.Count > 0)
                {
                    Vector2Int current = DFSStack.Pop();

                    if (!visited[current.x, current.y])
                    {
                        visited[current.x, current.y] = true;
                        PopList.Add(current);

                        foreach (Vector2Int dir in adjVector)
                        {
                            Vector2Int temp = current + dir;

                            if (grid.IsValidPos(temp)
                                && grid.gridArray[temp.x, temp.y] is not null
                                && !visited[temp.x, temp.y]
                                && grid.gridArray[temp.x, temp.y].type == type
                                && grid.gridArray[temp.x, temp.y].state == VegetableState.Riped)
                            {
                                DFSStack.Push(temp);
                            }
                        }
                    }

                    if (PopList.Count >= Match3Config.MinPopNum)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void ResetDFS()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                visited[i, j] = false;
            }
        }

        DFSStack.Clear();
        PopList.Clear();
    }

    private void FillEmpty()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                if (grid.gridArray[i, j] is null)
                {
                    SetElement(i, j, (VegetableType) UnityEngine.Random.Range(0, VegetableConfig.VegNum));
                }
            }
        }
    }

    // Spawn/Destroy Vegetable in Grid
    private void SetElement(int x, int y, VegetableType type)
    {
        var veg = VegetableObjPool[type].GetObject();
        veg.transform.position = grid.GridToWorld(x, y, GridPoint.Center);
        grid.gridArray[x, y] = veg.GetComponent<Vegetable>();
    }

    private void DestroyElement(int x, int y)
    {
        Vegetable veg = grid.gridArray[x, y];
        veg.Pop();
        VegetableObjPool[veg.type].ReturnObject(veg.gameObject);
        grid.gridArray[x, y] = null;
    }
}
