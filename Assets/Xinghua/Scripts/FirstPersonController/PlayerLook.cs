using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private PlayerInputManager inputManager;
    [SerializeField] private Transform character;
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;
    [SerializeField] private float rawLookMultiply = 0.009f;
    Vector2 velocity;
    Vector2 frameVelocity;
    // private float maxRecoilAmount = 10f;

    private float recoilAddSpeed = 3f;
    private float recoilRecoverSpeed = 4f;
    [SerializeField] private float recoilSpeedMultiplay = 1f;
    private float recoilOffsetY = 0f;
    void Reset()
    {
        character = GetComponentInParent<PlayerMovement>().transform;
    }
    private void Awake()
    {
        inputManager = GetComponentInParent<PlayerInputManager>();
    }
    private void OnEnable()
    {
        if (inputManager != null)
        {

            inputManager.OnLookInput += Look;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnLookInput -= Look;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector2 rawLook;


    void Update()
    {
        rawLook = inputManager.inputActions.Player.Look.ReadValue<Vector2>();
        Vector2 rawLookScale = Vector2.Scale(rawLook, Vector2.one * rawLookMultiply);

        Vector2 rawFrameVelocity = Vector2.Scale(rawLookScale, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down 
        /*   transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
           character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);*/
        Shoot shoot = character.GetComponent<Shoot>();
        Debug.Log("camera recoil up");
        if (shoot.isAutoShooting == true)
        {
            Debug.Log("camera recoil up");
            recoilOffsetY += recoilAddSpeed * recoilSpeedMultiplay * Time.deltaTime;
            //recoilOffsetY = Mathf.Clamp(recoilOffsetY, 0f, maxRecoilAmount);
        }
        else
        {
            recoilOffsetY = Mathf.MoveTowards(recoilOffsetY, 0f, recoilRecoverSpeed * recoilSpeedMultiplay * Time.deltaTime);
        }

        float finalY = Mathf.Clamp(velocity.y + recoilOffsetY, -90f, 90f);
        transform.localRotation = Quaternion.AngleAxis(-finalY, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    private void Look(Vector2 dir)
    {
        rawLook = dir;
    }
}
