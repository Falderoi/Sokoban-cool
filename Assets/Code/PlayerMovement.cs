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
                if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Floor
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Switch)
                {
                    position = desiredPosition;
                    transform.position = new Vector2(position.x, -position.y);
                    // Cas boite
                }
                else if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Box
                  || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.SwitchandBox)
                {
                    Vector2Int desiredBoxPosition = desiredPosition + (desiredPosition - position);
                    TileType desiredBoxTileType = boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x];
                    if (desiredBoxPosition.x <= 9
                        && desiredBoxPosition.y <= 9
                        && desiredBoxPosition.x >= 0
                        && desiredBoxPosition.y >= 0
                        && (desiredBoxTileType == TileType.Floor || desiredBoxTileType == TileType.Switch))
                    {
                        position = desiredPosition;
                        transform.position = new Vector2(position.x, -position.y);

                        // Transformation case de départ
                        if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Box) // Boite simple
                            boardManager.board[desiredPosition.y, desiredPosition.x] = TileType.Floor;
                        else // Boite était sur un switch
                            boardManager.board[desiredPosition.y, desiredPosition.x] = TileType.Switch;

                        // Transformation case d'arrivée
                        if (desiredBoxTileType == TileType.Floor) // Je pousse la boite vers un sol
                            boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] = TileType.Box;
                        else // Je pousse la boite vers un switch
                            boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] = TileType.SwitchandBox;

                        boardManager.UpdateVisuals();
                        CheckWinCondition();
                    }
                }
            }
        }
    }


    private void CheckWinCondition()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (boardManager.board[row, col] == TileType.Switch)
                    return;
            }

        }
        Debug.Log("victoire !");
    }
}