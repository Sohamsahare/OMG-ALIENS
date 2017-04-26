using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void EndGame()
    {
        Debug.Log("We Quit!!");
        Application.Quit();
    }
}
