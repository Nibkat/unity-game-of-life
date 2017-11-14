using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Vector2 cellPosition = Vector2.zero;

    private bool alive;
    public bool tempAlive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseUp()
    {
        SetAlive(!alive);
    }

    public void SetAlive(bool value)
    {
        alive = value;

        if (alive)
        {
            spriteRenderer.color = Color.black;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public bool Alive
    {
        get
        {
            return alive;
        }
    }
}
