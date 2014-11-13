using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Transform myTransform;
    private Rigidbody myRigidBody;
    private Vector3 targetPosition;
    private float targetDistance;
    Quaternion newRotation;

    public LayerMask floorMask;

    private float movementSpeed = 5;
    private float rotationSpeed = 0.1f;
    private bool shouldMove;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        targetPosition = myTransform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        targetDistance = Vector3.Distance(targetPosition, myTransform.position);

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
        
        if (targetDistance < 0.5f)
        {
            shouldMove = false;
        }
        if (shouldMove)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, targetPosition, movementSpeed * Time.deltaTime);
            myRigidBody.rotation = Quaternion.Lerp(myRigidBody.rotation, newRotation, rotationSpeed);
        }
	}
}
