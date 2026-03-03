using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BtnLv1 : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
        UIManager.Instance.ResumeGame();
    }

}
