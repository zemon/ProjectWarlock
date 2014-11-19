using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour {

    private RectTransform cursorPosition;

    public GameObject inGame;
    public GameObject gameLobby;
    public GameObject mainMenu;

    public Button btn_CreateGame;
    public Button btn_RefreshServerlist;
    public InputField inputfield_EnterName;

    void Awake()
    {
        cursorPosition = GetComponent<RectTransform>();
    }

    void Start()
    {
        InputSettings.mousePosition = new Vector2(Screen.width/2,Screen.height/2);
    }
	
	// Update is called once per frame
    void Update()
    {
        if (mainMenu.activeSelf || gameLobby.activeSelf)
        {
            InputSettings.mousePosition = Input.mousePosition;
        }
        else
        {
            InputSettings.mousePosition.x += Input.GetAxisRaw("Mouse X");
            InputSettings.mousePosition.x = Mathf.Clamp(InputSettings.mousePosition.x, 0, Screen.width);
            InputSettings.mousePosition.y += Input.GetAxisRaw("Mouse Y");
            InputSettings.mousePosition.y = Mathf.Clamp(InputSettings.mousePosition.y, 0, Screen.height);
        }
        cursorPosition.anchoredPosition = InputSettings.mousePosition;
	}

    void OnApplicationFocus(bool focusStatus)
    {
        if (inGame.activeSelf)
        {
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }
    }

    void OnGUI()
    {
    }
}
