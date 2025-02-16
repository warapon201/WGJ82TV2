using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{

    public class ThirdPersonUserControl : MonoBehaviour
    {
   
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private Rigidbody rb;
        public float power;

        public bool automove;
        public float autoSpeed = 1.0f;
        public float frequency;
        private float currentFrequency;
        
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {

        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            //Automatic move
            if(automove)
            {
                currentFrequency += Time.deltaTime;
                if(currentFrequency > this.frequency )
                {
                    currentFrequency = 0.0f;
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move =  UnityEngine.Random.Range(-1.0f,1.0f)*m_CamForward + UnityEngine.Random.Range(-1.0f,1.0f)*m_Cam.right;
                    
                    rb.AddForce(m_Move * Time.deltaTime * power);
                    //m_Move = m_Move * this.autoSpeed;
                    //rb.velocity = m_Move;


                }
                return;
            }


            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            
            rb.AddForce(m_Move * Time.deltaTime * power);
                    }
    }
}
