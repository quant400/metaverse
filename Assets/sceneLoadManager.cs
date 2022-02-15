using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using System;
using UnityEngine.SceneManagement;

public class sceneLoadManager : MonoBehaviour
{
    public GameObject playerConroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //setPlayerGridDataFromPosition();
    }
    public void setPlayerGridDataFromPosition()
    {
        int x = getIntFromFloat(playerConroller.transform.position.x);
        int z = getIntFromFloat(playerConroller.transform.position.z);
        sceneAdditiveModel.playerGridPostion.Value = new Vector2(x, z);

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
