using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISellectLevel : UICanvas
{
    public void OpenSelectLevel()
    {
        UIManager.Instance.OpenUI<UISellectLevel>();
    }
    public void ExitSelectLevel()
    {
        UIManager.Instance.CloseUIDirectly<UISellectLevel>();
    }
}
