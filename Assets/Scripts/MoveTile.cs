using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTile : MonoBehaviour
{
    public static MoveTile Instance { get; private set; }

    private Tile _firstTile;
    private Tile _secondTile;
    private bool _isMoving;
    private bool _isRevers;
    private Vector2 _posFirst;
    private Vector2 _posSecond;
    private float _speed;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }

        _speed = StaticConfigurate.SpeedTile;
    }

    public void Move(Tile tile)
    {
        if (_firstTile == null)
            _firstTile = tile;
        else if (_secondTile == null && tile != _firstTile)
        {
            _secondTile = tile;
            if (CheckToMove())
            {
                ForwardMove();
            }
            else
                DropTiles();
        }
        else
            DropTiles();
    }

    private bool CheckToMove()
    {
        if (_secondTile._index.y == _firstTile._index.y)
        {
            if (_secondTile._index.x == _firstTile._index.x + 1 || _secondTile._index.x == _firstTile._index.x - 1)
            {
                return true;
            }
        }
        else if (_secondTile._index.x == _firstTile._index.x)
        {
            if (_secondTile._index.y == _firstTile._index.y + 1 || _secondTile._index.y == _firstTile._index.y - 1)
            {
                return true;
            }
        }
        return false;
    }

    private void ForwardMove()
    {
        _posFirst = _firstTile.gameObject.transform.localPosition;
        _posSecond = _secondTile.gameObject.transform.localPosition;

        Point indexFirst = _firstTile._index;
        Point indexSecond = _secondTile._index;

        BoardGenerator.Instance.SetTileAtPoint(indexFirst, _secondTile);
        BoardGenerator.Instance.SetTileAtPoint(indexSecond, _firstTile);

        _firstTile._index = indexSecond;
        _secondTile._index = indexFirst;


        _isMoving = true;
    }

    private void DropTiles()
    {
        if (_firstTile != null)
        {
            _firstTile.Select(false);
            _firstTile = null;

        }
        if (_secondTile != null)
        {
            _secondTile.Select(false);
            _secondTile = null;
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            _firstTile.gameObject.transform.localPosition = Vector2.MoveTowards(_firstTile.gameObject.transform.localPosition, _posSecond, Time.deltaTime * _speed);
            _secondTile.gameObject.transform.localPosition = Vector2.MoveTowards(_secondTile.gameObject.transform.localPosition, _posFirst, Time.deltaTime * _speed);

            if ((Vector2)_firstTile.gameObject.transform.localPosition == _posSecond && (Vector2)_secondTile.gameObject.transform.localPosition == _posFirst)
            {
                if(_isRevers)
                {
                    DropTiles();
                    _isRevers=false;
                    _isMoving = false;
                }
                else
                {
                    if (BoardGenerator.Instance.VerifyBoardAfterSwitch(_firstTile._index,_secondTile._index))
                    {
                        DropTiles();
                        _isMoving = false;
                    }
                    else
                    {
                        ForwardMove();
                        _isRevers = true;

                    }
                }
            }
        }
    }
}
