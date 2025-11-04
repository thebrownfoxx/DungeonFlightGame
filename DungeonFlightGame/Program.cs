namespace DungeonFlightGame;

internal static class Program
{
    private static void Main()
    {
        var game = new Game(
            rows: 7,
            columns: 4,
            playerPosition: new Position(0, 0),
            initialPlayerHealth: 10,
            initialEnemyHealth: 2);

        while (true)
        {
            PrintMap(game.Map);
            Console.WriteLine($"Health: {game.PlayerHealth}");
            Console.WriteLine($"Position: {game.PlayerPosition}");
            Console.WriteLine();

            if (game.GameOver)
            {
                Console.WriteLine("You died!");
                return;
            }

            var direction = PlayerInput();

            if (game.MovePlayer(direction))
                continue;

            Console.WriteLine("Can't move in that direction!");
            Console.WriteLine();
        }
    }

    private static void PrintMap(int[,] map)
    {
        for (var row = 0; row < map.GetLength(0); row++)
        {
            for (var column = 0; column < map.GetLength(1); column++)
            {
                Console.Write($"{map[row, column]} ");
            }

            Console.WriteLine();
        }
    }

    private static Direction PlayerInput()
    {
        while (true)
        {
            var direction = ParseInput(Console.ReadLine());

            if (direction is not null)
                return direction.Value;

            Console.WriteLine("Invalid input!");
            Console.WriteLine();
        }
    }

    private static Direction? ParseInput(string? input)
    {
        return input?.ToLower() switch
        {
            "w" => Direction.North,
            "d" => Direction.East,
            "s" => Direction.South,
            "a" => Direction.West,
            _ => null,
        };
    }
}