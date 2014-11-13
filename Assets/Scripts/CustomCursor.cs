using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour {

    public RectTransform cursorPosition;

    void Awake()
    {
        cursorPosition = GetComponent<RectTransform>();
        Screen.lockCursor = true;
        Screen.showCursor = false;
    }

    void Start()
    {
        InputSettings.mousePosition = new Vector2(Screen.width/2,Screen.height/2);
    }
	
	// Update is called once per frame
    void Update()
    {
        InputSettings.mousePosition.x += Input.GetAxisRaw("Mouse X");
        InputSettings.mousePosition.x = Mathf.Clamp(InputSettings.mousePosition.x, 0, Screen.width);
        InputSettings.mousePosition.y += Input.GetAxisRaw("Mouse Y");
        InputSettings.mousePosition.y = Mathf.Clamp(InputSettings.mousePosition.y, 0, Screen.height);
        cursorPosition.anchoredPosition = InputSettings.mousePosition;
        

	}



    void OnGUI()
    {
    }
}
