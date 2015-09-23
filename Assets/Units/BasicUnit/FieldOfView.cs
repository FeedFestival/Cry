using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {

    public BaseAI BASEAI;

    void OnTriggerEnter(Collider playerCapsule)
    {
        if (playerCapsule.tag != BASEAI.gameObject.tag && playerCapsule.tag != "Enviorment" && playerCapsule.tag != "Untagged")
        {
            if (BASEAI.debuging)
                Debug.Log("I Saw Enemy");

            if (playerCapsule.transform.parent)
                BASEAI.EnemyFound(playerCapsule.transform.parent.transform.gameObject);
            else
                BASEAI.EnemyFound(playerCapsule.gameObject);
        }
    }

    void OnTriggerStay()
    {

    }
}
