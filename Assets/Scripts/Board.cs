using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary> Класс игрового поля, связующий класс между всеми компонентами </summary>
public class Board : MonoBehaviour
{
    [SerializeField] private DiceView _diceView;

    [SerializeField] private ModifierElement _modifierElement;

    [SerializeField] private int _minDifficulty;
    [SerializeField] private int _maxDifficulty;

    [Header("UI Settings")]
    [SerializeField] private TMP_Text _difficultyClassTxt;
    [SerializeField] private GameObject _helpBox;
    [SerializeField] private HorizontalLayoutGroup _modifierGrid;
    [SerializeField] private TMP_Text _totalBonusTxt;
    [SerializeField] private Button _randomizeBtn;

    private readonly List<ModifierElement> _modifierElements = new();

    private Sequence _sequence;
    private DiceController _diceController;
    private Vector3 _defaultDifficultyPosition;

    private Pool _winEffectPool;
    private Pool _loseEffectPool;

    private int _currentDifficulty;
    private int _currentTotalBonus;

    private void Start()
    {
        //Создаем пулы (можно было сделать и универсальный пул, но в рамках тестового решил сделать как проще)
        _winEffectPool = new Pool(VFXPoolConfig.GetVfx(VFXType.Win), transform);
        _loseEffectPool = new Pool(VFXPoolConfig.GetVfx(VFXType.Lose), transform);
        var reboundPool = new Pool(VFXPoolConfig.GetVfx(VFXType.Rebound), transform);

        _diceController = new DiceController(_diceView, reboundPool);
        _diceController.RollBegin += OnRollBegin;
        _diceController.RollComplete += OnRollComplete;

        _randomizeBtn.onClick.AddListener(Setup);

        Setup();
    }

    /// <summary> Когда ролл начался отключаем возможность взаимодействия с d20, отключаем подсказку </summary>
    private void OnRollBegin()
    {
        _helpBox.SetActive(false);
        _diceView.Interactable = false;
    }

    /// <summary> По завершению ролла запускаются анимации циферок </summary>
    private void OnRollComplete()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        _defaultDifficultyPosition =
            _difficultyClassTxt.transform.position; // Т.к Canvas перерисовывает объекты не в том же кадре,
                                                    // что бы была корректная позиция я устанавливаю её перед использованием

        var isWin = _currentDifficulty < _currentTotalBonus + _diceController.CurrentValue;
                                                    
        foreach (var modifierElement in _modifierElements)
        {
            modifierElement.SetDefaultPosition(); //Таже причина что и с _defaultDifficultyPosition

            _sequence
                //Анимация движения, изменения размера значения модификатора
                .Append(modifierElement.TransformValue.DOScale(1.2f, 0.1f))
                .Append(modifierElement.TransformValue.DOMove(_diceView.TransformValue.position, 0.2f))
                .Append(modifierElement.TransformValue.DOMove(modifierElement.DefaultValuePosition, 0.1f))
                .Join(modifierElement.TransformValue.DOScale(1f, 0.1f))
                //Добавляем значение к кубику
                .Append(_diceController.AddValue(modifierElement.Value));
        }
        
        _sequence
            //Двигаем значение сложности
            .Append(_difficultyClassTxt.transform.DOMove(_diceView.TransformValue.position, 0.2f))
            //Запускаем нужный эффект
            .AppendCallback(ResultEffect(isWin))
            //Возвращаем значение сложности на место
            .Append(_difficultyClassTxt.transform.DOMove(_defaultDifficultyPosition, 0.1f))
            //Завершаем ролл - включаем взаимодействие с кубиком и подсказку
            .OnComplete(() =>
            {
                _diceView.Interactable = true;
                _helpBox.SetActive(true);
            });
    }

    /// <summary> Возвращает корректный эффект победа/проигрыш </summary>
    private TweenCallback ResultEffect(bool win)
    {
        var currentPool = win ? _winEffectPool : _loseEffectPool;
        return () => currentPool.GetFreeElement(_diceView.Position);
    }

    /// <summary> Установка цели, модификаторов </summary>
    private void Setup()
    {
        //Если есть модификаторы - чистим
        foreach (var modifierElement in _modifierElements)
            Destroy(modifierElement.gameObject);
        _modifierElements.Clear();
        
        _currentTotalBonus = 0;

        var count = Random.Range(1, 4);
        var modifierModels = ModifierConfig.GetModifiers(count);

        for (var i = 0; i < count; i++)
        {
            var modifierElement = Instantiate(_modifierElement, _modifierGrid.transform);
            var model = modifierModels[i];
            modifierElement.Init(model);
            _modifierElements.Add(modifierElement);
            _currentTotalBonus += modifierElement.Value;
        }

        _currentDifficulty = Random.Range(_minDifficulty, _maxDifficulty + 1);

        _difficultyClassTxt.SetText($"{_currentDifficulty}");
        _totalBonusTxt.SetText($"Total bonus +{_currentTotalBonus}");
    }

    private void OnDestroy()
    {
        _diceController.RollBegin -= OnRollBegin;
        _diceController.RollComplete -= OnRollComplete;
        _diceController.Dispose();

        _randomizeBtn.onClick.RemoveListener(Setup);
    }
}