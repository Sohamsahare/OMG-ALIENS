using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    public void Quit()
    {
        Debug.Log("Application Quit!");
        Application.Quit();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	public void NextLevel(){
		SceneManager.LoadScene (1);
	}
}
