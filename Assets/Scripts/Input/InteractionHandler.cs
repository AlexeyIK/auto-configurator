using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class InteractionHandler : MonoBehaviour
{
    private bool _isHover;
    private bool _isCaptured;
    private Camera _camera;

    private Outline _outline;

    private InputAction _pointerClickAction;
    private InputAction _pointerPositionAction;

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
                _isHover = false;
                _isCaptured = false;
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

        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.red;
        _outline.OutlineWidth = 4f;
        _outline.enabled = false;

        _pointerPositionAction = InputSystem.actions.FindAction("DragPosition");
        _pointerClickAction = InputSystem.actions.FindAction("Click");

        _pointerPositionAction.performed += OnPointerMove;
        _pointerClickAction.performed += OnPointerClick;
    }

    private void OnDestroy()
    {
        _pointerPositionAction.performed -= OnPointerMove;
        _pointerClickAction.performed -= OnPointerClick;

        var collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        var isPressed = context.ReadValueAsButton();

        if (_isHover && isPressed)
        {
            if (_clickHighlight)
                SwitchOutline(true, true);

            if (_debugMode)
                Debug.Log($"[{context.ReadValueAsButton()}] Click on object {gameObject.name}");

            _isCaptured = true;
            OnClick?.Invoke(gameObject);
        }
        else if (_isCaptured)
        {
            SwitchOutline(false);

            if (_debugMode)
                Debug.Log($"[{context.ReadValueAsButton()}] Release object {gameObject.name}");

            _isCaptured = false;
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
                if (!_isHover)
                    PointerEnter();

                return;
            }
        }

        if (_isHover)
            PointerLeave();
    }

    public void PointerEnter()
    {
        if (_hoverHighlight)
            SwitchOutline(true);

        _isHover = true;
        OnHoverStart?.Invoke();
    }

    public void PointerLeave()
    {
        SwitchOutline(false);

        _isHover = false;
        OnHoverEnd?.Invoke();
    }

    private void SwitchOutline(bool isEnable, bool isPress = false)
    {
        _outline.OutlineColor = isPress ? Color.cyan : Color.red;
        _outline.enabled = isEnable;
    }
}
