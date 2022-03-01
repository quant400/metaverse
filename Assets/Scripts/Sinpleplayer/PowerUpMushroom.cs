using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PowerUpMushroom : MonoBehaviour
{
    [SerializeField]
    float PowerupSpeedMultiplier, jumpMultiplier, powerUpDuration;
    AudioSource audioS;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        int chance = Random.Range(0 , 100);
        if (chance > gameplayView.instance.GetMushroomPowerUpChance())
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Light>().enabled = false;
        }
        else if (tag == "Manhole")
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().startColor = GetComponent<Light>().color;
            transform.GetChild(0).gameObject.SetActive(true);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            if (tag == "Manhole")
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = false;

            }
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Light>().enabled = false;
            StartCoroutine(Powerup(other.gameObject));
        }
    }

    public void PlayPowerUp()
    {
        if(!gameplayView.instance.GetSFXMuted())
            audioS.Play();
    }

    IEnumerator Powerup(GameObject player)
    {
        PlayPowerUp();
        ThirdPersonController TPC = player.GetComponent<ThirdPersonController>();
        TPC.MoveSpeed *= PowerupSpeedMultiplier;
        TPC.JumpHeight *= jumpMultiplier;
        TPC.SprintSpeed = TPC.MoveSpeed;
        //for testing 
        if (player != null)
        {
            player.transform.GetChild(3).gameObject.SetActive(true);

        }

        yield return new WaitForSeconds(powerUpDuration);

        TPC.MoveSpeed /= PowerupSpeedMultiplier;
        TPC.JumpHeight /= jumpMultiplier;
        TPC.SprintSpeed = TPC.MoveSpeed*1.5f;
        //for testing 
        if (player != null)
        {
            player.transform.GetChild(3).gameObject.SetActive(false);
        }
        if(tag!="Manhole")
            Destroy(gameObject);

    }


}
