using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    private static readonly int Roll = Animator.StringToHash("Roll");

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Button _test;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private ParticleSystem _hit;

    private Sequence _sequence;

    private Vector2 _defaultPosition;

    private void Start()
    {
        _test.onClick.AddListener(Rolling);
        Init();
    }

    private void Init()
    {
        _defaultPosition = transform.position;
    }

    private void Rolling()
    {
        _animator.enabled = true;
        _test.interactable = false;
        _value.gameObject.SetActive(false);

        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetEase(Ease.Flash);

        // for (var i = 0; i < _positions.Length; i++)
        // {
        //     _sequence
        //         .Append(transform.DOMove(_positions[i], 0.3f))
        //         .AppendCallback(ShowHit);
        // }

        _sequence
            .Append(transform.DOMove(_defaultPosition, 0.15f))
            .OnComplete(() =>
            {
                _animator.enabled = false;
                _test.interactable = true;
                _value.gameObject.SetActive(true);
                var (value, sprite) = DiceConfig.GetRandomSprite();
                _spriteRenderer.sprite = sprite;
                _value.SetText($"{value}");
            });
    }

    private void ShowHit()
    {
        var hitEffect = Instantiate(_hit, transform.position, Quaternion.identity);
    }
}