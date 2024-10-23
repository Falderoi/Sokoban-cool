using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    private Vector2Int position;

    public void Init(Vector2Int startPosition)
    {
        position = startPosition;
        transform.position = new Vector2(position.x, -position.y);
    }

    private void Update()
    {
        Vector2Int desiredPosition = position;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            desiredPosition = new Vector2Int(position.x, position.y - 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            desiredPosition = new Vector2Int(position.x, position.y + 1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            desiredPosition = new Vector2Int(position.x - 1, position.y);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            desiredPosition = new Vector2Int(position.x + 1, position.y);

        if (desiredPosition != position)
        {
            if (desiredPosition.x <= 9
                && desiredPosition.y <= 9
                && desiredPosition.x >= 0
                && desiredPosition.y >= 0)
            {
                // Cas sol
                if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Floor)
                {
                    position = desiredPosition;
                    transform.position = new Vector2(position.x, -position.y);
                    // Cas boite
                }
                else if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Box)
                {
                    Vector2Int desiredBoxPosition = desiredPosition + (desiredPosition - position);
                    if (desiredBoxPosition.x <= 9
                        && desiredBoxPosition.y <= 9
                        && desiredBoxPosition.x >= 0
                        && desiredBoxPosition.y >= 0
                        && boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] == TileType.Floor)
                    {
                        position = desiredPosition;
                        transform.position = new Vector2(position.x, -position.y);
                        boardManager.board[desiredPosition.y, desiredPosition.x] = TileType.Floor;
                        boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] = TileType.Box;
                        boardManager.UpdateVisuals();
                    }
                }
            }
        }
    }
}