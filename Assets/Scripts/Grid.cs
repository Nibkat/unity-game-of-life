using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    [Header("Grid")]
    [SerializeField]
    private GameObject cell;

    [SerializeField]
    private Vector2 gridSize = new Vector2(4, 4);
    public Cell[,] cellArray;

    private GameObject grid;

    [Header("Simulation")]
    [SerializeField]
    private bool simulate = false;
    [SerializeField]
    private float simulationSpeed = 1f;
    [SerializeField]
    private Slider simulationSpeedSlider;
    private int generations;
    [SerializeField]
    private Text generationText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateGrid();
        StartCoroutine(Simulate());
    }

    public void GenerateGrid()
    {
        if (grid != null)
        {
            Destroy(grid);
        }

        Transform gridParent = new GameObject("Grid Parent").transform;

        Vector2 cellSize = new Vector2(cell.GetComponent<SpriteRenderer>().bounds.size.x, cell.GetComponent<SpriteRenderer>().bounds.size.y);

        cellArray = new Cell[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellArray[x, y] = Instantiate(cell, new Vector3(-(cellSize.x * gridSize.x / 2) + cellSize.x * x, -(cellSize.y * gridSize.y / 2) + cellSize.y * y, 0), Quaternion.identity).GetComponent<Cell>();
                cellArray[x, y].transform.SetParent(gridParent);

                cellArray[x, y].cellPosition = new Vector2(x, y);

                cellArray[x, y].name = "Cell " + x + " - " + y;
            }
        }

        grid = gridParent.gameObject;

        generations = 0;
        generationText.text = "Generation: " + generations + "\n------------------------------";
    }

    public void NextGeneration()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (cellArray[x, y].Alive)
                {
                    cellArray[x, y].tempAlive = CountCellNeighbors(x, y) == 2 || CountCellNeighbors(x, y) == 3;
                }
                else
                {
                    cellArray[x, y].tempAlive = CountCellNeighbors(x, y) == 3;
                }
            }
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellArray[x, y].SetAlive(cellArray[x, y].tempAlive);
            }
        }

        generations++;
        generationText.text = "Generation: " + generations + "\n------------------------------";
    }

    private IEnumerator Simulate()
    {
        while (true)
        {
            if (simulate)
            {
                NextGeneration();
            }

            yield return new WaitForSeconds(simulationSpeed);
        }
    }

    public void RandomizeGrid()
    {
        generations = 0;
        generationText.text = "Generation: " + generations + "\n------------------------------";

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellArray[x, y].SetAlive(Random.Range(0, 2) == 0);
            }
        }
    }

    public void SetSimulationSpeed()
    {
        simulationSpeed = simulationSpeedSlider.value;

        Debug.Log("Simulation speed: " + simulationSpeed);
    }

    public void ToggleSimulate()
    {
        simulate = !simulate;
    }

    private int CountCellNeighbors(int x, int y)
    {
        int neighbors = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                try
                {
                    if (cellArray[x + i, y + j].Alive && cellArray[x + i, y + j] != cellArray[x, y])
                    {
                        neighbors++;
                    }
                }
                catch { }
            }
        }

        return neighbors;
    }
}
