using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    public static void ReloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public static void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void QuitGame() {
        Application.Quit();
    }

    public void ReloadCurrentSceneBtn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSceneBtn(int index) {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneBtn(string name) {
        SceneManager.LoadScene(name);
    }

    public void LoadNextLevelBtn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGameBtn() {
        Application.Quit();
    }

}