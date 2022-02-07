using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public Transform player;

    public Platform mainPlatform;
    public Platform[] platforms;

    private void Update()
    {
        FindMainPlatform();
        PositionOtherPlatforms();
    }

    void FindMainPlatform()
    {
        float distance = 9999f;
        foreach (Platform platform in platforms)
        {
            if ((platform.transform.GetChild(0).position - player.position).magnitude < distance)
            {
                distance = (platform.transform.GetChild(0).position - player.position).magnitude;
                mainPlatform = platform;
            }
        }
        mainPlatform.setIndex();
    }

    void PositionOtherPlatforms()
    {
        int count = 1;
        foreach (Platform platform in platforms)
        {
            if (platform != mainPlatform)
            {
                platform.internalIndex = count;
                SnapPltform(platform);
                count++;
            }
        }
    }

    void SnapPltform(Platform platform)
    {
        switch(platform.internalIndex)
        {
            case 1:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x + 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z);
                break;
            case 2:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x + 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z + 100);
                break;
            case 3:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x, mainPlatform.transform.position.y, mainPlatform.transform.position.z+100);
                break;
            case 4:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x - 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z + 100);
                break;
            case 5:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x - 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z);
                break;
            case 6:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x - 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z-100);
                break;
            case 7:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x, mainPlatform.transform.position.y, mainPlatform.transform.position.z - 100);
                break;
            case 8:
                platform.transform.position = new Vector3(mainPlatform.transform.position.x + 100, mainPlatform.transform.position.y, mainPlatform.transform.position.z - 100);
                break;
        }

        platform.setIndex();
    }

    public Vector2 GetMainPlatformIndex()
    {
        return mainPlatform.index;
    }
}
