using UnityEngine;


namespace RotationScriptNameSpace
{
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast object should rotate")]
    private float rotationForce = 0.05f;
    private GameObject rotatableObject;
    private Collider objectCollider;
    private bool shouldRotate = false;
    private Vector3 lastMousePos;
    private Vector2 mousePosDelta = new Vector2(0, 0);

    void Start()
    {
        Input.simulateMouseWithTouches = false;
        lastMousePos = Input.mousePosition;
        rotatableObject = gameObject;
        objectCollider = rotatableObject.GetComponent<Collider>();
    }

    void Update()
    {   
        UpdateMousePosDelta();
        checkTouchOnCollider();
        CheckSwipesRotation();
    }

    void CheckSwipesRotation()  // CHECK SWIPES
    {
        if(!shouldRotate) return;
        float angle = -rotationForce * GetSwipeLength();
        Vector3 axis = GetSwipeAxisPerp();
        Vector3 centrePoint = rotatableObject.transform.position;

        rotatableObject.transform.RotateAround(centrePoint, axis, angle);
    }

    Vector3 GetSwipeAxisPerp()  // GETTING AXIS TO ROTATE AROUND
    {
        Vector2 dir;
        if(Input.GetMouseButton(0))
        {
            dir = GetMousePosDelta().normalized;
            return new Vector3(-dir.y, dir.x, 0);
        }
        if(Input.touchCount == 0) return new Vector3(0, 0, 0);
        Touch touch = Input.GetTouch(0);
        dir = touch.deltaPosition.normalized;
        return new Vector3(-dir.y, dir.x, 0);
    }

    float GetSwipeLength()  //SWIPE VECTOR MAGNITUDE
    {
        if(Input.GetMouseButton(0)) return GetMousePosDelta().magnitude;  // MOUSE CHECK
        if(Input.touchCount == 0) return 0;
        Touch touch = Input.GetTouch(0);
        return touch.deltaPosition.magnitude;
    }

    void checkTouchOnCollider()  // CHECK IF TOUCH IS ON OBJECT'S COLLIDER
    {
        Ray ray;
        RaycastHit hit;
        if(Input.GetMouseButtonUp(0))
        {
            shouldRotate = false;
            return;
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(objectCollider.Raycast(ray, out hit, Mathf.Infinity) && Input.GetMouseButtonDown(0)) shouldRotate = true;
            
        if(Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Ended)
        {
            shouldRotate = false;
            return;
        }
        ray = Camera.main.ScreenPointToRay(touch.position);

        if(objectCollider.Raycast(ray, out hit, Mathf.Infinity) && touch.phase == TouchPhase.Began) shouldRotate = true; 
    }

    Vector2 GetMousePosDelta()
    {
        return mousePosDelta;
    }

    void UpdateMousePosDelta()  // UPDATING MOUSE POSITION DELTA
    {
        mousePosDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;
    }

}

}





