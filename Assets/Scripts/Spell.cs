using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

    public int speed;
    public int team;
    public int force;
    public int range;

	// Use this for initialization
	void Start ()
    {
        ChangeColor(team);
        Destroy(gameObject,range);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
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
