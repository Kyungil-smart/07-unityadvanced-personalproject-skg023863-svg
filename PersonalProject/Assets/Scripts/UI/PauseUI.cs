using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public void BackToGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void GotoTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene((int)SceneName.TitleScene);
    }
}
