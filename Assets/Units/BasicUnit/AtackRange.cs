using UnityEngine;
using System.Collections;

public class AtackRange : MonoBehaviour {

    public BaseAI BASEAI;

    void Awake() {
        BASEAI = this.transform.parent.GetComponent<BaseAI>();
    }

    void OnTriggerEnter(Collider objectInside)
    {
        if (objectInside.tag != BASEAI.gameObject.tag)
        {
            if (BASEAI.debuging)
                Debug.Log("Enemy is near in " + this.gameObject.name);

            BASEAI.EnemyNear();
        }
    }

    void OnTriggerExit(Collider objectOutside)
    {
        if (objectOutside.tag != BASEAI.gameObject.tag)
        {
            if (BASEAI.debuging)
                Debug.Log("Enemy left " + this.gameObject.name);
        }
    }
}
