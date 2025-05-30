using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParamsPanel : MonoBehaviour
{
    private InputAction m_PressAction;
    private InputAction m_PointerPosition;
    private DragHandler m_StandRotator;

    [SerializeField] public TextMeshProUGUI Param1;
    [SerializeField] public TextMeshProUGUI Param2;
    [SerializeField] public TextMeshProUGUI Param3;

    private void Awake()
    {
        m_PressAction = InputSystem.actions.FindAction("DragStart");
        m_PointerPosition = InputSystem.actions.FindAction("DragPosition");

        if (GameObject.Find("RotatingStand").TryGetComponent<DragHandler>(out m_StandRotator))
            m_StandRotator.DragPerforming += (position, delta) => Param3.text = $"Drag delta: {delta}";
    }

    void Update()
    {
        Param1.text = $"Cursor Pos: {m_PointerPosition.ReadValue<Vector2>()}";
        Param2.text = $"Drag: {m_PressAction.ReadValue<float>() > 0}";
    }
}
