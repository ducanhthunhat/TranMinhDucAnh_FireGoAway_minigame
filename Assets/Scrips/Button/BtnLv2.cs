using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnLv2 : MonoBehaviour
{
    // Start is called before the first frame updatepublic void LoadLevel1()
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
        UIManager.Instance.ResumeGame();
    }

}
