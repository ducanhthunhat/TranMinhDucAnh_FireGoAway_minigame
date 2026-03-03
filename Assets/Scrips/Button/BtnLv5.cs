using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnLv5 : MonoBehaviour
{
    public void LoadLevel5()
    {
        SceneManager.LoadScene(5);
        UIManager.Instance.ResumeGame();
    }
}
