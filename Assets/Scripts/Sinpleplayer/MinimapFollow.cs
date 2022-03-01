using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField]
    Transform player;

    private void LateUpdate()
    {
        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(player!=null)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
        
    }
}
