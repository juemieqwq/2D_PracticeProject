using UnityEngine;
using UnityEngine.SceneManagement;

//AI写的- -
public class SceneStateManager : MonoBehaviour
{
    private static bool _isApplicationQuitting = false;
    private static bool _isSceneUnloading = false;

    /// <summary>
    /// 是否正在退出应用程序
    /// </summary>
    public static bool IsApplicationQuitting
    {
        get { return _isApplicationQuitting; }
    }

    /// <summary>
    /// 是否正在卸载场景
    /// </summary>
    public static bool IsSceneUnloading
    {
        get { return _isSceneUnloading; }
    }

    /// <summary>
    /// 综合判断：是否处于退出状态（应用退出或场景卸载）
    /// </summary>
    public static bool IsQuitting
    {
        get { return _isApplicationQuitting || _isSceneUnloading; }
    }

    /// <summary>
    ///     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]作用
    /// 这个特性允许你在不创建GameObject、不手动调用的情况下，在游戏运行时的特定阶段自动执行一个静态方法。
    /// </summary>
    //public enum RuntimeInitializeLoadType
    //{
    //    // 在场景加载之前执行（第一帧之前）
    //    BeforeSceneLoad,

    //    // 在场景加载之后执行（第一帧之前）  
    //    AfterSceneLoad,

    //    // 在子系统初始化之后执行
    //    AfterAssembliesLoaded,

    //    // 在Splash Screen显示之前执行
    //    BeforeSplashScreen,

    //    // 子系统初始化时执行
    //    SubsystemRegistration
    //}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 确保管理器在场景加载前存在
        var manager = new GameObject("SceneStateManager");
        manager.AddComponent<SceneStateManager>();
        DontDestroyOnLoad(manager);
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.quitting += OnApplicationQuitting;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Application.quitting -= OnApplicationQuitting;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _isSceneUnloading = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isSceneUnloading = false;
    }

    private void OnApplicationQuitting()
    {
        _isApplicationQuitting = true;
        _isSceneUnloading = true;
    }
}
