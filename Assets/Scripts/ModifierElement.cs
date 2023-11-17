using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Эллемент модификатора </summary>
public class ModifierElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _modifierTxt;
    [SerializeField] private TMP_Text _nameTxt;
    [SerializeField] private Image _icon;
    
    /// <summary> Значение модификатора </summary>
    public int Value { get; private set; }

    public Vector3 DefaultValuePosition { get; private set; }

    public Transform TransformValue => _modifierTxt.transform;

    public void Init(ModifierConfig.Model model)
    {
        Value = model.Value;

        _icon.sprite = model.Icon;
        _modifierTxt.SetText($"+{Value}");
        _nameTxt.SetText(model.Name);
    }

    public void SetDefaultPosition()
    {
        DefaultValuePosition = _modifierTxt.transform.position;
    }
}
