using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    // Used for the player speed.
    public float speed;
    // Used for the camera rotation speed.
    public float sensibility;
    // Used to determine the force jump
    public float jumpForce;

    private Rigidbody m_rigidbody;
    private Camera m_mainCamera;
    private GroundedStateManager m_grounded;
    private float m_currentRotation;

	// Use this for initialization
	void Start () {
        m_rigidbody = this.GetComponent<Rigidbody>();
        m_mainCamera = this.GetComponentInChildren<Camera>();
        m_grounded = this.GetComponentInChildren<GroundedStateManager>();
        m_currentRotation = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        // We define the vector to follow.
        // This are the values in zqsd or arrows keys (depending of the user).

        float hValue = Input.GetAxis("Horizontal");
        float vValue = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");

        // Here we stop the player immediatly.
        // In order to stop only the "side vector" we want in the first place to
        // get the down vector of the  rigidbody. 
        // so we change the base of the vector.
        Matrix4x4 worldToLocal = this.transform.worldToLocalMatrix;
        Vector3 localVector = worldToLocal * m_rigidbody.velocity;
        // We get only the "Y" component.
        localVector = new Vector3(0.0f, localVector.y, 0.0f);
        // We "stop" the rigidbody.
        m_rigidbody.velocity = Vector3.zero;
        // And add the correct Gravity Vector.
        Matrix4x4 localToWorld = this.transform.localToWorldMatrix;
        m_rigidbody.velocity = localToWorld * localVector;

        // We want to see if the controller move.
        if (hValue != 0 || vValue != 0)
        {
            // On each local axis of the player we apply whether or not there is an input.
            // The result if the final force to apply to the rigidBody.
            Vector3 moveVect = (this.transform.forward * vValue + this.transform.right * hValue).normalized * speed;
            
            m_rigidbody.velocity += moveVect;
        }

        if (m_grounded.isGrounded && jump >= 1.0f)
        {
            Vector3 jumpVector = localToWorld * (Vector3.up * jumpForce);
            m_rigidbody.velocity += jumpVector;
        }
        
    }

    void LateUpdate()
    {
        // Here we want to apply a rotation to the camera
        // on its right axis
        // But to the whole body on his up axis.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        float offsetRotation = mouseY * sensibility * Time.deltaTime;
        // This condidtions are for clamping the rotation onto the right axis
        if(m_currentRotation + offsetRotation > 85.0f)
        {
            offsetRotation = 85.0f - m_currentRotation;
            m_currentRotation = 85.0f;
        }else if(m_currentRotation + offsetRotation < -85.0f)
        {
            offsetRotation = -(85.0f - m_currentRotation);
            m_currentRotation = -85.0f;
        }
        else
        {
            m_currentRotation += offsetRotation;
            // The camera rotation on its local right axis with the value of mouse Y;
            m_mainCamera.transform.Rotate(Vector3.right, offsetRotation);
        }
        
        
        

        // Here the body rotation with the value of the mouse X
        this.transform.Rotate(this.transform.up, mouseX * sensibility * Time.deltaTime);

        

    }
}
