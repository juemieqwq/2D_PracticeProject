using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private UIManager()
    {

    }
    public static UIManager Instance = null;
    public PanelInfoTable panelInfoTable;
    public PackageItemTable packageItemTable;
    private GameObject UIRoot = null;
    public Dictionary<string, BasePanel> runningPanels = new Dictionary<string, BasePanel>();
    public Dictionary<string, BasePanel> cachePanels = new Dictionary<string, BasePanel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            panelInfoTable = Resources.Load<PanelInfoTable>("UI/Panel/PanelInfoTable");
            packageItemTable = Resources.Load<PackageItemTable>("UI/Package/PackageTable");

            if (UIRoot == null)
            {
                UIRoot = new GameObject("UIRoot");
                //var canvas = UIRoot.AddComponent<Canvas>();
                //canvas.renderMode = RenderMode.ScreenSpaceCamera;
                //var canvasScale = UIRoot.AddComponent<CanvasScaler>();
                //canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                //canvasScale.referenceResolution = new Vector2(1920, 1080);

            }
            if (FindObjectOfType<EventSystem>() == null)
            {
                // 如果场景中没有EventSystem，可以自动添加
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CrateUIManager()
    {
        if (FindFirstObjectByType<UIManager>() == null)
        {
            var UIObject = new GameObject("UIManager");
            UIObject.AddComponent<UIManager>();
        }
    }

    public BasePanel OpenPanel(string panelName)
    {
        BasePanel panel = null;

        if (runningPanels.TryGetValue(panelName, out panel))
        {
            Debug.LogError("界面以打开，无法再次打开");
            return null;
        }
        if (cachePanels.TryGetValue(panelName, out var havePanel))
        {
            //havePanel.gameObject.transform.parent.gameObject.SetActive(true);
            havePanel.OpenPanel(panelName);
            cachePanels.Remove(panelName);
            runningPanels.Add(panelName, havePanel);
            return havePanel;
        }


        GameObject panelPrefab = panelInfoTable.GetPanelPrefabs(panelName).panelPrefab;
        if (panelPrefab != null)
        {
            GameObject panelGameObject = GameObject.Instantiate(panelPrefab, UIRoot.transform, false);
            panel = panelGameObject.GetComponentInChildren<BasePanel>();
            panel.OpenPanel(panelName);
            runningPanels.Add(panelName, panel);
            return panel;
        }
        return null;
    }

    public bool ClosePanel(string panelName)
    {
        BasePanel panel;
        Time.timeScale = 1;
        if (!runningPanels.TryGetValue(panelName, out panel))
        {
            Debug.LogError("界面未打开");
            return false;
        }
        panel.ClosePanel();
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            if (runningPanels.ContainsKey("PackagePanel"))
            {
                ClosePanel("PackagePanel");
            }
            else
                OpenPanel("PackagePanel");
        }

        if (Input.GetKeyDown("o"))
        {
            BasePanel packagePanel;
            if (runningPanels.TryGetValue("PackagePanel", out packagePanel))
            {
                var info = packageItemTable.GetPackageItemInfo(ItemName.Ruby);
                (packagePanel as PackagePanel).AddItem(ItemName.Ruby, ItemRarity.Epic);
            }
            else if (cachePanels.TryGetValue("PackagePanel", out packagePanel))
            {
                var info = packageItemTable.GetPackageItemInfo(ItemName.Ruby);
                (packagePanel as PackagePanel).AddItem(ItemName.Ruby, ItemRarity.Epic);
            }
        }

        if (Input.GetKeyDown("p"))
        {
            BasePanel packagePanel;
            if (runningPanels.TryGetValue("PackagePanel", out packagePanel))
            {
                var info = packageItemTable.GetPackageItemInfo(ItemName.SilverBreastplate);
                (packagePanel as PackagePanel).AddItem(ItemName.SilverBreastplate, ItemRarity.Legendary);
            }
            else if (cachePanels.TryGetValue("PackagePanel", out packagePanel))
            {
                var info = packageItemTable.GetPackageItemInfo(ItemName.Ruby);
                (packagePanel as PackagePanel).AddItem(ItemName.SilverBreastplate, ItemRarity.Legendary);
            }
        }
    }

}
