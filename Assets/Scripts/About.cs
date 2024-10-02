using UnityEngine;

public class About : MonoBehaviour
{
    [SerializeField] GameObject aboutGameModal;

    public void ShowAbout()
    {
        aboutGameModal.SetActive(true);
    }

    public void HideAbout()
    {
        aboutGameModal.SetActive(false);
    }
}
