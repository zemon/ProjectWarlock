using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

    private Transform playerTransform;
    private Vector3 offset;
    private Transform myTransform;
    private bool paused;

    void Awake()
    {
        if (!networkView.isMine)
        {
            Destroy(gameObject);
        }
        myTransform = GetComponent<Transform>();
        playerTransform = transform.parent.transform;
        offset = myTransform.position - playerTransform.position;
        transform.parent = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myTransform.position = playerTransform.position + offset;
        }
        else
        {
            PanWithMouse();
        }

	}

    void PanWithMouse()
    {
        if (!paused)
        {
            if (InputSettings.mousePosition.x > Screen.width - 20)
            {
                myTransform.position += new Vector3(1, 0, 0);
            }
            else if (InputSettings.mousePosition.x < 20)
            {
                myTransform.position += new Vector3(-1, 0, 0);
            }
            else if (InputSettings.mousePosition.y < 20)
            {
                myTransform.position += new Vector3(0, 0, -1);
            }
            else if (InputSettings.mousePosition.y > Screen.height - 20)
            {
                myTransform.position += new Vector3(0, 0, 1);
            } 
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        paused = !focusStatus;
    }
}
