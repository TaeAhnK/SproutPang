using UnityEngine;

public static class RuntimeSettings
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}