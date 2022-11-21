using System;

namespace BattleShipNet.BLL.Requests;

public class Coordinate
{
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }

    public Coordinate(int x, int y)
    {
        XCoordinate = x;
        YCoordinate = y;
    }

    public override bool Equals(object obj)
    {
        Coordinate otherCoordinate = obj as Coordinate;

        if (otherCoordinate == null)
            return false;

        return otherCoordinate.XCoordinate == XCoordinate &&
               otherCoordinate.YCoordinate == YCoordinate;
    }

    public override int GetHashCode()
    {
        string uniqueHash = XCoordinate.ToString() + YCoordinate.ToString() + "00";
        return Convert.ToInt32(uniqueHash);
    }

}
