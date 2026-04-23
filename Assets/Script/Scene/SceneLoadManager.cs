using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour, ISaveableGameObject
{
    private SceneLoadManager()
    {

    }

    public static SceneLoadManager instance;

    [Header("监听事件")]
    [SerializeField]
    private VoidEventSO newGameEvent;
    [Header("广播事件")]
    [SerializeField]
    private VoidEventSO unLoadSceneEvent;
    [Header("菜单角色位置")]
    [SerializeField]
    public Vector3 menuPosition;
    [Header("第一个场景角色位置")]
    [SerializeField]
    private Vector3 firstPosition;
    [SerializeField]
    [Header("场景资源的SO")]
    private SceneAssetsReferenceSO sceneReferenceSO;
    [SerializeField]
    [Header("新场景加载事件")]
    private VoidEventSO loadSceneSO;
    public AssetReference currentScene { private set; get; }
    public string currentSceneKey { private set; get; }
    private AssetReference goToScene;
    private string goToSceneKey;
    private Vector3 playerGoToPosition;
    private bool isFade;
    [Header("淡出淡入的UI对象")]
    [SerializeField]
    private Fade fadeClass;
    [Header("淡出淡入的时间")]
    [SerializeField]
    private float fadeTime;
    //场景加载的协程引用
    private Coroutine loadSceneCoroutine;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Camera playerCamera;

    public bool isFirstEnterCave = true;
    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (sceneReferenceSO == null)
        {
            sceneReferenceSO = (Resources.LoadAsync("Scense/SceneAssetsReference").asset) as SceneAssetsReferenceSO;
            if (sceneReferenceSO == null)
                Debug.LogError("场景引用赋值失败");

        }
        currentScene = sceneReferenceSO.GetSceneAssetReference("Menu");
        currentSceneKey = "Menu";
        currentScene.LoadSceneAsync(LoadSceneMode.Additive, true);
        newGameEvent.AddEventListener(LoadNewGame);
    }



    private void Start()
    {
        player.playerController.SetPlayerController(false);
        player.transform.position = menuPosition;
        playerCamera.gameObject.SetActive(false);
        (this as ISaveableGameObject).RegisterSaveDate();
        isFirstEnterCave = true;
    }

    private void OnDisable()
    {
        (this as ISaveableGameObject).UnRegisterSaveDate();
    }



    public void LoadNewGame()
    {
        Debug.LogError("角色死亡次数:" + player.deathNum);
        if (player.deathNum == 1 || currentSceneKey == "Forest")
        {
            LoadNewScene("Forest", new Vector3(-28, -15, 0));
        }
        else
            LoadNewScene("Cave", firstPosition);
    }


    public void LoadNewScene(string GoToKey, Vector3 playerGoToPosition, bool isFade = true)
    {
        goToSceneKey = GoToKey;
        goToScene = sceneReferenceSO.GetSceneAssetReference(goToSceneKey);
        this.playerGoToPosition = playerGoToPosition;
        this.isFade = isFade;
        if (loadSceneCoroutine == null && goToScene != null)
            loadSceneCoroutine = StartCoroutine(IELoadNewScene());
    }

    private IEnumerator IELoadNewScene()
    {
        var waitFadeTime = new WaitForSeconds(fadeTime);
        if (isFade)
        {
            player.playerController.SetPlayerController(false);
            player.SetInputX(0);
            fadeClass.IsFadeIn(fadeTime, true);
            yield return waitFadeTime;
        }

        unLoadSceneEvent?.Raise();
        //等待上一个场景卸载
        yield return currentScene.UnLoadScene();
        //等待新场景加载
        yield return goToScene.LoadSceneAsync(LoadSceneMode.Additive, true);
        //将已卸载的空场景引用改为跳转的场景
        currentScene = goToScene;
        currentSceneKey = goToSceneKey;
        goToScene = null;
        goToSceneKey = string.Empty;
        PlayerManager.instance.player.transform.position = playerGoToPosition;
        loadSceneCoroutine = null;
        waitFadeTime = new WaitForSeconds(fadeTime * .5f);
        //等待一帧让新场景的进入事件进行订阅
        yield return null;
        loadSceneSO.Raise();
        yield return waitFadeTime;
        if (isFade)
        {
            fadeClass.IsFadeIn(fadeTime, false);
            yield return waitFadeTime;
            if (currentSceneKey == "Menu")
                player.playerController.SetPlayerController(false);
            else if (currentSceneKey != "Cave")
                player.playerController.SetPlayerController(true);
            else if (currentSceneKey == "Cave" && !isFirstEnterCave)
                player.playerController.SetPlayerController(true);
        }
        player.ResetPlayer();
    }

    public DataDefinition GetDateDefinition()
    {
        return null;
    }

    public void SaveDate(ref SavebleGameObjectDate date)
    {
        date.SaveSceneDate(currentSceneKey);
    }

    public void LoadSaveDate(ref SavebleGameObjectDate date)
    {
        var key = date.LoadSceneDate();
        if (key != currentSceneKey)
            LoadNewScene(key, player.transform.position);
    }
}
