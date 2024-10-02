using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{
    public Transform target;
    public float pointerSensivityX = 1f;
    public float pointerSensivityY = -1f;
    public float zoomSensivity = 1;
    public float rawDistanceFromTarget = .1f;

    [SerializeField] private float mouseSensivityConstantFactor = 1;
    [SerializeField] private float touchSensivityConstantFactor = .2f;
    [SerializeField] private float mouseZoomSensivityConstantFactor = -0.05f;
    [SerializeField] private float touchZoomSensivityConstantFactor = 0.005f;
    [SerializeField] private float distanceBottomBorder = 3f;
    [SerializeField] private float angleBorderX = 90f;
    [SerializeField] private float distanceTopBorder = 60f;
    [SerializeField] private float powerOfDistanceFromTarget = 10f;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        float distanceFromTarget = GetFinalDistanceFromTarget();
        SetPosition(distanceFromTarget);
    }

    void Update()
    {
        // Handle mouse
        if (Input.GetMouseButton(1))
        {
            var mouseDeltaPos = Input.mousePositionDelta * mouseSensivityConstantFactor;
            float deltaPosX = mouseDeltaPos.x;
            float deltaPosY = mouseDeltaPos.y;
            float zoomDelta = Input.mouseScrollDelta.y * mouseZoomSensivityConstantFactor;

            if (deltaPosX != 0f || deltaPosY != 0f)
            {
                transform.localEulerAngles = GetLocalEulerAngles(deltaPosX, deltaPosY);
            }

            if (zoomDelta != 0)
            {
                SetRawDistanceFromTarget(zoomDelta);
            }

            float distanceFromTarget = GetFinalDistanceFromTarget();
            SetPosition(distanceFromTarget);
        }

        // Handle touch
        if (Input.touchCount == 1)
        {
            var touchDeltaPos = Input.GetTouch(0).deltaPosition * touchSensivityConstantFactor;
            float deltaPosX = touchDeltaPos.x;
            float deltaPosY = touchDeltaPos.y;

            transform.localEulerAngles = GetLocalEulerAngles(deltaPosX, deltaPosY);
            float distanceFromTarget = GetFinalDistanceFromTarget();
            SetPosition(distanceFromTarget);
        }

        if (Input.touchCount == 2)
        {
            var firstTouch = Input.GetTouch(0);
            var secondTouch = Input.GetTouch(1);

            var prevFirstPos = firstTouch.position - firstTouch.deltaPosition;
            var prevSecondPos = secondTouch.position - secondTouch.deltaPosition;

            var currFirstPos = firstTouch.position;
            var currSecondPos = secondTouch.position;

            var prevMagnitude = (prevFirstPos -  prevSecondPos).magnitude;
            var currMagnitude = (currFirstPos - currSecondPos).magnitude;

            float zoomDelta = (prevMagnitude - currMagnitude) * touchZoomSensivityConstantFactor;

            SetRawDistanceFromTarget(zoomDelta);
            float distanceFromTarget = GetFinalDistanceFromTarget();
            SetPosition(distanceFromTarget);
        }
    }

    Vector3 GetLocalEulerAngles(float deltaPosX, float deltaPosY)
    {
        rotationX += deltaPosY * pointerSensivityY;
        rotationY += deltaPosX * pointerSensivityX;

        rotationX = Mathf.Clamp(
            rotationX,
            -angleBorderX,
            angleBorderX
        );

        return new Vector3(
            rotationX,
            rotationY,
            0
        );
    }

    void SetRawDistanceFromTarget(float zoomDelta)
    {
        rawDistanceFromTarget += zoomDelta * zoomSensivity;

        rawDistanceFromTarget = Mathf.Clamp(
            rawDistanceFromTarget,
            0,
            .12f
        );
    }

    float GetFinalDistanceFromTarget()
    {
        float baseNumber = 1.2f;
        var exponent = rawDistanceFromTarget * 100;

        return Mathf.Pow(
            baseNumber,
            exponent
        ) + distanceBottomBorder - 1;
    }

    void SetPosition(float distanceFromTarget)
    {
        var targetPosition = target.position;
        transform.localPosition = targetPosition - transform.forward * distanceFromTarget;
    }
}
