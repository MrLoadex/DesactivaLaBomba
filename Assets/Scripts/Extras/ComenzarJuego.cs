using UnityEngine;
using UnityEngine.SceneManagement;

public class ComenzarJuego : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }
}
