using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnLv3 : MonoBehaviour
{
    public void LoadLevel3()
    {
        SceneManager.LoadScene(3);
        UIManager.Instance.ResumeGame();
    }
}
