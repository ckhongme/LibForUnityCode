using UnityEditor;
using UnityEngine.UIElements;

public class TesUiElement : EditorWindow
{
    public void OnEnable()
    {
        var root = this.rootVisualElement;
        IntSlider slider = new IntSlider();
        root.Add(slider);
    }
}
