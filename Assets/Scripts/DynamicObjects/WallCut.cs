using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using System.Collections.Generic;

public class WallCut : MonoBehaviour
{
    public List<GameObject> Walls;
    public List<GameObject> Insides;

    string thisName;
    string wallName;

    void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;

        thisName = this.gameObject.name;
        if (thisName.Length > 3)
        {
            wallName = thisName.Remove(4, 3);

            Transform[] allChildren = new Transform[10];
            if (Walls == null || Walls.Count == 0 || Insides == null || Insides.Count == 0)
                allChildren = this.GetComponentsInChildren<Transform>();

            if (Walls == null || Walls.Count == 0)
            {
                Walls = new List<GameObject>();
                foreach (Transform child in allChildren)
                {
                    if (child.transform.gameObject.name.Contains(wallName) && child.transform.gameObject.name != thisName)
                    {
                        Debug.Log(wallName);
                        Walls.Add(child.transform.gameObject);
                    }
                }
            }
            if (Insides == null || Insides.Count == 0)
            {
                Insides = new List<GameObject>();
                foreach (Transform child in allChildren)
                {
                    if (child.transform.gameObject.name != thisName && child.transform.gameObject.name.Contains(wallName) == false)
                    {
                        Insides.Add(child.transform.gameObject);
                    }
                }
            }
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
                i.SetActive(value);
            }
    }

    void OnMouseEnter()
    {
        Debug.Log("isMouseOverMap");
        GlobalData.Player.isMouseOverMap = true;
    }
    void OnMouseExit()
    {
        Debug.Log("MouseOverMapIs-Not");
        GlobalData.Player.isMouseOverMap = false;
    }
}