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
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Switch
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Teleporter) // Permettre le déplacement vers le téléporteur
                {
                    position = desiredPosition;
                    transform.position = new Vector2(position.x, -position.y);

                    // Vérifie si le joueur est sur un téléporteur pour activer la téléportation
                    if (boardManager.board[position.y, position.x] == TileType.Teleporter)
                    {
                        TeleportPlayer();
                    }
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

                        if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Box)
                            boardManager.board[desiredPosition.y, desiredPosition.x] = TileType.Floor;
                        else
                            boardManager.board[desiredPosition.y, desiredPosition.x] = TileType.Switch;

                        if (desiredBoxTileType == TileType.Floor)
                            boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] = TileType.Box;
                        else
                            boardManager.board[desiredBoxPosition.y, desiredBoxPosition.x] = TileType.SwitchandBox;

                        boardManager.UpdateVisuals();
                        CheckWinCondition();
                    }
                    else if (boardManager.board[position.y, position.x] == TileType.Teleporter)
                    {
                        // Rechercher une autre case de téléporteur et y téléporter le joueur
                        for (int row = 0; row < 10; row++)
                        {
                            for (int col = 0; col < 10; col++)
                            {
                                if ((row != position.y || col != position.x) && boardManager.board[row, col] == TileType.Teleporter)
                                {
                                    position = new Vector2Int(col, row);
                                    transform.position = new Vector2(position.x, -position.y);
                                    return;
                                }
                            }
                        }
                    }

                }
            }
        }
    }
    private void TeleportPlayer()
    {
        // Recherche la position du deuxième téléporteur
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if ((row != position.y || col != position.x) && boardManager.board[row, col] == TileType.Teleporter)
                {
                    // Téléportation vers le deuxième téléporteur
                    position = new Vector2Int(col, row);
                    transform.position = new Vector2(position.x, -position.y);
                    return;
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

        Debug.Log("Victoire !");
        boardManager.LoadNextLevel(); // Charger le niveau suivant
    }
}
