using UnityEngine;
using System.Collections;

public class GroundedStateManager : MonoBehaviour
{
    public void OnCollisionStay(Collision collision)
    {
        m_isGrounded = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        m_isGrounded = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        m_isGrounded = true;
    }

    public bool isGrounded
    {
        get
        {
            return m_isGrounded;
        }
    }

    private bool m_isGrounded;

    // Use this for initialization
    void Start()
    {
        m_isGrounded = false;
    }


}
