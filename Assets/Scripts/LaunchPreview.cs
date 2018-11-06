using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    //[SerializeField] private float _maxIterations = 5;
    //[SerializeField] private float _maxDistance = 10f;
    public static Vector3 launchDirection;

    private LineRenderer _lineRenderer;
    private Vector3 _dragStartPoint;

    //private int _count = 2;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 3;
        _lineRenderer.useWorldSpace = true;
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

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + endPoint.normalized * 2);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + endPoint);

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, endPoint.normalized * 2, 20);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, endPoint, 20);
        Debug.DrawRay(hit.point, hit.point *2, Color.green);
        if (hit.collider != null) {
            Debug.Log(">>>line  hit point: " + hit.point);
            Debug.Log(">>>line  ray: " + ray.origin);
            //var reflectV2 = Vector2.Reflect(ray.origin, hit.normal);
            var reflectV2 = Vector2.Reflect(hit.point, hit.normal);
            Debug.Log(">>> rf: " + reflectV2);
           // RaycastHit2D ricochetHit = Physics2D.Raycast(hit.point + hit.normal, reflectV2);
            RaycastHit2D ricochetHit = Physics2D.Raycast(hit.point + hit.normal, reflectV2);
            _lineRenderer.SetPosition(1, hit.point);
            _lineRenderer.SetPosition(2, ricochetHit.point);
        }
    }
}