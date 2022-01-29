using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(RectTransform), typeof(Image),typeof(Animation))]

public class Tile : AbstractTile, IPointerDownHandler
{
    private Animation _anim;

    public int TileType
    {
        get { return _tileType; }
        set { _tileType = value; _image.sprite = BoardGenerator.Instance.BoardSetup.TileSprite[value]; SetName(); }
    }

    public override void InicializeTile(int tileType, Point index)
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _index = index;
        _anim = GetComponent<Animation>();


        _tileType = tileType;

        _image.sprite = BoardGenerator.Instance.BoardSetup.TileSprite[tileType];

        _rectTransform.localPosition = new Vector2(
            (-BoardGenerator.Instance.BoardWidth / 2) + BoardGenerator.Instance.CellSize / 2 + index.x * BoardGenerator.Instance.CellSize,
            (BoardGenerator.Instance.BoardHeight / 2) - BoardGenerator.Instance.CellSize / 2 - index.y * BoardGenerator.Instance.CellSize);
        SetName();

        _anim.Play("CreateTile");
    }

    public override void SetName()
    {
        gameObject.name = $"({_index.x}, {_index.y})+{_tileType}";

    }

    public void Delete()
    {
        _anim.Play("DeletTile");
        Destroy(gameObject,_anim.GetClip("DeletTile").length);
    }

    public void Select(bool select)
    {
        if (select)
            _image.color = Color.blue;
        else
            _image.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select(true);
        MoveTile.Instance.Move(this);
    }

}
