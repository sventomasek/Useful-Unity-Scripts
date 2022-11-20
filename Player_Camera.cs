using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    public Transform target;

    [Header("Settings")]
    [SerializeField] Vector2 fixedOffset = new Vector2(0, 0);
    [SerializeField] Vector2 smoothTime = new Vector2(0.05f, 0.05f);
    [Space]
    [SerializeField] Vector2 mouseMoveAmount = new Vector2(12, 12);
    [SerializeField] Vector2 joystickMoveAmount = new Vector2(2, 2);

    private Vector2 mouseOffset;
    private Vector2 joystickOffset;
    private Vector2 velocity;

    private Vector2 focusPoint;
    private bool usingController;

    void Update()
    {
        // Mouse Offset
        if (mouseMoveAmount != Vector2.zero)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            focusPoint.x = (target.position.x + mousePos.x) / mouseMoveAmount.x;
            focusPoint.y = (target.position.y + mousePos.y) / mouseMoveAmount.y;
        }

        // Switch to Mouse Input
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) usingController = false;

        // Controller Offset
        if (joystickMoveAmount != Vector2.zero) joystickOffset = new Vector2(Input.GetAxis("Horizontal Right") * joystickMoveAmount.x, Input.GetAxis("Vertical Right") * joystickMoveAmount.y);
        
        // Switch to Controller Input
        if (joystickOffset != Vector2.zero) usingController = true;
    }

    void FixedUpdate()
    {
		// Camera movement
        if (target != null)
        {
            float posX = 0;
            float posY = 0;

            // Calculate cameras position based on the targets position
            if (usingController)
            {
                posX = Mathf.SmoothDamp(transform.position.x, target.position.x + fixedOffset.x + joystickOffset.x, ref velocity.x, smoothTime.x);
                posY = Mathf.SmoothDamp(transform.position.y, target.position.y + fixedOffset.y + joystickOffset.y, ref velocity.y, smoothTime.y);
            }
            else
            {
                posX = Mathf.SmoothDamp(transform.position.x, target.position.x + fixedOffset.x + focusPoint.x, ref velocity.x, smoothTime.x);
                posY = Mathf.SmoothDamp(transform.position.y, target.position.y + fixedOffset.y + focusPoint.y, ref velocity.y, smoothTime.y);
            }

            // Apply cameras position
            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }
}
