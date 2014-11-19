using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager : MonoBehaviour {

    public Button StartGame;

	// Use this for initialization
	void Start ()
    {
        StartGame.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Network.isServer && !StartGame.gameObject.activeSelf)
        {
            StartGame.gameObject.SetActive(true);
        }
	}
}
