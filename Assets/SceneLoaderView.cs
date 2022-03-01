using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class SceneLoaderView : MonoBehaviour
{
    public static SceneLoaderView control;
    public GameObject prefabTest;
    [Serializable]
    public class scenesClass
    {
       public string sceneName;
        public Vector2 sceneCordination;
        public Vector3 instancePosition;
        public scenesClass(string name, Vector2 index, Vector3 position)
        {
            sceneName = name;
            sceneCordination = index;
            instancePosition = position;
        }
    }
    [Serializable]
public class sceneLoadedData
    {
        public string sceneName;
        public int rceneRank;
        public sceneLoadedData(string name, int index)
        {
            sceneName = name;
            rceneRank = index;
        }
    }


    public List<scenesClass> scenesIndexList = new List<scenesClass>();
    public List<sceneLoadedData> sideLoadedScenesListData = new List<sceneLoadedData>();

    public scenesClass[] scenesList;
    public List<scenesClass> loadedScenes = new List<scenesClass>();
    public ReactiveProperty<scenesClass> toLoadScene = new ReactiveProperty<scenesClass>();
    public ReactiveProperty<scenesClass> toLoadSceneSideScenes = new ReactiveProperty<scenesClass>();
    public List<sceneLoadClass> SideScenesLoadClassList = new List<sceneLoadClass>();
    public Vector2[,] indexToScenesArray;
    public int arrayLenght=60;
    public int startIndex = 30;
    Vector2[] staticPostionToCenter = new Vector2[8] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 1), new Vector2(-1, -1) };
    [SerializeField]
    ReactiveProperty<int> currentExistCount = new ReactiveProperty<int>();
    bool checkLoadOnce;
    public bool useAddressable;
    public  List<SceneInstance> loadedScenesInstance = new List<SceneInstance>();
    public  SceneInstance sceneToUnload = new SceneInstance();
    public  bool sceneFound;
    // Start is called before the first frame update
    void Awake()
    {
           indexToScenesArray = new Vector2[arrayLenght, arrayLenght];
        startIndex = arrayLenght / 2;
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        setArrayOfIndexes(indexToScenesArray);
    }
    void setArrayOfIndexes(Vector2[,] array)
    {
        scenesIndexList = new List<scenesClass>();
        int count = 0;
        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j  = array.GetLength(1)-1 ; j > -1; j--)
            {
                count++;
                array[i, j] = new Vector2(i - startIndex, j - startIndex);
                scenesIndexList.Add(new scenesClass("scene " + count.ToString(), new Vector2(i - startIndex, startIndex -j ), Vector3.zero));
                //GameObject clone = Instantiate(prefabTest, new Vector3((i - startIndex) * 100, 0, (j - startIndex) * 100), Quaternion.identity, transform);
            }
        }
        Debug.Log(array[startIndex, startIndex+1]);

    }
    void Start()
    {
        observeSceneChanged();
        obserSceneSideCountValue();

    }
    void observeSceneChanged()
    {
        toLoadScene
            .Delay(TimeSpan.FromMilliseconds(500))
            .Do(_=> unloadScene())
            .DelayFrame(2)
            .Do(_ => setSceneDataAndLoad(_))
            .Subscribe()
            .AddTo(this);
        toLoadSceneSideScenes
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Do(_ => setSideSceneDataAndLoad(_))
            .Subscribe()
            .AddTo(this);
    }
    void unloadScene()
    {
        if (loadedScenes != null)
        {
            UnloadAllScenesExcept("Main");
            loadedScenes.Clear();
            loadedScenes = null;
            loadedScenes = new List<scenesClass>();
        }
        if (useAddressable)
        {
            loadedScenesInstance.Clear();
            loadedScenesInstance = new List<SceneInstance>();
        }
      
    }
    void setSceneDataAndLoad(scenesClass sceneData)
    {
        Debug.Log("Assets/Project/Scenes/SceneLoadDemo/" + sceneData.sceneName + ".unity");
        if (sceneData.sceneName.Length > 0)
        {
            if (useAddressable)
            {
                Addressables.LoadSceneAsync("Assets/Project/Scenes/SceneLoadDemo/" + sceneData.sceneName + ".unity", LoadSceneMode.Additive).Completed += (asyncHandle) =>
                {
                   loadedScenesInstance.Add(asyncHandle.Result);
                    checkAddressableLoaded(sceneData.sceneName);
                };

            }
            else
            {

                SceneManager.sceneLoaded += OnMainSceneScene;
                SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(sceneData);
            }
        }




    }
    void UnloadAllScenesExcept(string sceneName)
    {
        if (useAddressable)
        {
            int c = loadedScenesInstance.Count;
            for (int i = 0; i < c; i++)
            {
                Scene scene = loadedScenesInstance[i].Scene;
                if (scene.name != sceneName)
                {
                    Addressables.UnloadSceneAsync(loadedScenesInstance[i]).Completed += (asyncHandle) =>
                    {
                    };
                }

            }
        


        }
        else
        {
            int c = SceneManager.sceneCount;
            for (int i = 0; i < c; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != sceneName)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
        
    }
    void setSideSceneDataAndLoad(scenesClass sceneData)
    {

        if (sceneData.sceneName.Length > 0)
        {
            if (useAddressable)
            {
                
                 
                
                Addressables.LoadSceneAsync("Assets/Project/Scenes/SceneLoadDemo/" + sceneData.sceneName + ".unity", LoadSceneMode.Additive).Completed += (asyncHandle) =>
                {
                    loadedScenesInstance.Add(asyncHandle.Result);
                    OnSideSceneLoadedAddressable(sceneData.sceneName);

                };

            }
            else
            {

                if (!checkLoadOnce)
                {
                    SceneManager.sceneLoaded += OnSideSceneLoaded;
                    checkLoadOnce = true;
                }
                SceneManager.LoadScene(sceneData.sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(sceneData);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadSceneFromVector(Vector2 index,Vector3 centerPositon)
    {
        if (useAddressable)
        {

            int place = (int)(((index.x + startIndex) * arrayLenght) + index.y-1 + startIndex);
            toLoadScene.Value = scenesIndexList[place];
            scenesIndexList[place].instancePosition = centerPositon;
        }
        else
        {

            int place = (int)(((index.x + startIndex) * arrayLenght) + index.y - 1 + startIndex);
            toLoadScene.Value = scenesIndexList[place];
            scenesIndexList[place].instancePosition = centerPositon;
        }
    }
    public void LoadSideScene(Vector2 index, Vector3 centerPositon, scenesClass scene)
    {
        if (useAddressable)
        {
            int place = (int)(((index.x + startIndex) * arrayLenght) + index.y - 1+ startIndex);
            toLoadSceneSideScenes.Value = new scenesClass("SideObject", index, centerPositon);
            scenesIndexList[place].instancePosition = centerPositon;
        }
        else
        {
            int place = (int)(((index.x + startIndex) * arrayLenght) + index.y - 1 + startIndex);
            toLoadSceneSideScenes.Value = new scenesClass("SideObject", index, centerPositon);
            scenesIndexList[place].instancePosition = centerPositon;
        }
    }
    sceneLoadClass FindSceneObject( Vector2 gridValue,string sceneNAme)
    {
        int c = SceneManager.sceneCount;
        sceneLoadClass localData=null;

        sceneLoadClass data = new sceneLoadClass();
        data.gridValue = new Vector2(9999, 9999);
        for (int i = 0; i < c; i++)
        {
            Scene sceneValue = SceneManager.GetSceneAt(i);
            if (sceneNAme == sceneValue.name)
            {
                GameObject[] rootObjs = sceneValue.GetRootGameObjects();
                Debug.Log(rootObjs.Length);
                if (rootObjs != null)
                {
                    if (rootObjs.Length > 0)
                    {
                        for (int j = 0; j < rootObjs.Length; j++)
                        {
                            if (rootObjs[j].GetComponent<sceneLoadClass>() != null)
                            {
                                localData = rootObjs[j].GetComponent<sceneLoadClass>();
                                return localData;
                            }
                        }
                        
                    }
                }
            }
        }
        if (localData == null)
        {
            return data;

        }
        else
        {
            return localData;
        }


    }
    sceneLoadClass FindSceneObjectAddressable(Vector2 gridValue, string sceneNAme)
    {
        int c = loadedScenesInstance.Count;
        sceneLoadClass localData = null;
        Debug.Log(c);
        sceneLoadClass data = new sceneLoadClass();
        data.gridValue = new Vector2(9999, 9999);
        for (int i = 0; i < c; i++)
        {
            Scene sceneValue = loadedScenesInstance[i].Scene;
            if (sceneNAme == sceneValue.name)
            {
                GameObject[] rootObjs = sceneValue.GetRootGameObjects();
                Debug.Log(rootObjs.Length);
                if (rootObjs != null)
                {
                    if (rootObjs.Length > 0)
                    {
                        for (int j = 0; j < rootObjs.Length; j++)
                        {
                            if (rootObjs[j].GetComponent<sceneLoadClass>() != null)
                            {
                                localData = rootObjs[j].GetComponent<sceneLoadClass>();
                                return localData;
                            }
                        }

                    }
                }
            }
        }
        if (localData == null)
        {
            return data;

        }
        else
        {
            return localData;
        }


    }
    sceneLoadClass FindSideSceneObject(Vector2 gridValue)
    {
        sceneLoadClass data = new sceneLoadClass();
        foreach (sceneLoadClass obj in SideScenesLoadClassList)
        {
            if (obj.gridValue == gridValue)
            {
                data = obj;
            }

        }
        return data;

    }
    public void setSceneIndexBeforeSetPostion( string mainScene, string centerLoadedScene)
    {

        if (useAddressable)
        {
            sceneLoadClass data = new sceneLoadClass();

            int c = loadedScenesInstance.Count;
            SideScenesLoadClassList.Clear();
            for (int i = 0; i < c; i++)
            {
                Scene scene = loadedScenesInstance[i].Scene;

                if (scene.name != centerLoadedScene)
                {
                    GameObject[] rootObjs = scene.GetRootGameObjects();
                    if (rootObjs != null)
                    {
                        if (rootObjs.Length > 0)
                        {
                            for (int j = 0; j < rootObjs.Length; j++)
                            {
                            
                                if (rootObjs[j].GetComponent<sceneLoadClass>() != null)
                                {
                                    SideScenesLoadClassList.Add(rootObjs[j].GetComponent<sceneLoadClass>());
                                }
                            }
                        }

                    }
                }



            }
        }
        else
        {
            sceneLoadClass data = new sceneLoadClass();

        int c = SceneManager.sceneCount;
        SideScenesLoadClassList.Clear();
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            
                GameObject[] rootObjs = scene.GetRootGameObjects();
                if (rootObjs != null)
                {
                if (rootObjs.Length > 0)
                {
                    for (int j = 0; j < rootObjs.Length; j++)
                    {
                            if ((scene.name != mainScene) && (scene.name != centerLoadedScene))
                            {
                                if (rootObjs[j].GetComponent<sceneLoadClass>() != null)
                                {
                                    SideScenesLoadClassList.Add(rootObjs[j].GetComponent<sceneLoadClass>());
                                }
                            }
                    }
                }
                    
                }
                
            
        }
        }



    }
    void obserSceneSideCountValue()
    {
        currentExistCount
            .Where(_ => _ == 8)
            .DelayFrame(2)
            .Where(_=>checkLoadOnce)
            .Do(_ => setSceneIndexBeforeSetPostion("Main", toLoadScene.Value.sceneName))
            .Do(_ => setSideLoadScenePosition())
            .Do(_ => SceneManager.sceneLoaded -= OnSideSceneLoaded)
            .Do(_=> checkLoadOnce=false)
            .Subscribe()
            .AddTo(this);
    }
    public void OnSideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Vector3 center = toLoadSceneSideScenes.Value.instancePosition;
        Vector2 index = toLoadSceneSideScenes.Value.sceneCordination;
        int c = SceneManager.sceneCount;
         currentExistCount.Value = 0;
        sideLoadedScenesListData.Clear();
        for (int i = 0; i < c; i++)
        {
            Scene sceneValue = SceneManager.GetSceneAt(i);

            if (scene.name==sceneValue.name)
            {
                sideLoadedScenesListData.Add(new sceneLoadedData(scene.name, i));
                currentExistCount.Value++;
            }
        }
       
      
       
    }
    public void OnSideSceneLoadedAddressable(string sceneName)
    {
        Vector3 center = toLoadSceneSideScenes.Value.instancePosition;
        Vector2 index = toLoadSceneSideScenes.Value.sceneCordination;
        int c = loadedScenesInstance.Count;
        currentExistCount.Value = 0;
        sideLoadedScenesListData.Clear();
        for (int i = 0; i < c; i++)
        {
            Scene sceneValue = loadedScenesInstance[i].Scene;

            if (sceneName == sceneValue.name)
            {
                sideLoadedScenesListData.Add(new sceneLoadedData(sceneName, i));
                currentExistCount.Value++;
            }
        }



    }
    public void setSideLoadScenePosition()
    {
        if (SideScenesLoadClassList != null)
        {
           if ( SideScenesLoadClassList.Count >= PlatformManager.control.sidePlatforms.Count)
            {
               for ( int i = 0; i < SideScenesLoadClassList.Count; i++)
                {

                    SideScenesLoadClassList[i].objectParent.position = PlatformManager.control.sidePlatforms[i].centerPostion();
                }
            }
        }
    }
    public void OnMainSceneScene(Scene scene, LoadSceneMode mode)
    {
        Vector2 index = toLoadScene.Value.sceneCordination;
        Observable.Timer(TimeSpan.Zero)
                     .Do(_ => setMainSceneObjectInCenter(scene.name, toLoadScene.Value, index))
                     .Subscribe()
                     .AddTo(this);
    }
    public void checkAddressableLoaded(string sceneName)
    {
        Vector2 index = toLoadScene.Value.sceneCordination;
        Observable.Timer(TimeSpan.Zero)
            .DelayFrame(1)
                     .Do(_ => setMainSceneObjectInCenter(sceneName, toLoadScene.Value, index))
                     .Subscribe()
                     .AddTo(this);
    }
    public void setMainSceneObjectInCenter(string sceneNameMain, scenesClass sceneData,Vector2 indexGrid)
    {

        if (useAddressable)
        {
            if (sceneNameMain == sceneData.sceneName)
            {
                if (FindSceneObjectAddressable(indexGrid, sceneNameMain).gridValue.x != 9999)
                {
                    GameObject mainSceneObject = FindSceneObjectAddressable(indexGrid, sceneNameMain).gameObject;
                    mainSceneObject.GetComponent<sceneLoadClass>().objectParent.position = sceneData.instancePosition;
                    mainSceneObject.GetComponent<sceneLoadClass>().gridValue = sceneData.sceneCordination;
                    mainSceneObject.GetComponent<sceneLoadClass>().sceneNameTextFile.text = sceneNameMain;
                    mainSceneObject.GetComponent<sceneLoadClass>().sceneGridValue.text = "Grid Values Are : " + sceneData.sceneCordination.x.ToString() + " , " + sceneData.sceneCordination.y.ToString();
                    mainSceneObject.GetComponent<sceneLoadClass>().setRandomColors();

                    Debug.Log("founded ");

                }
                else
                {
                    Debug.Log("cant be found ");
                }


            }
        }
        else
        {
            if (sceneNameMain == sceneData.sceneName)
            {
                if (FindSceneObject(indexGrid, sceneNameMain).gridValue.x != 9999)
                {
                    GameObject mainSceneObject = FindSceneObject(indexGrid, sceneNameMain).gameObject;
                    mainSceneObject.GetComponent<sceneLoadClass>().objectParent.position = sceneData.instancePosition;
                    mainSceneObject.GetComponent<sceneLoadClass>().gridValue = sceneData.sceneCordination;
                    mainSceneObject.GetComponent<sceneLoadClass>().sceneNameTextFile.text = sceneNameMain;
                    mainSceneObject.GetComponent<sceneLoadClass>().sceneGridValue.text = "Grid Values Are : " + sceneData.sceneCordination.x.ToString() + " , " + sceneData.sceneCordination.y.ToString();
                    mainSceneObject.GetComponent<sceneLoadClass>().setRandomColors();

                    SceneManager.sceneLoaded -= OnMainSceneScene;
                    Debug.Log("founded ");

                }
                else
                {
                    Debug.Log("cant be found ");
                }


            }
        }
       
    }
    public void setSideSceneObjectInCenter(string sceneNameMain, scenesClass sceneData,Vector3 centerPos,Vector2 indexGrid)
    {
        if (sceneNameMain == sceneData.sceneName)
        {
            GameObject mainSceneObject = FindSideSceneObject( indexGrid).gameObject;
            mainSceneObject.GetComponent<sceneLoadClass>().objectParent.position = centerPos;
            mainSceneObject.GetComponent<sceneLoadClass>().gridValue = sceneData.sceneCordination;
            SceneManager.sceneLoaded -= OnSideSceneLoaded;

        }
    }

}
