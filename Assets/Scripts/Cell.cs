using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spr;

    public Vector2 cellPosition = Vector2.zero;

    private bool alive;
    public bool tempAlive;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseUp()
    {
        SetAlive(!alive);

        print("Alive neighbors: " + CountNeighbors);
    }

    public void SetAlive(bool value)
    {
        alive = value;

        if (alive)
        {
            spr.color = Color.black;
        }
        else
        {
            spr.color = Color.white;
        }
    }

    public bool Alive
    {
        get
        {
            return alive;
        }
    }

    public int CountNeighbors
    {
        get
        {
            int neighbors = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    try
                    {
                        if (Grid.instance.cellArray[(int)cellPosition.x + x, (int)cellPosition.y + y].GetComponent<Cell>().alive && Grid.instance.cellArray[(int)cellPosition.x + x, (int)cellPosition.y + y].GetComponent<Cell>() != this)
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
}
