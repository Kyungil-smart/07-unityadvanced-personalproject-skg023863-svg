using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadUI : MonoBehaviour
{
    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene((int)SceneName.GameScene);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene((int)SceneName.TitleScene);
    }
}
