using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum GameMode
{
    LocalMultiplayer,
    SingleplayerWithBots,
    MultiplayerHost,
    MultiplayerMember
}

public class GameManager : MonoBehaviour
{
    public Material inactiveMaterial;
    public Material firstPlayerMaterial;
    public Material secondPlayerMaterial;
    public Material thirdPlayerMaterial;

    public GameObject IntersectionLinePrefab;

    public GameObject summaryPanel;
    public TextMeshProUGUI summaryText;

    [SerializeField] GameObject currentPlayerContainer;
    [SerializeField] Image currentPlayerIndicator;
    [SerializeField] TextMeshProUGUI currentPlayer;
    [SerializeField] GameObject RankingButton;

    public GameObject[,,] gameCharacters = new GameObject[3, 3, 3];
    public Player[] players;
    public int movementPlayerIndex = 0;

    WinnerChecker winnerChecker;
    CharacterSpawner characterSpawner;
    ObjectsRayPointer objectsRayPointer;

    void Start()
    {
        Debug.Log(gameCharacters.Length);
        var fpn = PlayerPrefs.GetString("firstNickname", "Player1");
        var spn = PlayerPrefs.GetString("secondNickname", "Player2");
        var tpn = PlayerPrefs.GetString("thirdNickname", "Player3");

        players = new Player[] {
            new(firstPlayerMaterial, "OccupiedByPlayer0", fpn),
            new(secondPlayerMaterial, "OccupiedByPlayer1", spn),
            new(thirdPlayerMaterial, "OccupiedByPlayer2", tpn)
        };

        winnerChecker = GetComponent<WinnerChecker>();
        characterSpawner = GetComponent<CharacterSpawner>();
        objectsRayPointer = GetComponent<ObjectsRayPointer>();
        currentPlayerContainer.SetActive(true);
        RankingButton.SetActive(false);
        UpdateCurrentPlayerIndicator();
    }

    public void TakeOver(GameObject gameCharacter)
    {
        if (!gameCharacter.CompareTag("Unoccupied")) return;

        gameCharacter.tag = players[movementPlayerIndex].gameObjectTag;
        gameCharacter.GetComponent<Renderer>().material = players[movementPlayerIndex].material;

        var winner = winnerChecker.GetWinner();

        if (winner != null)
        {
            Debug.Log(winner.nickName);
            winner.isInGame = false;
            
            var cases = winnerChecker.results[winner];

            foreach (var @case in cases)
            {
                ShowIntersectionLine(@case, winner);
            }
        }

        NextTurn();
        UpdateCurrentPlayerIndicator();
    }

    void NextTurn()
    {
        bool isBoardFilled = HowManyPlacesAreOccupied() >= gameCharacters.Length;
        if (GetPlayersInGame().Length < 2 || isBoardFilled)
        {
            objectsRayPointer.allowSelecting = false;
            ShowSummary(isBoardFilled);
            return;
        }

        do movementPlayerIndex = GetNextMovementPlayerIndex();
        while (!players[movementPlayerIndex].isInGame);
    }

    int GetNextMovementPlayerIndex()
    {
        return (movementPlayerIndex + 1) % players.Length;
    }

    Player[] GetPlayersInGame()
    {
        return Array.FindAll(players, player => player.isInGame);
    }

    string[] MapPlayersName(Player[] players)
    {
        string[] names = new string[players.Length];
        for (int i = 0; i < players.Length; i++) 
        {
            names[i] = players[i].nickName;
        }
        return names;
    }

    int HowManyPlacesAreOccupied()
    {
        int occupied = 0;
        foreach (var gameCharacter in gameCharacters)
        {
            if (!gameCharacter.CompareTag("Unoccupied")) occupied++;
        }
        return occupied;
    }

    void ShowSummary(bool isBoardFilled)
    {
        currentPlayerContainer.SetActive(false);
        RankingButton.SetActive(true);
        string text = "";
        int place = 1;

        foreach (var result in winnerChecker.results)
        {
            var player = result.Key;
            text += place++ + ". " + player.nickName + "<br>";
        }

        if (isBoardFilled)
        {
            var playersInGame = GetPlayersInGame();
            if (playersInGame.Length >= 2)
            {
                text += "Draw:<br>";
                text +=String.Join(", ", MapPlayersName(playersInGame));
            }
        }

        summaryText.text = text;
        summaryPanel.SetActive(true);
    }

    float GetAngleZ(Vector3 vector)
    {
        return Mathf.Acos(vector.y / vector.magnitude) * Mathf.Rad2Deg;
    }

    float GetAngleY(Vector3 vector)
    {
        Debug.Log(vector);
        float result = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg + 90;
        return result;
    }


    Vector3 GetIntersectionLineRotation(Vector3 firstPosition, Vector3 thirdPosition)
    {
        Vector3 vector = thirdPosition - firstPosition;

        float yRot = GetAngleY(vector);
        float zRot = GetAngleZ(vector);

        return new(0, yRot, zRot);
    }

    void ShowIntersectionLine(Case @case, Player player)
    {
        var fp = @case.firstPosition;
        var tp = @case.thirdPosition;
        var dbo = characterSpawner.distanceBetweenObjects;
        var parent = characterSpawner.parent;
        Vector3 linePosition = (((fp + tp) / 2) - Vector3.one) * dbo;

        var intersectionLine = Instantiate(IntersectionLinePrefab, parent);
        intersectionLine.transform.position = linePosition;
        intersectionLine.transform.eulerAngles = GetIntersectionLineRotation(fp,  tp);

        Renderer renderer = intersectionLine.GetComponent<Renderer>();
        renderer.material = player.material;
    }

    void UpdateCurrentPlayerIndicator() 
    {
        currentPlayerIndicator.color = players[movementPlayerIndex].material.color;
        currentPlayer.text = players[movementPlayerIndex].nickName;
    }

    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
