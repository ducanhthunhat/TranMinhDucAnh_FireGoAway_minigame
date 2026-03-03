using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnLv4 : MonoBehaviour
{
    public void LoadLevel4()
    {
        SceneManager.LoadScene(4);
        UIManager.Instance.ResumeGame();
    }
}
