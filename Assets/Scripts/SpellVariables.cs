using UnityEngine;
using System.Collections;

public class SpellVariables : MonoBehaviour {
    public int team;
	// Use this for initialization
	void Start () {
        ChangeColor(team);
	}
    private void ChangeColor(int t)
    {
        switch (t)
        {
            case 0:
                gameObject.renderer.material.color = Color.green;
                break;
            case 1:
                gameObject.renderer.material.color = Color.blue;
                break;
            case 2:
                gameObject.renderer.material.color = Color.red;
                break;
            case 3:
                gameObject.renderer.material.color = Color.yellow;
                break;
            default:
                break;
        }
    }

}
