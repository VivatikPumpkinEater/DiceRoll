using System.Collections.Generic;
using UnityEngine;

public class DiceConfig : BaseConfig<DiceConfig>
{
    [SerializeField] private List<Sprite> _sprites;

    public static (int, Sprite) GetRandomSprite()
    {
        var index = Random.Range(0, Instance._sprites.Count);
        return (index + 1, Instance._sprites[index]);
    }
}
