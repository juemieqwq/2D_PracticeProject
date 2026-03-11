using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    private UIManager()
    {

    }
    public static UIManager Instance = null;
    public PanelInfoTable panelInfoTable;
    public PackageItemTable packageItemTable;
    [SerializeField]
    private PlayerController playerController;

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
                //eventSystem.AddComponent<StandaloneInputModule>();
                eventSystem.AddComponent<InputSystemUIInputModule>();
            }

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

    }



    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //private static void CrateUIManager()
    //{
    //    if (FindFirstObjectByType<UIManager>() == null)
    //    {
    //        var UIObject = new GameObject("UIManager");
    //        UIObject.AddComponent<UIManager>();
    //    }
    //}



    public BasePanel OpenPanel(string panelName)
    {
        if (SceneLoadManager.instance.currentSceneKey == "Menu")
            return null;
        BasePanel panel = null;
        if (runningPanels.TryGetValue(panelName, out panel))
        {
            Debug.LogError("界面以打开，无法再次打开");
            return null;
        }
        playerController.SetPlayerController(false);
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
        playerController.SetPlayerController(true);
        return true;
    }

    private void Update()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (runningPanels.ContainsKey("PackagePanel"))
            {
                ClosePanel("PackagePanel");
            }
            else if (runningPanels.ContainsKey("SettingPanel"))
            {
                ClosePanel("SettingPanel");
            }
            else
                OpenPanel("SettingPanel");
        }
        else if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (runningPanels.ContainsKey("PackagePanel"))
            {
                ClosePanel("PackagePanel");
            }
            else
                OpenPanel("PackagePanel");
        }



        if (Keyboard.current.oKey.wasPressedThisFrame)
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

        if (Keyboard.current.pKey.wasPressedThisFrame)
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
