using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
public class setFollowSettings : MonoBehaviour
{
    public  CinemachineVirtualCamera followCameraSettings;
    public GameObject player;
    public StarterAssetsInputs _input;
    Cinemachine3rdPersonFollow thirdperson;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = transform.GetComponentInParent<GameObject>();
        }
      
        if (followCameraSettings == null)
        {
            followCameraSettings = GetComponent<CinemachineVirtualCamera>();
        }
       
        if (followCameraSettings != null)
        {
            followCameraSettings.LookAt = followCameraSettings.Follow;
             thirdperson = followCameraSettings.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            followCameraSettings.AddCinemachineComponent<CinemachineHardLookAt>();
            thirdperson.Damping.z = 0.5f;
        }
        
    }
    public void setObject()
    {
        if (followCameraSettings == null)
        {
            followCameraSettings = GetComponent<CinemachineVirtualCamera>();
        }

        if (followCameraSettings != null)
        {
            followCameraSettings.LookAt = followCameraSettings.Follow;
            thirdperson = followCameraSettings.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            followCameraSettings.AddCinemachineComponent<CinemachineHardLookAt>();
            thirdperson.Damping.z = 0.5f;
        }

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (followCameraSettings != null)
    //    {

    //        if (_input.sprint)
    //        {
    //            thirdperson.Damping.z = 1.2f;
    //        }
    //        else
    //        {
    //            thirdperson.Damping.z = 0.5f;
    //        }
    //    }
    //}
}
