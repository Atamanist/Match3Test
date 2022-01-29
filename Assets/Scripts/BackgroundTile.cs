using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : AbstractTile
{
    public override void InicializeTile(int tileType, Point index)
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _index=index;

        _tileType = tileType;

        _image.sprite = BoardGenerator.Instance.BoardSetup.BackgroundSprite[tileType];

        _rectTransform.localPosition = new Vector2(
            (-BoardGenerator.Instance.BoardWidth / 2) + index.x * BoardGenerator.Instance.CellSize,
            (BoardGenerator.Instance.BoardHeight / 2) - index.y * BoardGenerator.Instance.CellSize);
        SetName();
    }

    public override void SetName()
    {
        gameObject.name = $"({_index.x}, {_index.y})+{_tileType}";
    }
}
