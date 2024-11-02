using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject boxTilePrefab;
    [SerializeField] private GameObject switchTilePrefab;
    [SerializeField] private GameObject boxandswitchTilePrefab;
    [SerializeField] private GameObject teleporterPrefab;
    [SerializeField] private GameObject[] floorTilePrefabs;
    [SerializeField] private PlayerMovement playerController;
  
    
    [SerializeField] private LevelData[] levels; // Tableau pour plusieurs niveaux
    private int currentLevelIndex = 0; // Index du niveau actuel

    public TileType[,] board { get; private set; }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.Log("Tous les niveaux sont terminés !");
            return;
        }

        // Charger le niveau spécifié
        LevelData loadedLevel = levels[levelIndex];
        board = new TileType[10, 10];
        string[] levelLines = loadedLevel.content.Split('\n');

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
                    case 'T':
                        board[row, col] = TileType.Teleporter;
                        break;

                }
            }
        }
        InstantiateFloorTiles();
        UpdateVisuals();
    }

    private void InstantiateFloorTiles()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (board[row, col] == TileType.Floor || board[row, col] == TileType.Switch || board[row, col] == TileType.SwitchandBox)
                {
                    GameObject chosenFloorPrefab = (Random.value < 0.1f) ? floorTilePrefabs[0] : floorTilePrefabs[1];
                    Instantiate(chosenFloorPrefab, new Vector2(col, -row), Quaternion.identity, transform);
                }
            }
        }
    }

    public void UpdateVisuals()
    {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("Movable"))
                Destroy(childTransform.gameObject);
        }

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Vector2 position = new Vector2(col, -row);
                if (board[row, col] == TileType.Box || board[row, col] == TileType.Switch || board[row, col] == TileType.SwitchandBox)
                {
                    GameObject chosenFloorPrefab = (Random.value < 0.1f) ? floorTilePrefabs[0] : floorTilePrefabs[1];
                    Instantiate(chosenFloorPrefab, position, Quaternion.identity, transform);
                }

                if (board[row, col] == TileType.Wall)
                {
                    Instantiate(wallTilePrefab, position, Quaternion.identity, transform).tag = "Movable";
                }
                else if (board[row, col] == TileType.Box)
                {
                    Instantiate(boxTilePrefab, position, Quaternion.identity, transform).tag = "Movable";
                }
                else if (board[row, col] == TileType.Switch)
                {
                    Instantiate(switchTilePrefab, position, Quaternion.identity, transform).tag = "Movable";
                }
                else if (board[row, col] == TileType.SwitchandBox)
                {
                    Instantiate(boxandswitchTilePrefab, position, Quaternion.identity, transform).tag = "Movable";
                }
                else if (board[row, col] == TileType.Teleporter)
                {
                    Instantiate(teleporterPrefab, position, Quaternion.identity, transform).tag = "Movable";
                }
            }
        }
    }

    // Passer au niveau suivant
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("Félicitations ! Vous avez terminé tous les niveaux !");
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






