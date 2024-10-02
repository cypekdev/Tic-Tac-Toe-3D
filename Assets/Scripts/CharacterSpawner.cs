using System.ComponentModel;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public float distanceBetweenObjects = 1.2f;
    public GameObject prefab;
    public Transform parent;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        for (int i = 0; i < 3; i++)
        {
            float posX = (i - 1) * distanceBetweenObjects;

            for (int j = 0; j < 3; j++)
            {
                float posY = (j - 1) * distanceBetweenObjects;

                for (int k = 0; k < 3; k++)
                {
                    float posZ = (k - 1) * distanceBetweenObjects;

                    GameObject gameCharacter = Instantiate(
                        prefab,
                        new Vector3(posX, posY, posZ),
                        Quaternion.identity,
                        parent
                    );

                    gameCharacter.GetComponent<Renderer>().material = gameManager.inactiveMaterial;
                    gameCharacter.tag = "Unoccupied";

                    gameManager.gameCharacters[i, j, k] = gameCharacter;
                }
            }
        }
    }
}
