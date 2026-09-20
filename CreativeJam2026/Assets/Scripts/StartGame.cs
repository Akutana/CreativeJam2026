using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadDinerScene()
    {
        SceneManager.LoadScene("Diner");
    }
}
