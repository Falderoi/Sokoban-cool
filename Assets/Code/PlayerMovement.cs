using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TextMeshProUGUI movementCounterTextTMP; 
    
    private Vector2Int position;
    private int movementCounter = 0;

    public void Init(Vector2Int startPosition)
    {
        position = startPosition;
        transform.position = new Vector2(position.x, -position.y);
        ResetMovementCounter(); 
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
                if (boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Floor
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Switch
                    || boardManager.board[desiredPosition.y, desiredPosition.x] == TileType.Teleporter)
                {
                    position = desiredPosition;
                    transform.position = new Vector2(position.x, -position.y);

                    movementCounter++;
                    UpdateMovementCounterText(); 

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

                        movementCounter++;
                        UpdateMovementCounterText(); 

                        boardManager.UpdateVisuals();
                        CheckWinCondition();
                    }
                    else if (boardManager.board[position.y, position.x] == TileType.Teleporter)
                    {
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
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if ((row != position.y || col != position.x) && boardManager.board[row, col] == TileType.Teleporter)
                {
                    position = new Vector2Int(col, row);
                    transform.position = new Vector2(position.x, -position.y);

                    movementCounter++;
                    UpdateMovementCounterText(); 

                    return;
                }
            }
        }
    }

    private void ResetMovementCounter()
    {
        movementCounter = 0;
        UpdateMovementCounterText();
    }

    private void UpdateMovementCounterText()
    {
        movementCounterTextTMP.text = "Mouvements : " + movementCounter;
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
        boardManager.LoadNextLevel();
        ResetMovementCounter(); 
    }
}

