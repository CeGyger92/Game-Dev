using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Transform CameraFocus;
    public Camera orthoCam;
    public GameObject dwarf;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;


    // Use this for initialization
    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    public void panZoomOut()
    {
        target = CameraFocus;
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
        dwarf.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        dwarf.GetComponent<PlayerMovement>().frozen = true;
        StartCoroutine("ZoomOut");
    }

    public void panZoomIn()
    {
        target = dwarf.transform;
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
        dwarf.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        dwarf.GetComponent<PlayerMovement>().frozen = false;
        StartCoroutine("ZoomIn");
    }


    IEnumerator ZoomOut()
    {
        while (orthoCam.orthographicSize < 10)
        {
            yield return new WaitForSeconds(.01f);
            orthoCam.orthographicSize += .05f;
        }
    }

    IEnumerator ZoomIn()
    {
        while (orthoCam.orthographicSize > 5)
        {
            yield return new WaitForSeconds(.01f);
            orthoCam.orthographicSize -= .05f;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }  
}
