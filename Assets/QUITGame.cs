using UnityEngine;

public class QUITGame : MonoBehaviour
{
    public void QuitGame()
    {
        // Logs to confirm button works in editor
        //Debug.Log("Quit button pressed!");

        // Quits the application
        Application.Quit();
    }
}
