using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public abstract class AbstractTile : MonoBehaviour
{
    protected int _tileType;

    protected Image _image;

    protected RectTransform _rectTransform;

    public Point _index;

    public abstract void InicializeTile(int tileType, Point index);

    public abstract void SetName();
}
