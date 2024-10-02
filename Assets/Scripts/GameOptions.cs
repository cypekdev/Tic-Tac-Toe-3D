using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptions : MonoBehaviour
{
    [SerializeField] GameObject gameOptionsModal;
    [SerializeField] TMP_InputField firstNickname;
    [SerializeField] TMP_InputField secondNickname;
    [SerializeField] TMP_InputField thirdNickname;

    public void ShowGameOptions()
    {
        gameOptionsModal.SetActive(true);
    }

    public void HideGameOptions()
    {
        gameOptionsModal.SetActive(false);
    }

    void SaveOptions()
    {
        PlayerPrefs.SetString("firstNickname", firstNickname.text);
        PlayerPrefs.SetString("secondNickname", secondNickname.text);
        PlayerPrefs.SetString("thirdNickname", thirdNickname.text);
        PlayerPrefs.Save();
    }

    public void EnterGame()
    {
        SaveOptions();
        SceneManager.LoadSceneAsync("Game");
    }
}
