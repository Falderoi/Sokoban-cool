using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject floorTilePrefab;
    public bool[,] board { get; private set; }

    private void Start()
    {
        board = new bool[10, 10];
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (row == 0 || row == 9 || col == 0 || col == 9)
                    board[row, col] = true;
                else
                    board[row, col] = false;
            }
        }
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (board[row, col] == true)
                    Instantiate(wallTilePrefab,
                        new Vector2(col, -row), Quaternion.identity);
                else
                    Instantiate(floorTilePrefab,
                    new Vector2(col, -row), Quaternion.identity);

            }
        }
    }
    private void TestListe()
    {
        List<string> maListeDeStrings = new List<string>();
        maListeDeStrings.Add("bonjour"); // ["bonjour"]
        maListeDeStrings.Add("test"); // ["Bonjour", "test"]
        maListeDeStrings.RemoveAt(1);
        string recupValeur = maListeDeStrings[0];
        maListeDeStrings[0] = "Coucou";

        string[] maListeALAncienne = new string[10];
        List<List<string>> listeDeListe = new List<List<string>>();
        string[,] maListeDeListeALAncienne = new string[10, 10];
        maListeDeListeALAncienne[0, 1] = "test";

    }





}
