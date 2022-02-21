using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSreenScript : MonoBehaviour
{
    [SerializeField]
    GameObject characterSelectionPanel;


    public void PlayButton()
    {
        //characterSelectionPanel.SetActive(true);
        
        //characterSelectionPanel.transform.localScale = Vector3.one;
        //Debug.Log("ok");
        this.gameObject.SetActive(false);

    }
}
