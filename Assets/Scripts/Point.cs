using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int nx, int ny)
    { x = nx; y = ny; }

    public bool Equals(Point p)
    { return (x == p.x && y == p.y); }

    public static Point mult(Point p, int m)
    { return new Point(p.x * m, p.y * m); }

    public static Point add(Point p, Point o)
    { return new Point(p.x + o.x, p.y + o.y); }

    public static Point up
    { get { return new Point(0, 1); } }

    public static Point down
    { get { return new Point(0, -1); } }

    public static Point left
    { get { return new Point(-1, 0); } }

    public static Point right
    { get { return new Point(1, 0); } }
}
