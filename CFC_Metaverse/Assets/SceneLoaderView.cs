using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using UnityEngine.SceneManagement;
using System;
public class SceneLoaderView : MonoBehaviour
{
    public static SceneLoaderView control;
    [Serializable]
    public class scenesClass
    {
       public string sceneName;
        public Vector2 sceneCordination;
        public Vector3 instancePosition;
    }
    public scenesClass[] scenesList;
    public List<scenesClass> loadedScenes = new List<scenesClass>();
    public ReactiveProperty<scenesClass> toLoadScene = new ReactiveProperty<scenesClass>();
    // Start is called before the first frame update
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        observeSceneChanged();

    }
    void observeSceneChanged()
    {
        toLoadScene
            .Do(_ => setSceneData(_))
            .Subscribe()
            .AddTo(this);
    }
    void setSceneData(scenesClass sceneData)
    {
        if (loadedScenes != null)
        {
            if (loadedScenes.Count > 0)
            {
                for(int i= 0; i < loadedScenes.Count; i++)
                {
                    sceneLoader.UnloadScene(loadedScenes[i].sceneName);
                }
            }
            loadedScenes.Clear();
            loadedScenes = null;
            loadedScenes = new List<scenesClass>();
        }
        if (sceneData.sceneName.Length>0 )
        {
            Debug.Log(sceneData.sceneName);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
            loadedScenes.Add(sceneData);
        }
       


    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadSceneFromVector(Vector2 index,Vector3 centerPositon)
    {
        for(int i =0; i < scenesList.Length; i++)
        {
            if (scenesList[i].sceneCordination == index)
            {
                toLoadScene.Value = scenesList[i];
                scenesList[i].instancePosition = centerPositon;
            }
        }
    }
    sceneLoadClass FindSceneObject(string sceneName)
    {
        sceneLoadClass data =new sceneLoadClass();
        foreach (sceneLoadClass obj in FindObjectsOfType(typeof(sceneLoadClass)))
        {
            if (obj.gameObject.scene.name.CompareTo(sceneName) == 0)
            {
                data = obj;
            }
           
        }
        return data;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Observable.Timer(TimeSpan.Zero)
                        .DelayFrame(2)
                        .Do(_ => setSceneObjectInCenter(scene.name))
                        .Subscribe()
                        .AddTo(this);
    }
    public void setSceneObjectInCenter(string sceneNameMain)
    {
        if (sceneNameMain == toLoadScene.Value.sceneName)
        {
            GameObject mainSceneObject = FindSceneObject(toLoadScene.Value.sceneName).gameObject;
            mainSceneObject.GetComponent<sceneLoadClass>().objectParent.position = toLoadScene.Value.instancePosition;
            mainSceneObject.GetComponent<sceneLoadClass>().gridValue = toLoadScene.Value.sceneCordination;
            SceneManager.sceneLoaded -= OnSceneLoaded;

        }
    }

}
