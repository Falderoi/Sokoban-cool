using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    private Vector2Int position;

    // Movement counter
    public int moveCount { get; private set; } = 0; 

    // Optimal move count
    private int optimalMoves;

    public struct GameState
    {
        public Vector2Int playerPosition;
        public TileType[,] boardState;

        public GameState(Vector2Int playerPos, TileType[,] board)
        {
            playerPosition = playerPos;
            boardState = board.Clone() as TileType[,];
        }
    }
    public void Init(Vector2Int startPosition)
    {
        position = startPosition; // Définit la position du joueur
        transform.position = new Vector2(position.x, -position.y); // Met à jour la position du transform
        moveCount = 0; // Réinitialiser le compteur de mouvements du joueur
        boardManager.IncrementMoveCount(); // Appelle cette méthode pour mettre à jour l'affichage des mouvements
        Debug.Log($"Player initialized at position: {startPosition}");

    }



    private void Update()
    {
        Vector2Int desiredPosition = position;
        bool playerMoved = false;

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
                // Si le joueur se déplace
                if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Floor
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Switch
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Teleporter)
                {
                    position = desiredPosition;
                    transform.position = new Vector2(position.x, -position.y);
                    boardManager.IncrementMoveCount(); // Incrémente le compteur de mouvements
                    playerMoved = true;

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
                        moveCount++; 

  

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
                        // Search for another teleporter and teleport the player
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
                    position = desiredPosition; // Si le mouvement est valide
                    moveCount++;
                    boardManager.IncrementMoveCount();
                    if (playerMoved)
                    {
                        MovePlayer(desiredPosition);
                    }
                }
            }
        }
    }
    private void MovePlayer(Vector2Int desiredPosition)
    {

        boardManager.IncrementMoveCount();
    }
    private void TeleportPlayer()
    {

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if ((row != position.y || col != position.x) && boardManager.board[row, col] == TileType.Teleporter)
                {
                    // Teleport to the second teleporter
                    position = new Vector2Int(col, row);
                    transform.position = new Vector2(position.x, -position.y);
                    return;
                }
            }
        }
    }
    public void ResetPosition()
    {
        position = Vector2Int.zero; // Réinitialiser la position à l'origine ou à la position de départ
        transform.position = new Vector2(position.x, -position.y); // Met à jour la position du transform
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

        Debug.Log("Victory!");
        boardManager.LoadNextLevel(); // Load the next level
    }
}

