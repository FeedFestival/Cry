using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vision : MonoBehaviour
{
    MeshRenderer parentVision;
    public bool startWithValue = false;

    public List<GameObject> Walls;
    public List<GameObject> Insides;

    void Start()
    {
        parentVision = this.transform.parent.transform.gameObject.GetComponent<MeshRenderer>();
        parentVision.enabled = startWithValue;

        if (Insides.Count > 0)
            foreach (GameObject i in Insides)
            {
                i.SetActive(false);
            }
    }

    void OnTriggerEnter(Collider unitObject)
    {
        Debug.Log(unitObject);
        ShowWall(false);
        parentVision.enabled = true;
    }
    void OnTriggerExit(Collider unitObject)
    {
        ShowWall(true);
        parentVision.enabled = false;
    }

    public void ShowWall(bool value)
    {
        if (Walls.Count > 0)
            foreach (GameObject e in Walls)
            {
                e.SetActive(value);
            }
        if (Insides.Count > 0)
            foreach (GameObject i in Insides)
            {
                i.SetActive(!value);
            }
    }
}
