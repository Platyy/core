using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
