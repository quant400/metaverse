using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
public class PlatformManager : MonoBehaviour
{
    public static PlatformManager control;
    public Transform player;
    public ReactiveProperty<Vector3> reactivePlayerPosition = new ReactiveProperty<Vector3>();
    public Platform mainPlatform;
    public List<Platform> sidePlatforms=new List<Platform>();

    public Platform[] platforms;
    public ReactiveProperty<Platform> mainReactivePlatform = new ReactiveProperty<Platform>();
    public Material[] stateMaterials;
    void Awake()
    {
        if (control == null)
        {
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        obserePlayerMoving();
    }
    private void Update()
    {
        reactivePlayerPosition.Value = player.position;
        //FindMainPlatform();
        //PositionOtherPlatforms();
    }
    void obserePlayerMoving()
    {
        reactivePlayerPosition
            .Do(_ => FindMainPlatform())
            .Subscribe()
            .AddTo(this);
        mainReactivePlatform
            .Where(_ => _ != null)
            .Do(_ => PositionOtherPlatforms())
            .Do(_ => _.gameObject.GetComponent<Renderer>().material = stateMaterials[0])
            .Do(_ => SceneLoaderView.control.loadSceneFromVector(_.index, _.gameObject.transform.GetChild(0).position))
            .DelayFrame(2)
            .Do(_=> loadSidePlatforms())
            .Subscribe()
            .AddTo(this);

    }
    void loadSidePlatforms()
    {
        for(int i = 0; i < sidePlatforms.Count; i++)
        {
            SceneLoaderView.control.LoadSideScene(sidePlatforms[i].index, sidePlatforms[i].gameObject.transform.GetChild(0).position, SceneLoaderView.control.toLoadSceneSideScenes.Value);
        }
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
                mainReactivePlatform.Value = mainPlatform;
        sidePlatforms.Clear();
            for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i] != mainPlatform)
            {
                sidePlatforms.Add(platforms[i]);
            }
        }
           
        
        
    }
    
    void PositionOtherPlatforms()
    {
        int count = 1;
        foreach (Platform platform in platforms)
        {
            if (platform != mainPlatform)
            {
                setOtherPlatformMaterials(platform.gameObject.GetComponent<Renderer>());
                platform.internalIndex = count;
                SnapPltform(platform);
                count++;
            }
        }
    }
    void setOtherPlatformMaterials(Renderer mesh)
    {
        mesh.material = stateMaterials[1];
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
