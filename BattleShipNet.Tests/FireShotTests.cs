
namespace BattleShipNet.Facts;
public class FireShotFacts
{
    #region "Board Setup"
    /// <summary>
    /// Let's set up a board as follows:
    /// Destroyer: (1,8) (2,8)
    /// Cruiser: (3,1) (3,2) (3,3)
    /// Sub: (1,5) (2,5) (3,5)
    /// Battleship: (10,6) (10,7) (10,8) (10, 9)
    /// Carrier: (4,4) (5,4) (6,4) (7,4) (8,4)
    /// 
    ///    1 2 3 4 5 6 7 8 9 10
    ///  1     R
    ///  2     R
    ///  3     R
    ///  4       C C C C C
    ///  5 S S S
    ///  6                   B
    ///  7                   B
    ///  8 D D               B
    ///  9                   B
    /// 10
    /// </summary>
    /// <returns>A board that is ready to play</returns>
    private Board SetupBoard()
    {
        Board board = new();

        PlaceDestroyer(board);
        PlaceCruiser(board);
        PlaceSubmarine(board);
        PlaceBattleship(board);
        PlaceCarrier(board);

        return board;
    }

    private void PlaceCarrier(Board board)
    {
        var request = new PlaceShipRequest()
        {
            Coordinate = new Coordinate(4, 4),
            Direction = ShipDirection.Right,
            ShipType = ShipType.Carrier
        };

        board.PlaceShip(request);
    }

    private void PlaceBattleship(Board board)
    {
        var request = new PlaceShipRequest()
        {
            Coordinate = new Coordinate(6, 10),
            Direction = ShipDirection.Down,
            ShipType = ShipType.Battleship
        };

        board.PlaceShip(request);
    }

    private void PlaceSubmarine(Board board)
    {
        var request = new PlaceShipRequest()
        {
            Coordinate = new Coordinate(5, 3),
            Direction = ShipDirection.Left,
            ShipType = ShipType.Submarine
        };

        board.PlaceShip(request);
    }

    private void PlaceCruiser(Board board)
    {
        var request = new PlaceShipRequest()
        {
            Coordinate = new Coordinate(3, 3),
            Direction = ShipDirection.Up,
            ShipType = ShipType.Cruiser
        };

        board.PlaceShip(request);
    }

    private void PlaceDestroyer(Board board)
    {
        var request = new PlaceShipRequest()
        {
            Coordinate = new Coordinate(8, 1),
            Direction = ShipDirection.Right,
            ShipType = ShipType.Destroyer
        };

        board.PlaceShip(request);
    }
    #endregion

    [Fact]
    public void CoordinateEquality()
    {
        var c1 = new Coordinate(5, 10);
        var c2 = new Coordinate(5, 10);

        Assert.Equal(c1, c2);
    }

    [Fact]
    public void CanNotFireOffBoard()
    {
        var board = SetupBoard();

        var coordinate = new Coordinate(15, 20);

        var response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Invalid, response.ShotStatus);
    }

    [Fact]
    public void CanNotFireDuplicateShot()
    {
        var board = SetupBoard();

        var coordinate = new Coordinate(5, 5);
        var response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Miss, response.ShotStatus);

        // fire same shot
        response = board.FireShot(coordinate);
        Assert.Equal(ShotStatus.Duplicate, response.ShotStatus);
    }

    [Fact]
    public void CanMissShip()
    {
        var board = SetupBoard();

        var coordinate = new Coordinate(5, 5);
        var response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Miss, response.ShotStatus);
    }

    [Fact]
    public void CanHitShip()
    {
        var board = SetupBoard();

        var coordinate = new Coordinate(1, 3);
        var response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Hit, response.ShotStatus);
        Assert.Equal("Cruiser", response.ShipImpacted);
    }

    [Fact]
    public void CanSinkShip()
    {
        var board = SetupBoard();

        var coordinate = new Coordinate(6, 10);
        var response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Hit, response.ShotStatus);
        Assert.Equal("Battleship", response.ShipImpacted);

        coordinate = new Coordinate(7, 10);
        response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Hit, response.ShotStatus);
        Assert.Equal("Battleship", response.ShipImpacted);

        coordinate = new Coordinate(8, 10);
        response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.Hit, response.ShotStatus);
        Assert.Equal("Battleship", response.ShipImpacted);

        coordinate = new Coordinate(9, 10);
        response = board.FireShot(coordinate);

        Assert.Equal(ShotStatus.HitAndSunk, response.ShotStatus);
        Assert.Equal("Battleship", response.ShipImpacted);
    }

    [Fact]
    public void CanWinGame()
    {
        var board = SetupBoard();

        Assert.Equal(ShotStatus.HitAndSunk, SinkDestroyer(board).ShotStatus);
        Assert.Equal(ShotStatus.HitAndSunk, SinkCruiser(board).ShotStatus);
        Assert.Equal(ShotStatus.HitAndSunk, SinkSubmarine(board).ShotStatus);
        Assert.Equal(ShotStatus.HitAndSunk, SinkBattleship(board).ShotStatus);
        Assert.Equal(ShotStatus.Victory, SinkCarrier(board).ShotStatus);
    }

    private FireShotResponse SinkCarrier(Board board)
    {
        var coordinate = new Coordinate(4, 4);
        board.FireShot(coordinate);

        coordinate = new Coordinate(4, 5);
        board.FireShot(coordinate);

        coordinate = new Coordinate(4, 6);
        board.FireShot(coordinate);

        coordinate = new Coordinate(4, 7);
        board.FireShot(coordinate);

        coordinate = new Coordinate(4, 8);
        return board.FireShot(coordinate);
    }

    private FireShotResponse SinkBattleship(Board board)
    {
        var coordinate = new Coordinate(6, 10);
        board.FireShot(coordinate);

        coordinate = new Coordinate(7, 10);
        board.FireShot(coordinate);

        coordinate = new Coordinate(8, 10);
        board.FireShot(coordinate);

        coordinate = new Coordinate(9, 10);
        return board.FireShot(coordinate);
    }

    private FireShotResponse SinkSubmarine(Board board)
    {
        var coordinate = new Coordinate(5, 1);
        board.FireShot(coordinate);

        coordinate = new Coordinate(5, 2);
        board.FireShot(coordinate);

        coordinate = new Coordinate(5, 3);
        return board.FireShot(coordinate);
    }

    private FireShotResponse SinkCruiser(Board board)
    {
        var coordinate = new Coordinate(1, 3);
        board.FireShot(coordinate);

        coordinate = new Coordinate(2, 3);
        board.FireShot(coordinate);

        coordinate = new Coordinate(3, 3);
        return board.FireShot(coordinate);
    }

    private FireShotResponse SinkDestroyer(Board board)
    {
        var coordinate = new Coordinate(8, 1);
        board.FireShot(coordinate);

        coordinate = new Coordinate(8, 2);
        return board.FireShot(coordinate);
    }
}
