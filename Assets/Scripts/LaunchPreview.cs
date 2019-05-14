using UnityEngine;

public class LaunchPreview : MonoBehaviour {
    public static Vector3 launchDirection;

    [Header("Ghosts(boo)")]
    public GameObject ghostGO;
    [Tooltip("Default is 1")]
    public float ghostScale = 1;
    public Color ghostColor = Color.white;

    private GameObject[] ghosts = new GameObject[11];
    private GameObject[] ghostsReflect = new GameObject[6];
    private LineRenderer _lineRenderer;
    private Vector3 _dragStartPoint;
    private Vector3 baseOffsetDirection = Vector3.down * 0.1f;

    [Header("Launch Preview Settings")]
    public bool active = false;
    public LayerMask myWallsMask;


    private void Start() {
        Init();
        ghosts = new GameObject[30];
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i] = Instantiate(ghostGO);
            ghosts[i].transform.parent = transform;
            ghosts[i].transform.position = new Vector3(-100, -100, 0);
            ghosts[i].transform.localScale = new Vector3(ghostScale, ghostScale, ghostScale);
            ghosts[i].GetComponent<SpriteRenderer>().color = ghostColor;
        }

        ghostsReflect = new GameObject[10];

        for (int i = 0; i < ghostsReflect.Length; i++) {
            ghostsReflect[i] = Instantiate(ghostGO);
            ghostsReflect[i].transform.parent = transform;
            ghostsReflect[i].transform.position = new Vector3(-100, -100, 0);
            ghostsReflect[i].transform.localScale = new Vector3(ghostScale, ghostScale, ghostScale);
            ghostsReflect[i].GetComponent<SpriteRenderer>().color = ghostColor;
        }
    }

    public void Init() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 3;
        _lineRenderer.useWorldSpace = true;
    }

    private void Update() {
        if (_lineRenderer != null && active) {
            float distance = Vector2.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

            float betweenBalls = 0f;

            for (int i = 0; i < ghosts.Length; i++) {
                betweenBalls += 0.3f;
                Vector3 position = Vector3.MoveTowards(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1), betweenBalls);
                ghosts[i].transform.position = position;
            }

            betweenBalls = 0f;
            for (int i = 0; i < ghostsReflect.Length; i++) {
                betweenBalls += 0.3f;
                Vector3 position = Vector3.MoveTowards(_lineRenderer.GetPosition(1), _lineRenderer.GetPosition(2), betweenBalls);
                ghostsReflect[i].transform.position = position;
            }
        }
    }

    public void SetStartPoint(Vector3 worldPoint) {
        _dragStartPoint = worldPoint;
        _dragStartPoint.z = 0;
        _lineRenderer.SetPosition(0, _dragStartPoint);
    }

    public void SetEndPoint(Vector3 worldPoint) {
        
        Vector3 pointOffset = worldPoint - _dragStartPoint;
        Vector3 mousePoint = transform.position + pointOffset;

        Vector3 newPoint = mousePoint - _lineRenderer.transform.position;
        mousePoint.z = 0;

        BallLauncher.aimCanceled = newPoint.y < 0; // I'm so coder
        if(BallLauncher.aimCanceled)
            BallLauncher.Instance.HideGhosts();

        Vector3 endPoint = newPoint.normalized * 2;
        endPoint.z = 0;

        launchDirection = /*mousePoint +*/ endPoint.normalized * 2; // magic


        //Debug.Log(" launchDirection = " + launchDirection + " mousePoint = " + mousePoint + "endPoint = " + endPoint.normalized * 2);

        //_lineRenderer.SetPosition(1, launchDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, launchDirection, 20, myWallsMask);
        Debug.DrawRay(hit.point, hit.point * 2, Color.green);
        if (hit.collider != null) {
            //Debug.Log("hit " + hit.point + " hit " + hit.transform.gameObject.name);
            _lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y-0.1f)); // -0.1f so the balls follow a bit more preciely 

            Vector3 offsetDirection = Vector3.zero;
            offsetDirection = baseOffsetDirection;
     
            var reflectDir = Vector3.Reflect(launchDirection, hit.normal)/* + offsetDirection*/;
            //Debug.Log("hit.normal = " + hit.normal + " launchDirection = " + launchDirection + " , BUT reflectDir);

            RaycastHit2D reflectRay = Physics2D.Raycast(hit.point * 0.999999f, reflectDir, 20, myWallsMask);

            _lineRenderer.SetPosition(2, reflectRay.point);
        }
    }
}