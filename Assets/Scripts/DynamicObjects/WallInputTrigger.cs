using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class WallInputTrigger : MonoBehaviour
{

    public WallLedgeType LedgeType;

    private float Ypos;
    private float groundYPos;

    private Vector3 UILinePosition;

    private Vector3 StartPointPosition;

    private GameObject StartPoint;

    public Transform thisTransform;

    private Vector3 lastPosition = new Vector3();

    void Start()
    {
        thisTransform = this.transform;
        Ypos = thisTransform.position.y;

        switch (LedgeType)
        {
            case WallLedgeType.TwoMetters:
                groundYPos = Ypos - 2f;
                break;
            case WallLedgeType.ThreeMetters:
                groundYPos = Ypos - 3f;
                break;
            case WallLedgeType.FourMetters:
                groundYPos = Ypos - 4f;
                break;
            default:
                break;
        }
    }

    void OnMouseEnter()
    {
        // Change cursor towall climb cursor
    }

    void OnMouseOver()
    {
        // Calculate base on the point where we want to climb.
        CalculateStartPoint();

        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            if (!StartPoint)
                StartPoint = new GameObject();

            StartPoint.transform.position = StartPointPosition;
        }

        Debug.DrawLine(lastPosition, UILinePosition);
        Debug.DrawLine(lastPosition, StartPointPosition);
    }

    void OnMouseExit()
    {

    }

    void CalculateStartPoint()
    {
        RaycastHit Hit;
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Ray, out Hit, 100))
        {
            Hit.point = new Vector3( Mathf.Round(Hit.point.x * 100f) / 100f,Mathf.Round(Hit.point.y * 100f) / 100f,Mathf.Round(Hit.point.z * 100f) / 100f);
            if (lastPosition != Hit.point)
            {
                lastPosition = Hit.point;

                if (Hit.point.y == thisTransform.position.y)
                {
                    var _UILinePosition = new Vector3(Hit.point.x, Ypos, Hit.point.z);

                    var One_meterInFront = (Vector3)(thisTransform.forward + _UILinePosition);
                    var One_meterInFront_Half_metterDown = One_meterInFront + new Vector3(0, -0.5f, 0);
                    var Half_metterDown = _UILinePosition + new Vector3(0, -0.5f, 0);

                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(One_meterInFront_Half_metterDown, (Half_metterDown - One_meterInFront_Half_metterDown)), out hit, 10))
                    {
                        ShowLine(hit);
                    }
                }
                else
                {
                    ShowLine(Hit);
                }
            }
        }
    }

    private void ShowLine(RaycastHit hit)
    {
        UILinePosition = new Vector3(hit.point.x, Ypos, hit.point.z);
        StartPointPosition = new Vector3(hit.point.x, groundYPos, hit.point.z);
    }
}
