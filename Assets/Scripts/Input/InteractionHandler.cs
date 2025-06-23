using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class InteractionHandler : MonoBehaviour
{
    private bool isHover;
    private bool isCaptured;
    private Camera _camera;

    private Outline outline;

    private InputAction pointerClickAction;
    private InputAction pointerPositionAction;

    [SerializeField] private bool _isActive = true;
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private bool _hoverHighlight = true;
    [SerializeField] private bool _clickHighlight = true;

    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;

            if (!_isActive)
            {
                SwitchOutline(false);
                isHover = false;
                isCaptured = false;
            }
        }
    }

    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;

    public UnityEvent<GameObject> OnClick;
    public UnityEvent<GameObject> OnRelease;

    private void Awake()
    {
        _camera = Camera.main;

        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 4f;
        outline.enabled = false;

        pointerPositionAction = InputSystem.actions.FindAction("DragPosition");
        pointerClickAction = InputSystem.actions.FindAction("Click");

        pointerPositionAction.performed += OnPointerMove;
        pointerClickAction.performed += OnPointerClick;
    }

    private void OnDestroy()
    {
        pointerPositionAction.performed -= OnPointerMove;
        pointerClickAction.performed -= OnPointerClick;

        var collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        var isPressed = context.ReadValueAsButton();

        if (isHover && isPressed)
        {
            if (_clickHighlight)
                SwitchOutline(true, true);

            if (_debugMode)
                Debug.Log($"[{context.ReadValueAsButton()}] Click on object {gameObject.name}");

            isCaptured = true;
            OnClick?.Invoke(gameObject);
        }
        else if (isCaptured)
        {
            SwitchOutline(false);

            if (_debugMode)
                Debug.Log($"[{context.ReadValueAsButton()}] Release object {gameObject.name}");

            isCaptured = false;
            OnRelease?.Invoke(gameObject);
        }
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        if (!_isActive)
            return;

        Vector2 mousePosition = context.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, 10))
        {
            if (hit.transform == transform)
            {
                if (!isHover)
                    PointerEnter();

                return;
            }
        }

        if (isHover)
            PointerLeave();
    }

    public void PointerEnter()
    {
        if (_hoverHighlight)
            SwitchOutline(true);

        isHover = true;
        OnHoverStart?.Invoke();
    }

    public void PointerLeave()
    {
        SwitchOutline(false);

        isHover = false;
        OnHoverEnd?.Invoke();
    }

    private void SwitchOutline(bool isEnable, bool isPress = false)
    {
        outline.OutlineColor = isPress ? Color.cyan : Color.red;
        outline.enabled = isEnable;
    }
}
