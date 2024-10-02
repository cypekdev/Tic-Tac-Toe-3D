using UnityEngine;

public class ShowOnTouch : MonoBehaviour
{
    [SerializeField] GameObject obj;
    bool isHidden = true;
    void Start()
    {
        if (Input.mousePresent) isHidden = true;
        
        if (isHidden) obj.SetActive(false);
        else obj.SetActive(true);
    }
    void Update()
    {
        if (Input.mousePresent) isHidden = true;
        if (Input.touchCount > 0) isHidden = false;

        if (isHidden) obj.SetActive(false);
        else obj.SetActive(true);
    }
}
