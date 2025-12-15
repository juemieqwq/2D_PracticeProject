using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PanelInfoTable", menuName = "Panel/PanelInfoTable")]
public class PanelInfoTable : ScriptableObject
{

    [SerializeField]
    private List<PanelInfo> panelsList = new List<PanelInfo>();
    private Dictionary<string, PanelInfo> panelsDic;


    public PanelInfo GetPanelPrefabs(string panelName)
    {
        PanelInfo panelInfo;
        if (panelsDic == null)
        {
            panelsDic = new Dictionary<string, PanelInfo>();
            foreach (var panel in panelsList)
            {
                if (!panelsDic.ContainsKey(panel.panelName))
                {
                    panelsDic.Add(panel.panelName, panel);
                }
            }
        }
        if (panelsDic.TryGetValue(panelName, out panelInfo))
        {
            return panelInfo;
        }
        Debug.LogError("面板信息类获取失败");
        return null;
    }

}


[System.Serializable]
public class PanelInfo
{
    public GameObject panelPrefab;
    public string panelName;
}
