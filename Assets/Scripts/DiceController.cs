using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary> Контроллер кубика </summary>
public class DiceController
{
    public event Action RollBegin;
    public event Action RollComplete;

    /// <summary> Максимальный размер текста D20 </summary>
    private const float MaxSize = 2f;
    
    /// <summary> Продолжительность движения </summary>
    private const float MoveDuration = 0.3f;
    
    /// <summary> Продолжительность повышения размера </summary>
    private const float UpScaleDuration = 0.2f;
    
    /// <summary> Продолжительность понижения размера </summary>
    private const float DownScaleDuration = 0.1f;

    private readonly DiceView _diceView;
    private readonly Pool _effects;

    private Sequence _sequence;

    private Vector2 _defaultPosition;
    
    /// <summary> Правый верхний угол экрана в мировых координатах </summary>
    private Vector2 _upperRightScreen;

    /// <summary> Последний отскок был по вертикале? (для более красивово эффекта отскока) </summary>
    private bool _vertical;

    public int CurrentValue { get; private set; }

    public DiceController(DiceView diceView, Pool effects)
    {
        _diceView = diceView;
        _effects = effects;
        
        Init();
    }

    private void Init()
    {
        var camera = Camera.main;
        _upperRightScreen = camera.ScreenToWorldPoint(camera.pixelRect.size);
        _defaultPosition = _diceView.Position;

        _diceView.Click += Rolling;
    }

    private void Rolling()
    {
        RollBegin?.Invoke();
        _diceView.EnableRoll(true);

        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetEase(Ease.Flash);

        for (var i = 0; i < 4; i++)
        {
            _sequence
                .Append(_diceView.transform.DOMove(GetRandomPosition(), MoveDuration))
                .AppendCallback(() => _effects.GetFreeElement(_diceView.transform.position));
        }

        _sequence
            .Append(_diceView.transform.DOMove(_defaultPosition, MoveDuration))
            .OnComplete(() =>
            {
                _diceView.EnableRoll(false);
                var (value, sprite) = DiceConfig.GetRandomSprite();
                _diceView.Setup(sprite, value);
                CurrentValue = value;
                RollComplete?.Invoke();
            });
    }

    private Vector2 GetRandomPosition()
    {
        _vertical = !_vertical;

        if (_vertical)
            return new Vector2(Random.Range(-_upperRightScreen.x, _upperRightScreen.x),
                Random.Range(0, 2) == 0 ? -_upperRightScreen.y : _upperRightScreen.y);

        return new Vector2(Random.Range(0, 2) == 0 ? -_upperRightScreen.x : _upperRightScreen.x,
            Random.Range(-_upperRightScreen.y, _upperRightScreen.y));
    }

    /// <summary> Добавить значение кубика </summary>
    /// <param name="value"> значение которое нужно прибавить </param>
    public Sequence AddValue(int value)
    {
        var sequence = DOTween.Sequence()
            .Append(_diceView.TransformValue.DOScale(MaxSize, UpScaleDuration))
            .AppendCallback(() =>
            {
                CurrentValue += value;
                _diceView.ChangeValue(CurrentValue);
            })
            .Append(_diceView.TransformValue.DOScale(1f, DownScaleDuration));

        return sequence;
    }

    public void Dispose()
    {
        _diceView.Click -= Rolling;
    }
}