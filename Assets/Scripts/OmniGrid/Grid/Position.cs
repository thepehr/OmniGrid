using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Position
{
    public int x;
    public int y;
    public Position(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

    public int ManhatanMag {
        get 
        {
            return Mathf.Abs(x) + Mathf.Abs(y);
        }
    }

    public HashSet<Position> GetNeighbors(){
		var l = new HashSet<Position>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (Mathf.Abs(i) + Mathf.Abs(j) == 1){
					l.Add(this + new Position(i, j));
				}
			}
		}
		return l;
	}

	public HashSet<Position> GetAllNeighbors(){
		var l = new HashSet<Position>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (Mathf.Abs(i) + Mathf.Abs(j) >= 1){
					l.Add(this + new Position(i, j));
				}
			}
		}
		return l;
	}

	public static Position mouse {
		get {
			return GetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}


    public static Position right {
		get {
			return new Position(1, 0);
		}
	}

	public static Position left {
		get {
			return new Position(-1, 0);
		}
	}

	public static Position up {
		get {
			return new Position(0, 1);
		}
	}
    
	public static Position down {
		get {
			return new Position(0, -1);
		}
	}

    public static Position GetPosition(Vector2 pos){
		return new Position(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
	}

    public Vector3 GetWorldPosition()
    {
        return new Vector3(x, y, 0);
    }

	public override string ToString()
	{
		return "(" + x.ToString() + ", " + y.ToString() + ")";
	}

	public override int GetHashCode()
	{
		return 100000 * this.x + this.y;
	}

	public override bool Equals(object obj)
	{
		var o = (Position)obj;
		return this.x == o.x && this.y == o.y;
	}

	public static Position Clamp(Position value, Position min, Position max)
	{
		return new Position(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
	}

	public static bool operator ==(Position p1, Position p2)
	{
		if (ReferenceEquals(p1, p2))
		{
			return true;
		}
		if (ReferenceEquals(p1, null))
		{
			return false;
		}
		if (ReferenceEquals(p2, null))
		{
			return false;
		}
		return p1.x == p2.x && p1.y == p2.y;
	}

	public static bool operator !=(Position p1, Position p2)
	{
		return !(p1 == p2);
	}

	public static Position operator +(Position p1, Position p2)
	{
		if (ReferenceEquals(p1, null))
		{
			return new Position(-1, -1);
            //TODO
		}
		if (ReferenceEquals(p2, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        return new Position(p1.x + p2.x, p1.y + p2.y);
	}

	public static Position operator -(Position p1, Position p2)
	{
		if (ReferenceEquals(p1, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        if (ReferenceEquals(p2, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        return new Position(p1.x - p2.x, p1.y - p2.y);
	}

	public static Position operator -(Position p1)
	{
		if (ReferenceEquals(p1, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        return new Position(-p1.x, -p1.y);
	}

	public static Position operator *(Position p1, int modifier)
	{
		if (ReferenceEquals(p1, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        return new Position(p1.x * modifier, p1.y * modifier);
	}
	
	public static Position operator /(Position p1, int modifier)
	{
		if (ReferenceEquals(p1, null))
		{
            return new Position(-1, -1);
            //TODO
        }
        return new Position(p1.x / modifier, p1.y / modifier);
	}

}
