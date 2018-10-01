using System;

public class MineSweeperMain
{
    static void Main(string[] args)
    {
        var stage = new Board();

        while (true)
        {
            Console.Clear();
            stage.ShowBoard();
            stage.Input();
            if (stage.IsGameEnd()) break;
        }

        Console.Clear();
        stage.ShowBoard();
        if (stage.gameClearFlag)
            Console.WriteLine("Game Clear");
        else if (stage.gameOverFlag)
            Console.WriteLine("Game Over");
        else
            Console.WriteLine("What!?");
    }
}
