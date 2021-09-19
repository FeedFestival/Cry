using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vision : MonoBehaviour
{
    private MeshRenderer _parentVision;
    public bool StartWithValue = false;

    public List<GameObject> Walls;
    public List<GameObject> Insides;

    void Start()
    {
        _parentVision = transform.parent.transform.gameObject.GetComponent<MeshRenderer>();
        _parentVision.enabled = StartWithValue;

        if (Insides.Count > 0)
            foreach (GameObject i in Insides)
            {
                i.SetActive(false);
            }
    }

    void OnTriggerEnter(Collider unitObject)
    {
        if (unitObject.CompareTag("Player"))
        {
            ShowWall(false);
            _parentVision.enabled = true;
        }
    }
    void OnTriggerExit(Collider unitObject)
    {
        if (unitObject.CompareTag("Player"))
        {
            ShowWall(true);
            _parentVision.enabled = false;
        }
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
