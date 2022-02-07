using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Vector2 index;
    public Transform player;
    [SerializeField] Vector3 dir;

    [Header("Neighbors")]
    public GameObject leftNeighbor;
    public GameObject rightNeighbor;
    public GameObject upNeighbor;
    public GameObject downNeighbor;

    private void Start()
    {
        index.x = transform.position.x;
        index.y = transform.position.z;
    }

    private void Update()
    {
        GetPlayerPosition();
    }

    void GetPlayerPosition()
    {
        dir = (player.transform.position - this.transform.position);

        if (Mathf.Abs(dir.x) > 70 || Mathf.Abs(dir.z) > 70)
            Destroy(gameObject);

        if (dir.x < -40 && leftNeighbor == null) // LEFT
            InstantiatePlatform(0);

        if (dir.x > 40 && rightNeighbor == null) // RIGHT
            InstantiatePlatform(1);

        if (dir.z < -40 && upNeighbor == null) // UP
            InstantiatePlatform(2);

        if (dir.z > 40 && downNeighbor == null) // BOTTOM
            InstantiatePlatform(3);
    }

    void InstantiatePlatform(byte byt)
    {
        switch (byt)
        {
            case 0: leftNeighbor = Instantiate(gameObject, new Vector3(transform.position.x-100,transform.position.y,transform.position.z), Quaternion.identity);
                leftNeighbor.GetComponent<Platform>().rightNeighbor = gameObject;
                break;
            case 1: rightNeighbor = Instantiate(gameObject, new Vector3(transform.position.x+100,transform.position.y,transform.position.z), Quaternion.identity);
                rightNeighbor.GetComponent<Platform>().leftNeighbor = gameObject;
                break;
            case 2: upNeighbor = Instantiate(gameObject, new Vector3(transform.position.x,transform.position.y,transform.position.z-100), Quaternion.identity);
                upNeighbor.GetComponent<Platform>().downNeighbor = gameObject;
                break;
            case 3: downNeighbor = Instantiate(gameObject, new Vector3(transform.position.x,transform.position.y,transform.position.z+100), Quaternion.identity);
                downNeighbor.GetComponent<Platform>().upNeighbor = gameObject;
                break;
        }
    }
}
