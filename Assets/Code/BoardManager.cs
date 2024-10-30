using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject boxTilePrefab;
    [SerializeField] private GameObject switchTilePrefab;
    [SerializeField] private GameObject boxandswitchTilePrefab;
    [SerializeField] private PlayerMovement playerController;


    [SerializeField] private LevelData levelToLoad;

    public TileType[,] board { get; private set; }

    private void Start()
    {
        board = new TileType[10, 10];
        string[] levelLines = levelToLoad.content.Split('\n');

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                switch (levelLines[row][col])
                {
                    case 'F':
                        board[row, col] = TileType.Floor;
                        break;
                    case 'B':
                        board[row, col] = TileType.Box;
                        break;
                    case 'W':
                        board[row, col] = TileType.Wall;
                        break;
                    case 'S':
                        board[row, col] = TileType.Floor;
                        playerController.Init(new Vector2Int(col, row));
                        break;
                    case 'I':
                        board[row, col] = TileType.Switch;
                        break;
                }
            }
        }

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        foreach (Transform childTransform in transform)
            Destroy(childTransform.gameObject);

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (board[row, col] == TileType.Wall)
                    Instantiate(wallTilePrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);
                else if (board[row, col] == TileType.Box)
                    Instantiate(boxTilePrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);

                else if (board[row, col] == TileType.Switch)
                    Instantiate(switchTilePrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);
                else if (board[row, col] == TileType.SwitchandBox)
                    Instantiate(boxandswitchTilePrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);
                else
                    Instantiate(floorTilePrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);
            }
        }
    }
}

/* private void TestListe()
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

 }*/






