using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using System.IO;

public class SinglePlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject chickenPrefab;
    [SerializeField]
    GameObject[] characters;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform[] chickenSpawnPoints;
    [SerializeField]
    Transform chickenHolder;
    [SerializeField]
    GameObject[] NPCPrefab;
    [SerializeField]
    Transform[] NPCSPawnPoints;
    [SerializeField]
    int startingChickensForLevel;
    [SerializeField]
    float spawnInterval;

    private string chosenNFTName;

    private void Start()
    {
        //SinglePlayerScoreBoardScript.instance.PlayerJoined("1", 1);

        //span point chaged to index 4 intead of 0 will make it a single point intead of array if only one player in game decided
        //GameObject temp = Instantiate(characters[SingleplayerGameControler.instance.chosenAvatar], spawnPoint.position, Quaternion.identity);
        chosenNFTName = NameToSlugConvert(gameplayView.instance.chosenNFT.name);
        GameObject resource = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", chosenNFTName)) as GameObject;
        GameObject temp = Instantiate(resource, spawnPoint.position, Quaternion.identity);
        
        SpawnChickens();
        SpawnNpc();
    }


    void SpawnChickens()
    {
        int remainingToSpwan = startingChickensForLevel;//gameplayView.instance.GetChickenCount();
        int index = 0;
        while(remainingToSpwan>0)
        {
            var temp=Instantiate(chickenPrefab, chickenSpawnPoints[index].position, Quaternion.identity);
            temp.transform.parent = chickenHolder;
            index++;
            if (index >= 19)
                index = 0;
            remainingToSpwan--;
        }
        StartCoroutine("SpawnRandomChicken");
    }

    IEnumerator SpawnRandomChicken()
    {
        yield return new WaitForSeconds(spawnInterval);
        var temp=Instantiate(chickenPrefab, chickenSpawnPoints[Random.Range(0,10)].position, Quaternion.identity);
        temp.transform.parent = chickenHolder;
        StartCoroutine("SpawnRandomChicken");
    }

    void SpawnNpc()
    {
        for (int i=0;i<NPCSPawnPoints.Length;i++)
        {
            List<int> randomNPCs = new List<int>();
            var randomNPCNo = Random.Range(0, NPCPrefab.Length);
            while (randomNPCs.Contains(randomNPCNo) || NPCPrefab[randomNPCNo].name == chosenNFTName)
                randomNPCNo = Random.Range(0, NPCPrefab.Length);

            Instantiate(NPCPrefab[randomNPCNo], NPCSPawnPoints[i].position, Quaternion.identity);
        }
    }

    public GameObject[] GetCharacterList()
    {
        return characters;
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }
    
}
