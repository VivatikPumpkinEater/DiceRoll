using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DiceView : MonoBehaviour
{
    public event Action Click;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _value;

    public bool Interactable { get; set; } = true;

    public Vector3 Position => transform.position;
    public Transform TransformValue => _value.transform;
    
    public void Setup(Sprite sprite, int value)
    {
        _spriteRenderer.sprite = sprite;
        ChangeValue(value);
    }

    /// <summary> Вкл/выкл ролл. Управляет анимацией </summary>
    public void EnableRoll(bool value)
    {
        _animator.enabled = value;
        _value.enabled = !value;
    }

    /// <summary> Изменить значение кубика </summary>
    /// <param name="value"> конечное значение </param>
    public void ChangeValue(int value)
    {
        _value.SetText($"{value}");
    }

    private void OnMouseUpAsButton()
    {
        if (!Interactable)
            return;
        
        Click?.Invoke();
    }
}