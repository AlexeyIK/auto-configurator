using UnityEngine;
using UnityEngine.UIElements;

public abstract class PanelControllerBase : MonoBehaviour
{
    protected UIDocument document;
    /// <summary>
    /// Controllable VisualElement
    /// </summary>
    protected VisualElement panel;

    protected virtual void Awake()
    {
        document = GetComponent<UIDocument>();
        panel = GetPanelVisualElement();
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
    protected virtual void OnBackgroundClick(GameObject go)
    {
        if (go == gameObject)
            return;
    }

    /// <summary>
    /// To hide any panel
    /// </summary>
    public virtual void HidePanel()
    {
        panel.SetEnabled(false);
        panel.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// To show any panel
    /// </summary>
    public virtual void ShowPanel()
    {
        panel.SetEnabled(true);
        panel.style.display = DisplayStyle.Flex;
    }
}
