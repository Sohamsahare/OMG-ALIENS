using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelCompletion : MonoBehaviour {
    public void LevelCompleted()
    {

    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
