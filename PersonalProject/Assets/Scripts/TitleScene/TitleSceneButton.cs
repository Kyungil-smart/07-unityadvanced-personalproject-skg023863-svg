using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneButton : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene((int)SceneName.GameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
