using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitFoV : MonoBehaviour
{
    public float viewRadius = 30f;
    [Range(0, 360)]
    public float viewAngle = 360f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution = 1.5f;
    public int edgeResolveIterations = 3;
    public float edgeDstThreshold = 0.1f;

    public float maskCutawayDst = 0.1f;

    public MeshFilter viewMeshFilter;   // this guy needs to be a child of the caracter, it represents the actual circle that will show the masked texture;
    public MeshFilter viewVerticalMeshFilter;   // this guy needs to be a child of the caracter, it represents the actual circle that will show the masked texture;
    Mesh viewMesh;
    Mesh verticalViewMesh;

    // unit y position
    float unitYPos = 0f;

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        verticalViewMesh = new Mesh();
        verticalViewMesh.name = "View Mesh";

        viewMeshFilter.mesh = viewMesh;

        viewVerticalMeshFilter.mesh = verticalViewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(-transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        List<Vector3> viewPointsDown = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);

                        viewPointsDown.Add(new Vector3(edge.pointA.x, unitYPos, edge.pointA.z));
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);

                        viewPointsDown.Add(new Vector3(edge.pointB.x, unitYPos, edge.pointB.z));
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            viewPointsDown.Add(new Vector3(newViewCast.point.x, unitYPos, newViewCast.point.z));
            oldViewCast = newViewCast;
        }

        if (viewPointsDown.Count > 0)
            CreateCustom(viewPointsDown);

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutawayDst;
        }

        for (int i = 0; i < vertexCount - 1; i++)
        {
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    void CreateCustom(List<Vector3> viewPointsDown)
    {
        List<Vector3> full = new List<Vector3>();

        for (int i = 0; i < viewPointsDown.Count - 1; i++)
        {
            if (Vector3.Distance(viewPointsDown[i], this.gameObject.transform.position) < 9f && Vector3.Distance(viewPointsDown[i], viewPointsDown[i + 1]) < 4f)
            {
                full.Add(viewPointsDown[i]);
                full.Add(new Vector3(viewPointsDown[i].x, unitYPos + 3.5f, viewPointsDown[i].z));
            }
            //if (i > 3)
            //    break;
        }

        int vertexCount = full.Count;
        Vector3[] vertices = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i] = transform.InverseTransformPoint(full[i]) + Vector3.forward * maskCutawayDst;
            if (i + 1 < vertexCount)
                if (i % 2 == 0)
                {
                    Debug.DrawLine(full[i], full[i + 1], Color.green);
                }
                else
                {
                    Debug.DrawLine(full[i], full[i + 1], Color.blue);
                }
        }

        int[] triangles = new int[(vertexCount - 1) * 3];
        int t = 0;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            if (i != 0 && i < vertexCount - 1)
            {
                if (i % 2 == 0)
                {
                    triangles[t] = i;
                    triangles[t + 1] = i - 1;
                    triangles[t + 2] = i + 1;
                }
                else
                {
                    triangles[t] = i;
                    triangles[t + 1] = i + 1;
                    triangles[t + 2] = i - 1;
                }
                t = t + 3;
            }
        }

        verticalViewMesh.Clear();

        verticalViewMesh.vertices = vertices;
        verticalViewMesh.triangles = triangles;
        verticalViewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
