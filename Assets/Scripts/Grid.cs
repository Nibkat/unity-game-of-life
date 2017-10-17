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
    public GameObject[,] cellArray;

    private GameObject grid;

    [Header("Simulation")]
    [SerializeField]
    private bool simulate = false;
    [SerializeField]
    private float simulationSpeed = 1f;
    [SerializeField]
    private Slider simulationSpeedSlider;
    [SerializeField]
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

        cellArray = new GameObject[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellArray[x, y] = Instantiate(cell, new Vector3(-(cellSize.x * gridSize.x / 2) + cellSize.x * x, -(cellSize.y * gridSize.y / 2) + cellSize.y * y, 0), Quaternion.identity);
                cellArray[x, y].transform.SetParent(gridParent);
                
                cellArray[x, y].GetComponent<Cell>().cellPosition = new Vector2(x, y);

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
                if (cellArray[x, y].GetComponent<Cell>().Alive)
                {
                    if (cellArray[x, y].GetComponent<Cell>().CountNeighbors == 3 || cellArray[x, y].GetComponent<Cell>().CountNeighbors == 2)
                    {
                        cellArray[x, y].GetComponent<Cell>().tempAlive = true;
                    }
                    else
                    {
                        cellArray[x, y].GetComponent<Cell>().tempAlive = false;
                    }
                }
                else
                {
                    if (cellArray[x, y].GetComponent<Cell>().CountNeighbors == 3)
                    {
                        cellArray[x, y].GetComponent<Cell>().tempAlive = true;
                    }
                    else
                    {
                        cellArray[x, y].GetComponent<Cell>().tempAlive = false;
                    }
                }
            }
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellArray[x, y].GetComponent<Cell>().SetAlive(cellArray[x, y].GetComponent<Cell>().tempAlive);
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
                cellArray[x, y].GetComponent<Cell>().SetAlive(Random.Range(0, 2) == 0);
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
}
