using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        GameObject[] chickens;
        public string number;
        public bool ended=false;
        [SerializeField]
        float searchRadius;
        float originalRadius;
        float increaseRadiusAfter=5;
        float timeLeft;
        bool found;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
	        agent.updateRotation = false;
	        agent.updatePosition = true;
            originalRadius = searchRadius;
            StartGame();
            
        }
        public void StartGame()
        {
            StartCoroutine(FollowChicken());
        }

        private void Update()
        {
            if (!ended)
            {
                if (target != null)
                    agent.SetDestination(target.position);

                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    character.Move(agent.desiredVelocity, false, false);
                    GetComponent<Animator>().SetBool("Walking", true);
                }
                else
                {
                    character.Move(Vector3.zero, false, false);
                    
                }


                if (!found)
                {
                    GetComponent<Animator>().SetBool("Walking", false);
                    timeLeft -= Time.deltaTime;
                    if(timeLeft<=0)
                    {
                        searchRadius += searchRadius;
                        timeLeft = increaseRadiusAfter;
                    }
                }
            }
            
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        IEnumerator FollowChicken()
        {
            chickens = GameObject.FindGameObjectsWithTag("Chicken");
            SetTarget(chickens[Random.Range(0,50)].transform);
            found = true;
            yield return new WaitForSeconds(10);
            StartCoroutine("FindNearestChicken");
        }

        IEnumerator FindNearestChicken()
        {
            chickens = GameObject.FindGameObjectsWithTag("Chicken");
            Transform nearest=null;
            float mindist=100000;
            foreach(GameObject c in chickens)
            {
                var dist = Vector3.Distance(c.transform.position, transform.position);
                if (dist< mindist && dist<searchRadius)
                {
                    nearest = c.transform;
                    mindist = dist;
                }
            }
            if (nearest != null)
            {
                found = true;
                timeLeft = increaseRadiusAfter;
                searchRadius = originalRadius;
            }
            else
                found = false;
            SetTarget(nearest);
            yield return new WaitForSeconds(UnityEngine.Random.Range(1,4));
            StartCoroutine("FindNearestChicken");
        }
      
      
    }
    
}
