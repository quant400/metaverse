using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
public class sceneLoadClass : MonoBehaviour
{
    public Vector2 gridValue;
    public int sceneMatrixRank;
    public Transform mainParent;
    public Transform objectParent;
    public int gridFactor;
    public sceneAdditiveModel.sceneAdditiveClass sceneData = new sceneAdditiveModel.sceneAdditiveClass();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
