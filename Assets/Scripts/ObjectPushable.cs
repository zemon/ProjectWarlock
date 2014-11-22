using UnityEngine;
using System.Collections;

public class ObjectPushable : MonoBehaviour {

    public void Push(Vector3 dir, int force)
    {
        Debug.Log("Dir" + dir + "force" + force);
        rigidbody.AddForce(dir * force);
    }
}
