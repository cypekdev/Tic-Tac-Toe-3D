using System;
using UnityEngine;

public class ObjectsRayPointer : MonoBehaviour
{

    public GameObject lastIndicatedObject;
    public bool isIndicating;
    public bool allowSelecting;

    private Material hightlightedMaterial;
    int objectIndex = 0;
    GameManager gameManager;

    [SerializeField] GameObject SettingsPanel;


    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        allowSelecting = true;
    }

    void Update()
    {
        SetHightlightedColor();
        ResetAllInactivesMaterial();
        if (allowSelecting && !SettingsPanel.activeSelf)
        {
            HandlePointing();
            HightLightGameObject();
            HandleSelecting();
        }
    }


    void SetHightlightedColor()
    {
        hightlightedMaterial = new(gameManager.inactiveMaterial);
        int mpi = gameManager.movementPlayerIndex;
        Color color = new Material(gameManager.players[mpi].material).color;
        color.a = 0.5f;
        hightlightedMaterial.color = color;
    }

    void ResetAllInactivesMaterial()
    {
        GameObject[,,] gameCharacters = gameManager.gameCharacters;

        foreach (var gameCharacter in gameCharacters)
        {
            if (gameCharacter.CompareTag("Unoccupied"))
            {
                gameCharacter.GetComponent<Renderer>().material = gameManager.inactiveMaterial;
            }
        }
    }

    void HandlePointing()
    {
        Ray? ray = null;

        if (Input.GetMouseButton(1))
        {
            isIndicating = false;
            return;
        }
        if (Input.touchCount > 1) return;
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(touch.position);
        }
        if (Input.mousePresent)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        if (ray == null) return;

        RaycastHit[] hits = Physics.RaycastAll((Ray)ray);

        RaycastHit[] availableHits = Array.FindAll(
            hits,
            hit => hit.transform.gameObject.CompareTag("Unoccupied")
        );

        if (availableHits.Length > 0)
        {
            float scrollValueDelta = Input.mouseScrollDelta.y;

            if (scrollValueDelta > 0)
            {
                objectIndex++;
            }
            else if (scrollValueDelta < 0)
            {
                objectIndex--;
            }

            objectIndex = (availableHits.Length + objectIndex) % availableHits.Length;

            GameObject target = availableHits[objectIndex].transform.gameObject;
            
            lastIndicatedObject = target;
            isIndicating = true;
            return;
        }
        isIndicating = false;
    }

    void HightLightGameObject()
    {
        if (!isIndicating) return;
        Renderer renderer = lastIndicatedObject.GetComponent<Renderer>();
        if (renderer != null) renderer.material = hightlightedMaterial;
    }


    private void HandleSelecting()
    {
        if (!isIndicating) return;
        if (Input.mousePresent && Input.GetMouseButtonUp(0)) gameManager.TakeOver(lastIndicatedObject);
    }

    public void PutButtonClicked()
    {
        gameManager.TakeOver(lastIndicatedObject);
    }
}
