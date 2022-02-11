using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using TMPro;
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
    // Start is called before the first frame update
    void Start()
    {
        setAllMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setAllMeshes()
    {
        foreach (Transform child in objectParent.transform)
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
