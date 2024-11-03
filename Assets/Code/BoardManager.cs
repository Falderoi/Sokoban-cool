using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject boxTilePrefab;
    [SerializeField] private GameObject switchTilePrefab;
    [SerializeField] private GameObject boxandswitchTilePrefab;
    [SerializeField] private GameObject teleporterPrefab;
    [SerializeField] private GameObject[] floorTilePrefabs;
    [SerializeField] private PlayerMovement playerController;
    [SerializeField] private TextMeshProUGUI movementText; // UI text for moves
    [SerializeField] private LevelData[] levels; // Array for multiple levels

    private LevelData currentLevel;
    private int currentLevelIndex = 0; // Index of the current level
    public TileType[,] board { get; private set; }
    private int moveCount = 0; // Counter for moves

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.Log("All levels completed!");
            return;
        }

        // Charger le niveau spécifié
        currentLevel = levels[levelIndex];
        board = new TileType[10, 10];
        string[] levelLines = currentLevel.content.Split('\n');

        // Réinitialiser le compteur de mouvements
        moveCount = 0;
        UpdateMovementDisplay(); // Met à jour l'affichage

        // Initialiser la position de départ du joueur
        Vector2Int playerStartPosition = Vector2Int.zero; // Initialiser à une position par défaut
        bool playerFound = false; // Indicateur si le joueur est trouvé

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
                        playerStartPosition = new Vector2Int(col, row); // Enregistrer la position de départ
                        playerFound = true; // Indiquer que le joueur est trouvé
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

        // Initialiser la position du joueur ici
        if (playerFound)
        {
            playerController.Init(playerStartPosition); // Initialiser le joueur à la position correcte
        }
    }




    private void InstantiateFloorTiles()
    {
        // Optionally clear existing floor tiles before instantiating new ones
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Floor"))
                Destroy(child.gameObject);
        }

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (board[row, col] == TileType.Floor || board[row, col] == TileType.Switch)
                {
                    GameObject chosenFloorPrefab = (Random.value < 0.1f) ? floorTilePrefabs[0] : floorTilePrefabs[1];
                    Instantiate(chosenFloorPrefab, new Vector2(col, -row), Quaternion.identity, transform).tag = "Floor";
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
                if (board[row, col] == TileType.Box || board[row, col] == TileType.Switch)
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

    public void IncrementMoveCount()
    {
        moveCount++;
        Debug.Log("Move Count: " + moveCount); 
        UpdateMovementDisplay(); 
    }


    public void ResetLevel()
    {
        moveCount = 0; 
        UpdateMovementDisplay(); 
        LoadLevel(currentLevelIndex); 
    }

    public void UpdateMovementDisplay()
    {
        Debug.Log("Updating movement display to: " + moveCount);
        if (movementText != null)
        {
            movementText.text = "Moves: " + moveCount; 
        }
        else
        {
            Debug.LogError("movementText is not assigned in BoardManager.");
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            LoadLevel(currentLevelIndex); // Charge le niveau suivant
        }
        else
        {
            Debug.Log("Congratulations! You have completed all levels!");
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






