using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject boxTilePrefab;
    [SerializeField] private GameObject switchTilePrefab;
    [SerializeField] private GameObject boxandswitchTilePrefab;
    [SerializeField] private GameObject[] floorTilePrefabs;  // Tableau de prefabs pour les différentes cases de sol
    [SerializeField] private PlayerMovement playerController;

    [SerializeField] private LevelData levelToLoad;

    public TileType[,] board { get; private set; }

    private void Start()
    {
        LoadLevel(levelToLoad);
    }

    public void LoadLevel(LevelData loadedLevel)
    {
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
                }
            }
        }
        InstantiateFloorTiles();
        UpdateVisuals();
    }
    private void InstantiateFloorTiles()
    {
        // Instancie les cases de sol une seule fois lors du chargement du niveau
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (board[row, col] == TileType.Floor || board[row, col] == TileType.Switch || board[row, col] == TileType.SwitchandBox)
                {
                    GameObject chosenFloorPrefab;

                    // Exemple : 90% pour floorTilePrefabs[0] et 10% pour floorTilePrefabs[1]
                    if (Random.value < 0.1f) // 10% de chance
                        chosenFloorPrefab = floorTilePrefabs[0];
                    else                     // 90% de chance
                        chosenFloorPrefab = floorTilePrefabs[1];

                    Instantiate(chosenFloorPrefab,
                        new Vector2(col, -row),
                        Quaternion.identity,
                        transform);
                }
            }
        }
    }
    public void UpdateVisuals()
    {
        // Nettoie uniquement les objets mobiles (comme les murs, boîtes, interrupteurs), pas les sols
        foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("Movable"))  // Assurez-vous que seuls les objets avec le tag "Movable" sont détruits
                Destroy(childTransform.gameObject);
        }

        // Instancie les autres objets par-dessus le sol
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Vector2 position = new Vector2(col, -row);

                // S'assure qu'il y a un sol en dessous de chaque objet mobile
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






