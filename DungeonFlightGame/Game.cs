namespace DungeonFlightGame;

// This is just a fancy constructor (kinda like in Kotlin, tho not available in older versions)
// In that case, just convert it into a regular constructor...
public readonly struct Position(int row, int column)
{
    public readonly int Row = row;
    public readonly int Column = column;

    public override string ToString()
    {
        return $"{Row}, {Column}";
    }
}

public enum Direction
{
    North,
    East,
    South,
    West,
}

public sealed class Game
{
    // TODO: Map shouldn't be exposed like this, since we don't want it getting modified outside.
    public int[,] Map { get; }

    public Position PlayerPosition { get; private set; }

    public int PlayerHealth => HealthAtPosition(PlayerPosition);

    public bool GameOver => PlayerHealth <= 0;

    public Game(int rows, int columns, Position playerPosition, int initialPlayerHealth, int initialEnemyHealth)
    {
        Map = new int[rows, columns];
        PlayerPosition = playerPosition;

        GenerateMap(initialPlayerHealth, initialEnemyHealth);
    }

    private void GenerateMap(int initialPlayerHealth, int initialEnemyHealth)
    {
        for (var row = 0; row < Map.GetLength(0); row++)
        {
            for (var column = 0; column < Map.GetLength(1); column++)
            {
                if (PlayerPosition.Row == row && PlayerPosition.Column == column)
                {
                    Map[row, column] = initialPlayerHealth;
                    continue;
                }

                Map[row, column] = initialEnemyHealth;
            }
        }
    }

    private int HealthAtPosition(Position position)
    {
        return Map[position.Row, position.Column];
    }

    private void SetHealthAtPosition(Position position, int health)
    {
        Map[position.Row, position.Column] = health;
    }

    /**
     * Returns false if movement failed...
     */
    public bool MovePlayer(Direction direction)
    {
        var offset = direction switch
        {
            Direction.North => new Position(row: -1, column: 0),
            Direction.East => new Position(row: 0, column: 1),
            Direction.South => new Position(row: 1, column: 0),
            Direction.West => new Position(row: 0, column: -1),
            // We've already exhausted all the possible paths, but idk why C# doesn't seem to know that,
            // so we need this regardless :/
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var newPosition = new Position(
            row: PlayerPosition.Row + offset.Row,
            column: PlayerPosition.Column + offset.Column
        );

        if (IsPositionValid(newPosition) == false)
        {
            return false;
        }

        MovePlayer(newPosition);
        return true;
    }

    private void MovePlayer(Position newPosition)
    {
        var enemyAtPositionHealth = HealthAtPosition(newPosition);
        var newHealth = PlayerHealth - enemyAtPositionHealth;

        SetHealthAtPosition(newPosition, newHealth);
        SetHealthAtPosition(PlayerPosition, health: 0);

        PlayerPosition = newPosition;
    }

    private bool IsPositionValid(Position newPosition)
    {
        var row = newPosition.Row;
        var column = newPosition.Column;

        return row >= 0 && row < Map.GetLength(0) &&
               column >= 0 && column < Map.GetLength(1);
    }
}