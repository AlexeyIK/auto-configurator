using UnityEngine;
using UnityEngine.UIElements;

public abstract class PanelControllerBase : MonoBehaviour
{
    /// <summary>
    /// Controllable VisualElement
    /// </summary>
    protected VisualElement _panel;

    protected virtual void Awake()
    {
        _panel = GetPanelVisualElement();
    }

    /// <summary>
    /// Specifying controllable VisualElement
    /// </summary>
    /// <returns></returns>
    protected abstract VisualElement GetPanelVisualElement();

    /// <summary>
    /// Background click reactions
    /// </summary>
    /// <param name="go"></param>
    protected abstract void OnBackgroundClick(GameObject go);

    /// <summary>
    /// To hide any panel
    /// </summary>
    protected virtual void HidePanel()
    {
        _panel.parent.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// To show any panel
    /// </summary>
    protected virtual void ShowPanel()
    {
        _panel.parent.style.display = DisplayStyle.Flex;
    }
}
