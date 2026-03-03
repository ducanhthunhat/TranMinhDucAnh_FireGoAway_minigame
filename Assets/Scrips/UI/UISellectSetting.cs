using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISellectSetting : UICanvas
{
    public void OpenSelectSetting()
    {
        UIManager.Instance.OpenUI<UISellectSetting>();
    }
    public void ExitSelectSetting()
    {
        UIManager.Instance.CloseUIDirectly<UISellectSetting>();
    }
}
