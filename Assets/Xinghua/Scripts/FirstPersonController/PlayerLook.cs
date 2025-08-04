using System.Collections;
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
    private float recoilRecoverSpeed = 1f;
    [SerializeField] private float recoilSpeedMultiplay = 1f;
    private float recoilOffsetY = 0f;
    private Gun gun;
    Vector2 rawLook;
    Coroutine resetCoroutine;
    [SerializeField] private float recoilAmount = 0.2f;
    [HideInInspector]
    public bool isAiming = false;
    [SerializeField] private float aimSensitivityMultiplier = 0.0005f;
    private float normalFOV = 60f;
    private float aimFOV = 40f;
    private float fovTransitionSpeed = 10f;
    [SerializeField] float fovTransitionTime = 0.2f;
    private float targetFOV;

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
            inputManager.OnAimInputStart += AimStart;
            inputManager.OnAimInputCancle += AimCancel;
            inputManager.OnShootCanceled += ResetCameraPositon;
                }
        else
        {
            Debug.Log("input manager is null ");
        }

        gun = GetComponentInChildren<Gun>();
        gun.OnShoot += HandleShoot;
    }



    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnLookInput -= Look;
            inputManager.OnAimInputStart -= AimStart;
            inputManager.OnAimInputCancle -= AimCancel;
            inputManager.OnShootCanceled -= ResetCameraPositon;
        }
        else
        {
            Debug.Log("input manager is null ");
        }

        gun = GetComponentInChildren<Gun>();
        gun.OnShoot -= HandleShoot;
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalY = transform.position.y;
    }
    private void HandleShoot()
    {
        CameraUP();
        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(CameraPositonReset());

    }
    float originalY;
    private void CameraUP()
    {
        isUp = true;
        recoilOffsetY += recoilAddSpeed * recoilSpeedMultiplay * recoilAmount;

    }
    private IEnumerator CameraPositonReset()
    {
        yield return new WaitForEndOfFrame();
        recoilOffsetY = Mathf.MoveTowards(recoilOffsetY, originalY, recoilRecoverSpeed * recoilSpeedMultiplay * recoilAmount);
  
    }
    private bool isUp = false;
    private void ResetCameraPositon()
    {
        isUp = false;
        recoilOffsetY = 0;
       
    }
    private void AimStart()
    {
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        if (playerMovement.isSprinting == true) return;
        isAiming = true;
        targetFOV = aimFOV;
    }
    void AimCancel()
    {
        isAiming = false;
        targetFOV = normalFOV;
        if (fovCoroutine != null)
            StopCoroutine(fovCoroutine);
        fovCoroutine = StartCoroutine(SmoothFOV(Camera.main.fieldOfView, normalFOV, fovTransitionTime));
    }
    private void HandleCameraFOV()
    {
        float currentFOV = Camera.main.fieldOfView;
        float lerpFactor = Time.deltaTime * fovTransitionSpeed;
        float nextFOV = Mathf.Lerp(currentFOV, targetFOV, lerpFactor);
        Camera.main.fieldOfView = nextFOV;
    }
    Coroutine fovCoroutine;

    IEnumerator SmoothFOV(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Camera.main.fieldOfView = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.fieldOfView = to;
    }
    void Update()
    {
        rawLook = inputManager.inputActions.Player.Look.ReadValue<Vector2>();
        Vector2 rawLookScale = Vector2.Scale(rawLook, Vector2.one * rawLookMultiply);
        float currentSensitivity;

        if (isAiming)
        {
            currentSensitivity = sensitivity * aimSensitivityMultiplier;
            HandleCameraFOV();
        }
        else
        {
            currentSensitivity = sensitivity;

        }

        Vector2 rawFrameVelocity = Vector2.Scale(rawLookScale, Vector2.one * currentSensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);


        Shoot shoot = character.GetComponent<Shoot>();
        if (isUp)
        {
           
            float finalY = Mathf.Clamp(velocity.y + recoilOffsetY, -90f, 90f);
            transform.localRotation = Quaternion.AngleAxis(-finalY, Vector3.right);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        }
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    private void Look(Vector2 dir)
    {
        rawLook = dir;
    }
}
