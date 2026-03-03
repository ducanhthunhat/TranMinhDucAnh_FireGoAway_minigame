using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIWinPanel : UICanvas
{
    public void NextLevel()
    {
        UIManager.Instance.CloseUIDirectly<UIWinPanel>();
        UIManager.Instance.ResumeGame();

        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
