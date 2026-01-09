using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool wasJustClicked = true;
    bool canMove;

    Rigidbody2D rb;
    public Transform boundaryHolder;
    Boundary playerBoundary;
    Collider2D playerCollider;

    Vector2 startPosition;

    struct Boundary
    {
        public float Up, Down, Left, Right;
        public Boundary(float up, float down, float left, float right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        startPosition = rb.position;

        playerBoundary = new Boundary(
            boundaryHolder.GetChild(0).position.y,
            boundaryHolder.GetChild(1).position.y,
            boundaryHolder.GetChild(2).position.x,
            boundaryHolder.GetChild(3).position.x
        );
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (wasJustClicked)
            {
                wasJustClicked = false;
                canMove = playerCollider.OverlapPoint(mousePos);
            }

            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(
                    Mathf.Clamp(mousePos.x, playerBoundary.Left, playerBoundary.Right),
                    Mathf.Clamp(mousePos.y, playerBoundary.Down, playerBoundary.Up)
                );
                rb.MovePosition(clampedMousePos);
            }
        }
        else
        {
            wasJustClicked = true;
        }
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = startPosition;
    }
}
