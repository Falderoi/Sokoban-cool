using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int playerRow = 1; // Position initiale du joueur (ligne)
    private int playerCol = 1; // Position initiale du joueur (colonne)

    private BoardManager boardManager;

    private bool isMoving = false; // Empêche les mouvements multiples à la fois

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        MovePlayer(playerRow, playerCol); // Place le joueur au départ
    }

    void Update()
    {
        // Si le joueur n'est pas déjà en mouvement, vérifier les entrées
        if (!isMoving)
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // Axe horizontal (flèches / joystick)
            float vertical = Input.GetAxisRaw("Vertical"); // Axe vertical (flèches / joystick)

            if (horizontal != 0)
            {
                AttemptMove(playerRow, playerCol + (int)horizontal); // Déplacer vers la gauche ou la droite
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
