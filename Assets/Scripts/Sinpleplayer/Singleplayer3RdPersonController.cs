using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{ 
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class Singleplayer3RdPersonController : MonoBehaviour
{
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public bool started=false;
        public bool ended=false;
       // string number = "1";
        bool sprint=false;
        private void Start()
        {
          
            // get the third person character ( this should never be null due to require component )
            m_Cam = transform.GetChild(0);
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump && started && !ended)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
            if((Input.GetKeyDown(KeyCode.LeftShift)|| Input.GetKeyDown(KeyCode.RightShift)) && !sprint)
            {
                sprint = !sprint;
                m_Character.Sprint();

            }
            if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) && sprint)
            {
                sprint = !sprint;
                m_Character.Walk();
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (started && !ended)
            {
                // calculate move direction to pass to character
                if (m_Cam != null)
                {
                    // calculate camera relative direction to move:
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    if (v > 0)
                        m_Move = v * m_CamForward + h * m_Cam.right;
                    else
                        m_Move = 0 * m_CamForward + h * m_Cam.right;
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }
                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, m_Jump);
                m_Jump = false;
            }
            else if(ended)
            {
                m_Character.Move(Vector3.zero, crouch, false);
            }
            
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Chicken"))
            {
                SinglePlayerScoreBoardScript.instance.AnimChickenCollected();
            }
        }
    }

}

