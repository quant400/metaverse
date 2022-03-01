using UnityEngine;
//using Photon.Pun;
using UnityEngine.AI;

public class SinglePlayerChickenScript : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    int range;
    [SerializeField]
    float timeToWander;
    NavMeshAgent nav;
    [SerializeField]
    AudioClip sound1, sound2;
    AudioSource aS;
    float timeLeftToWander;
    GameObject player;
    bool played;
    float timefornextSound;
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        aS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        Wander();
    }

    void Update()
    {


        // code for changing direction when player spotted nearby
       /* RaycastHit hit;
        if (Physics.SphereCast(transform.position, 5, transform.forward, out hit, 2))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Wander();
            }
        }*/
        if ((transform.position - nav.destination).magnitude < 3 || timeLeftToWander<=0)
        {
            Wander();

        }
        else
        {
            timeLeftToWander -= Time.deltaTime;
        }
        if (player != null)
        {
            if ((transform.position - player.transform.position).magnitude < 10)
            {
                if (timefornextSound <= 0)
                {
                    if (!gameplayView.instance.GetSFXMuted())
                    {
                        aS.clip = sound1;
                        aS.Play();
                    }

                    timefornextSound = Random.Range(2, 6);
                }
                else
                {
                    timefornextSound -= Time.deltaTime;
                }
            }
            else
            {
                timefornextSound = 0;
            }
        }
        //transform.LookAt(nav.destination);

    }
    Vector3 GetRandomLocation(int maxDistance)
    {
        //old path finding code
        /*NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;*/

       
        //new pathfinding code


        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * Random.Range(10,maxDistance) + transform.position;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        bool found= NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        if (!found)
        {
            return GetRandomLocation(range);
        }
        else
            return hit.position;
    }

        
    void Wander()
    {
        timeLeftToWander = timeToWander;
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(GetRandomLocation(range), path);
        nav.SetPath(path);


    }

    private void OnTriggerEnter(Collider other)
    {

        if ((other.CompareTag("Wall") || other.CompareTag("MapObject")))
        {
            Wander();
        }
        if (other.CompareTag("Player"))
        {
            if (!gameplayView.instance.GetSFXMuted())
            { aS.clip = sound2;
                aS.Play();
            }
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            GetComponent<BoxCollider>().enabled = false;
            SinglePlayerScoreBoardScript.instance.AnimChickenCollected();
            Invoke("Collected", 1f);
        }
        if (other.CompareTag("Bot"))
        {
            Collected();
        }
    }


    public void Collected()
    {
        Destroy(gameObject);
    }

  



}
