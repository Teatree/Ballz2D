using UnityEngine;
using System.Collections.Generic;

public class LaunchPreview : MonoBehaviour
{
    public static Vector3 launchDirection;
    public GameObject ghostGO;

    private GameObject[] ghosts;
    private GameObject[] ghostsReflect;
    private LineRenderer _lineRenderer;
    private Vector3 _dragStartPoint;

    //private int _count = 2;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 3;
        _lineRenderer.useWorldSpace = true;

        ghosts = new GameObject[11];

        for (int i = 0; i <= 10; ++i) {
            ghosts[i] = Instantiate(ghostGO);
            ghosts[i].transform.parent = transform;
        }

        ghostsReflect = new GameObject[6];

        for (int i = 0; i <= 5; ++i) {
            ghostsReflect[i] = Instantiate(ghostGO);
            ghostsReflect[i].transform.parent = transform;
        }
    }

    private void Update() {
        //literally just draw some objects
        if (_lineRenderer != null) {

            for (int i = 0; i < 10; ++i) {
                Vector3 position = Vector3.Lerp(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1), i * 0.1f);
                // TODO : create object at position
                ghosts[i].transform.position = position;
            }

            for (int i = 0; i < 5; ++i) {
                Vector3 position = Vector3.Lerp(_lineRenderer.GetPosition(1), _lineRenderer.GetPosition(2), i * 0.1f);
                // TODO : create object at position
                ghostsReflect[i].transform.position = position;
            }
        }
        //else {
        //    foreach(GameObject ghost in ghosts) {
        //        ghost.transform.position = new Vector3(-100, -100, 0);
        //    }
        //    foreach (GameObject ghostRef in ghostsReflect) {
        //        ghostRef.transform.position = new Vector3(-100, -100, 0);
        //    }
        //}
    }

    public void SetStartPoint(Vector3 worldPoint)
    {
        _dragStartPoint = worldPoint;
        _dragStartPoint.z = 0;
        _lineRenderer.SetPosition(0, _dragStartPoint);
    }

    public void SetEndPoint(Vector3 worldPoint)
    {
        Vector3 pointOffset = worldPoint - _dragStartPoint;
        Vector3 mousePoint = transform.position + pointOffset;

        Vector3 newPoint = mousePoint - _lineRenderer.transform.position;
        mousePoint.z = 0;

        Vector3 endPoint = newPoint.normalized * 2;
        endPoint.z = 0;

        launchDirection = mousePoint + endPoint.normalized * 2;

        _lineRenderer.SetPosition(1, launchDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, launchDirection, 20);
        Debug.DrawRay(hit.point, hit.point *2, Color.green);
        if (hit.collider != null) {
             _lineRenderer.SetPosition(1, hit.point);

            var reflectDir = 2 * (Vector3.Dot(launchDirection, Vector3.Normalize(hit.normal))) * Vector3.Normalize(hit.normal) - launchDirection;
            reflectDir *= -1;

            RaycastHit2D reflectRay = Physics2D.Raycast(hit.point*0.999999f, reflectDir, 20);
            //Debug.Log("reflectDir " + reflectDir + " reflectRay.point " + reflectRay.point + " reflectRay colliding with: " + reflectRay.collider.name);

            _lineRenderer.SetPosition(2, reflectRay.point);
        }
    }
}