using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int playerRow = 1; // Position initiale du joueur (ligne)
    private int playerCol = 1; // Position initiale du joueur (colonne)

    private BoardManager boardManager;

    private bool isMoving = false; // Emp�che les mouvements multiples � la fois

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        MovePlayer(playerRow, playerCol); // Place le joueur au d�part
    }

    void Update()
    {
        // Si le joueur n'est pas d�j� en mouvement, v�rifier les entr�es
        if (!isMoving)
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // Axe horizontal (fl�ches / joystick)
            float vertical = Input.GetAxisRaw("Vertical"); // Axe vertical (fl�ches / joystick)

            if (horizontal != 0)
            {
                AttemptMove(playerRow, playerCol + (int)horizontal); // D�placer vers la gauche ou la droite
            }
            else if (vertical != 0)
            {
                AttemptMove(playerRow - (int)vertical, playerCol); 
            }
        }
    }
    void AttemptMove(int newRow, int newCol)
    {
        
        if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 10)
        {
            
            if (!boardManager.board[newRow, newCol])
            {
                playerRow = newRow;
                playerCol = newCol;
                MovePlayer(playerRow, playerCol);
            }
            else
            {
                Debug.Log("Collision avec un mur!");
            }
        }
        isMoving = true;
        Invoke("ResetMove", 0.2f); 
    }

    void MovePlayer(int row, int col)
    {
        transform.position = new Vector2(col, -row); 
    }

    void ResetMove()
    {
        isMoving = false; 
    }
}
