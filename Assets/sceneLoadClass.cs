using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class sceneLoadClass : MonoBehaviour
{
    public Vector2 gridValue;
    public int sceneMatrixRank;
    public Transform mainParent;
    public Transform objectParent;
    public int gridFactor;
    public sceneAdditiveModel.sceneAdditiveClass sceneData = new sceneAdditiveModel.sceneAdditiveClass();
    public TextMeshProUGUI sceneNameTextFile;
    public TextMeshProUGUI sceneGridValue;
    public List<Renderer> allSceneMeshes = new List<Renderer>();
    public string adressableAdress;
    public GameObject loadedAdressableAsset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator loadAdressableAsset(string name)
    {
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);

        yield return goHandle;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            loadedAdressableAsset = Instantiate(goHandle.Result, objectParent);
            //etc...
        }
        setAllMeshes(loadedAdressableAsset);
        setRandomColors();
        Addressables.Release(goHandle);

    }
    public void noAdressableInstance(GameObject prefab)
    {
        loadedAdressableAsset = Instantiate(prefab, objectParent);
        setAllMeshes(loadedAdressableAsset);
        setRandomColors();
    }
    public async void asyncAdressablesLoad(string address)
    {
        var validateAddress = Addressables.LoadAssetAsync<GameObject>(address);
        GameObject enemyPrefab = await Addressables.LoadAssetAsync<GameObject>(address).Task;
        if (validateAddress.Status ==AsyncOperationStatus.Succeeded)
        {
            
                loadedAdressableAsset = Instantiate(enemyPrefab, objectParent);
            setAllMeshes(loadedAdressableAsset);
            setRandomColors();
            Addressables.Release(validateAddress);

        }
    }
    public void setAllMeshes(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {

            if (child.GetComponent<Renderer>())
            {
                allSceneMeshes.Add(child.GetComponent<Renderer>());
                
            }
        }

    }
    public void setRandomColors()
    {
        if (allSceneMeshes != null)
        {
            if (allSceneMeshes.Count > 0)
            {
                for(int i = 0;i< allSceneMeshes.Count; i++)
                {
                    allSceneMeshes[i].material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                }
            }
        }
    }
    public void setScenePostion(Vector2 gridPosition)
    {
        gridValue = gridPosition* gridFactor;
        mainParent.position = new Vector3(gridValue.x, 0, gridValue.y);
    }
    public void setpositionFromMatrixIndex(sceneAdditiveModel.sceneAdditiveClass data)
    {
        int x = (int)sceneAdditiveModel.playerGridPostion.Value.x;
        int y = (int)sceneAdditiveModel.playerGridPostion.Value.y;
        
    }
        public int getIntFromFloat(float value)
    {
        if (value >= 0)
        {
            return Mathf.FloorToInt(value);
        }
        else
        {
            if (Mathf.RoundToInt(value) - value > 0.5f)
            {
                return Mathf.FloorToInt(value) - 1;
            }
            else
            {
                return Mathf.RoundToInt(value);
            }
        }
    }
}
