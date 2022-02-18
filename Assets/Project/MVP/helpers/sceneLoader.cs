using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class sceneLoader : MonoBehaviour
{
    public static ReactiveCollection<string> loadedScenes = new ReactiveCollection<string>();
    public static string beforeScene;
    public static IObservable<string> loadedScenesAsObservable = loadedScenes.ToObservable();
    public static List<SceneInstance> loadedScenesInstance= new List<SceneInstance>();
    public static SceneInstance sceneToUnload = new SceneInstance();
    public static bool sceneFound;
    void Awake()
    {
        loadedScenes = Enumerable.Range(0, SceneManager.sceneCount).Select(sceneIndex => SceneManager.GetSceneAt(sceneIndex).name).ToReactiveCollection();
    }
    public static void AddScene(string sceneName)
    {
        // ResetARInfo();
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).AsObservable()
            .Take(1)
            .Subscribe(_ => loadedScenes.Add(sceneName));
    }
    public static void UnloadScene(string sceneName)
    {
        beforeScene = sceneName;
        SceneManager.UnloadSceneAsync(sceneName).AsObservable()
            .Take(1)
            .DelayFrame(1)
            .Subscribe(_ => loadedScenes.Remove(sceneName));
    }
    public static void ChangeScene(string sceneName)
    {
        string lastSceneName = loadedScenes.Last();
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).AsObservable()
            .Take(1)
            .Subscribe(_ => { loadedScenes.Add(sceneName); UnloadScene(lastSceneName); });
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    public static void LoadSceneAddressable(string sceneName)
    {
        Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive).Completed += (asyncHandle) =>
        {
            sceneLoader.loadedScenesInstance.Add(asyncHandle.Result);
        };

        
        
    }
    public static void UnloadSceneAddressable(string sceneName)
    {
        sceneLoader.getSceneFromValue(sceneName);
        if (sceneLoader.sceneFound)
        {
            Addressables.UnloadSceneAsync(sceneToUnload).Completed += (asyncHandle) =>
            {
                sceneLoader.loadedScenesInstance.Remove(sceneLoader.sceneToUnload);
                sceneLoader.loadedScenesInstance.RemoveAll(item => item.Scene == null);
            };
        }

    }
    public static void getSceneFromValue(string sNAme)
    {
        if (loadedScenesInstance.Count > 0)
        {
            for ( int i=0; i < loadedScenesInstance.Count; i++)
            {
                if (loadedScenesInstance[i].Scene.name == sNAme)
                {
                    sceneToUnload = loadedScenesInstance[i];
                    sceneLoader.sceneFound = true;
                    return;
                }
                else
                {
                    sceneLoader.sceneFound = false;
                }
            }
        }
    }
    

}

