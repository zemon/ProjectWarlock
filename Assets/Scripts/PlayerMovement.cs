using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Transform myTransform;
    private Rigidbody myRigidBody;
    private Vector3 targetPosition;
    private float targetDistance;
    Quaternion newRotation;

    public LayerMask floorMask;

    public float movementSpeed = 5;
    public float rotationSpeed = 0.1f;
    private bool shouldMove;

    //Network
    private Vector3 syncPos;
    private Quaternion syncRot;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion syncStartRotation = Quaternion.identity;
    private Quaternion syncEndRotation = Quaternion.identity;
    public int sp = 0;

    void Awake()
    {
        lastSynchronizationTime = Time.time;
        myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        targetPosition = myTransform.position;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        Quaternion syncRotation = Quaternion.identity;

        if (stream.isWriting)
        {
            syncPosition = myRigidBody.position;
            stream.Serialize(ref syncPosition);

            syncVelocity = myRigidBody.velocity;
            stream.Serialize(ref syncVelocity);

            syncRotation = myRigidBody.rotation;
            stream.Serialize(ref syncRotation);

        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);
            stream.Serialize(ref syncRotation);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = myRigidBody.position;
            syncEndRotation = syncRotation;
            syncStartRotation = myRigidBody.rotation;
        }
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (networkView.isMine)
        {
            MineMovement();
        }
        else
        {
            SyncedMovement();
        }
	}

    private void MineMovement()
    {
        targetDistance = Vector3.Distance(targetPosition, myTransform.position);

        if (targetDistance < 0.5f)
        {
            shouldMove = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray camRay = Camera.main.ScreenPointToRay(InputSettings.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask))
            {
                shouldMove = true;
                targetPosition = floorHit.point;
                targetPosition.y = myTransform.position.y;

                Vector3 playerToMouse = targetPosition - transform.position;
                newRotation = Quaternion.LookRotation(playerToMouse);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            myRigidBody.AddForce(Quaternion.AngleAxis(0, Vector3.up) * transform.forward * -400);
        }

        if (shouldMove)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, targetPosition, movementSpeed * Time.deltaTime);
            myRigidBody.rotation = Quaternion.Lerp(myRigidBody.rotation, newRotation, rotationSpeed);
        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        myTransform.position = Vector3.Lerp(myTransform.position, syncEndPosition, syncTime * 5);
        myTransform.rotation = syncEndRotation;
    }
}
