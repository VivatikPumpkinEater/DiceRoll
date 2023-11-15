using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifierElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _modifierTxt;
    [SerializeField] private Image _icon;
    
    public int Value { get; private set; }

    public void Init(Sprite icon, int modifierValue)
    {
        _icon.sprite = icon;
        _modifierTxt.SetText($"+{modifierValue}");
        Value = modifierValue;
    }
}
