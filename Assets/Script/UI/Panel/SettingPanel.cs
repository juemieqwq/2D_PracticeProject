using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingPanel : BasePanel
{
    [SerializeField]
    private GameObject settingPanel;
    [Header("通过滑动条的值变化进行广播")]
    [SerializeField]
    private FloatEventSO setMasterVolumeSO;
    [SerializeField]
    private Slider slider;
    private void Start()
    {
        //settingPanel?.SetActive(false);
        if (slider == null)
            slider = GetComponentInChildren<Slider>();
    }
    public override void ClosePanel()
    {
        settingPanel.SetActive(false);
        base.ClosePanel();
    }

    public override void OpenPanel(string name)
    {
        Time.timeScale = 0;
        slider.value = (RadioManager.instance.GetMasterVolume() + 80f) / 100f;
        settingPanel.SetActive(true);
        base.OpenPanel(name);
    }

    public void BroadCastEvent()
    {
        setMasterVolumeSO.Raise(slider.value * 100 - 80);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
