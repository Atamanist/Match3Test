using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator Instance { get; private set; }

    public int CellSize { get; private set; }
    public int BoardWidth { get; private set; }
    public int BoardHeight { get; private set; }
    public BoardSetup BoardSetup { get; private set; }

    [Header("Список досок с настройками")]
    [SerializeField] private BoardSetup[] _boardsWithSettings;

    [Header("Объект для создания бэкграунда")]
    [SerializeField] private RectTransform _backgroundBoard;
    [Header("Префаб тайла бэкграунда")]
    [SerializeField] private BackgroundTile _backgroundTile;

    [Header("Объект для создания тайлов игровых")]
    [SerializeField] private RectTransform _tileBoard;
    [Header("Префаб тайла игрового")]
    [SerializeField] private Tile _tile;

    private System.Random _random = new System.Random();

    private Tile[,] _filedTileBoard;


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

        StartGame();
    }

    private void StartGame()
    {
        CreateBoard();
        FillBoardBackground();
        FillBoardTile();
        VerifyBoard();
    }

    private void CreateBoard()
    {
        BoardSetup = _boardsWithSettings[_random.Next(0, _boardsWithSettings.Length - 1)];
        CellSize = StaticConfigurate.CellSize;
        BoardWidth = CellSize * StaticConfigurate.Width;
        BoardHeight = CellSize * StaticConfigurate.Height;
        _backgroundBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BoardWidth);
        _backgroundBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BoardHeight);
        _tileBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BoardWidth);
        _tileBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BoardHeight);
    }

    private void FillBoardBackground()
    {
        for (int y = 0; y < StaticConfigurate.Height; y++)
        {
            for (int x = 0; x < StaticConfigurate.Width; x++)
            {
                Point point = new Point(x, y);
                switch (BoardSetup.BoardLayout.rows[y].row[x])
                {
                    case StaticConfigurate.BackgroundTileType.Block:
                        Instantiate(_backgroundTile, _backgroundBoard).InicializeTile((int)StaticConfigurate.BackgroundTileType.Block, point);
                        break;
                    case StaticConfigurate.BackgroundTileType.Any:
                        Instantiate(_backgroundTile, _backgroundBoard).InicializeTile((int)StaticConfigurate.BackgroundTileType.Any, point);
                        break;
                }
            }
        }
    }

    private void FillBoardTile()
    {
        _filedTileBoard = new Tile[BoardWidth, BoardHeight];
        for (int y = 0; y < StaticConfigurate.Height; y++)
        {
            for (int x = 0; x < StaticConfigurate.Width; x++)
            {
                Point point = new Point(x, y);
                switch (BoardSetup.BoardLayout.rows[y].row[x])
                {
                    case StaticConfigurate.BackgroundTileType.Block:
                        break;
                    case StaticConfigurate.BackgroundTileType.Any:
                        _filedTileBoard[x, y] = Instantiate(_tile, _tileBoard);
                        _filedTileBoard[x, y].InicializeTile(_random.Next(0, BoardSetup.TileSprite.Length - 1), point);
                        break;
                }
            }
        }
    }


    void VerifyBoard()
    {
        for (int y = 0; y < StaticConfigurate.Height; y++)
        {
            for (int x = 0; x < StaticConfigurate.Width; x++)
            {
                Point p = new Point(x, y);
                int val = GetValueAtPoint(p);
                if (val <= 0) continue;
                VerificateRespawnTile(p, val);
            }
        }
    }

    private int GetValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= StaticConfigurate.Width || p.y < 0 || p.y >= StaticConfigurate.Height|| _filedTileBoard[p.x, p.y]==null) return -1;
        return _filedTileBoard[p.x, p.y].TileType;
    }

    private List<Point> IsConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = GetValueAtPoint(p);
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        foreach (Point dir in directions)
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if (GetValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) 
                AddPoints(ref connected, line); 
        }
        for (int i = 0; i < 2; i++) 
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            foreach (Point next in check)
            {
                if (GetValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }
            if (same > 1)
                AddPoints(ref connected, line);
        }
        for (int i = 0; i < 4; i++) 
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check)
            {
                if (GetValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }
            if (same > 2)
                AddPoints(ref connected, square);
        }
        if (main)
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, IsConnected(connected[i], false));
        }
        return connected;
    }

    private void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    private void SetValueAtPoint(Point p, int v)=>_filedTileBoard[p.x, p.y].TileType = v;

    public void SetTileAtPoint(Point p, Tile t)=>_filedTileBoard[p.x, p.y] = t;

    private int NewValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < BoardSetup.TileSprite.Length - 1; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[_random.Next(0, available.Count)];
    }

    public bool VerifyBoardAfterSwitch(Point a, Point b)
    {
        List<Point> pointsFirst = IsConnected(a, true);
        List<Point> pointsSecond = IsConnected(b, true);

        if (pointsFirst.Count != 0 || pointsSecond.Count != 0)
        {
            foreach (Point p in pointsFirst)
            {
                if (_filedTileBoard[p.x, p.y] != null)
                {
                    RespawnTile(p);
                    int val = GetValueAtPoint(p);
                    if (val <= 0) continue;
                    VerificateRespawnTile(p, val);
                }
            }
            foreach (Point p in pointsSecond)
            {
                if (_filedTileBoard[p.x, p.y] != null)
                {
                    RespawnTile(p);
                    int val = GetValueAtPoint(p);
                    if (val <= 0) continue;
                    VerificateRespawnTile(p, val);
                }
            }
            return true;
        }
        else
            return false;
    }

    private void RespawnTile(Point p)
    {
        _filedTileBoard[p.x, p.y].Delete();
        _filedTileBoard[p.x, p.y] = Instantiate(_tile, _tileBoard);
        _filedTileBoard[p.x, p.y].InicializeTile(_random.Next(0, BoardSetup.TileSprite.Length - 1), p);
    }

    private void VerificateRespawnTile(Point p,int val)
    {
        List<int> remove = new List<int>();
        while (IsConnected(p, true).Count > 0)
        {
            val = GetValueAtPoint(p);
            if (!remove.Contains(val))
                remove.Add(val);
            SetValueAtPoint(p, NewValue(ref remove));
        }
    }
}

