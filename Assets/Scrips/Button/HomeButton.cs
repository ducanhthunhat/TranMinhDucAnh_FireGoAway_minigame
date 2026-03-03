using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoHome()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.ResumeGame();
    }
}
