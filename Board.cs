using System;


public class Board
{
    public const int BOARD_SIZE = 10;
    public const int CELL_NONE  =  0;
    public const int CELL_BOMB  =  1;
    public const int CELL_PUT   =  2;


    private int[,] board;
    private Cursor cursor;
    private Random rd;
    public  bool   gameClearFlag { set; get; }
    public  bool   gameOverFlag  { set; get; }


    public Board()
    {
        gameClearFlag = false;
        gameOverFlag  = false;
        BuildInstance();
        InitBoard();
    }


    private void BuildInstance()
    {
        board  = new int[BOARD_SIZE, BOARD_SIZE];
        cursor = new Cursor();
        rd     = new Random();
    }

 
    private void InitBoard()
    {
        for (var y=0; y<BOARD_SIZE; ++y)
            for (var x=0; x<BOARD_SIZE; ++x)
                board[y, x] = CELL_NONE;

        randomPutBomb();
    }


    public void ShowBoard()
    {
        for (var y=0; y<BOARD_SIZE; ++y)
        {
            for (var x=0; x<BOARD_SIZE; ++x)
            {
                if (x == cursor.X && y == cursor.Y)
                    Write(" * ");
                else
                {
                    switch (board[y, x])
                    {
                    case CELL_NONE: Write(" + "); break;
                    case CELL_BOMB: 
                        Write((gameOverFlag || gameClearFlag) ? " B " : " + ");
                        break;
                    case CELL_PUT:  Write(" " + CountBomb(x, y) + " "); break;
                    default:        Write(" E "); break;
                    }
                }
            }
            WriteLine();
        }
    }


    public void Input()
    {
        char c = Console.ReadKey().KeyChar;
        switch (c)
        {
            case 'w': --cursor.Y; break;
            case 'a': --cursor.X; break;
            case 's': ++cursor.Y; break;
            case 'd': ++cursor.X; break;
            case 'o': 
                gameOverFlag  = IsGameOver(cursor.X, cursor.Y);
                PutCell(cursor.X, cursor.Y);
                gameClearFlag = IsGameClear();
                break;
        }
    }


    public int CountBomb(int x, int y)
    {
        var cnt = 0;

        for (var i=-1; i<=1; ++i)
            for (var j=-1; j<=1; ++j)
            {
                if (!IsStageWithIn(x+j, y+i)) continue;
                if (IsBomb(x+j, y+i)) ++cnt;
            }

        return cnt;
    }


    public void PutCell(int x, int y)
    {
        board[y, x] = CELL_PUT;

        if (CountBomb(x, y) == 0)
        {
            for (var i=-1; i<=1; ++i)
            {
                for (var j=-1; j<=1; ++j)
                {
                    if (IsStageWithIn(x+j, y+i) &&
                        !IsBomb(x+j, y+i) &&
                        !(board[y+i, x+j] == CELL_PUT))
                        PutCell(x+j, y+i);
                }
            }
        }
    }


    public void randomPutBomb()
    {
        for (var y=0; y<BOARD_SIZE; ++y)
        {
            for (var x=0; x<BOARD_SIZE; ++x)
            {
                int result = rd.Next(100);
                if (result < 20) board[y, x] = CELL_BOMB;
            }
        }
    }


    public bool IsStageWithIn(int x, int y)
    {
        return (x >= 0 && x < BOARD_SIZE) &&
               (y >= 0 && y < BOARD_SIZE);
    }


    public bool IsGameClear()
    {
        for (var y=0; y<BOARD_SIZE; ++y)
            for (var x=0; x<BOARD_SIZE; ++x)
                if (board[y, x] == CELL_NONE) return false;

        return true;
    }

    public bool IsGameOver(int x, int y)
    {
        return IsBomb(x, y);
    }


    public bool IsGameEnd()
    {
        return gameOverFlag || gameClearFlag;
    }


    private bool IsBomb(int x, int y)
    {
        return board[y, x] == CELL_BOMB;
    }


    private void Write(Object o)
    {
        Console.Write(o);
    }


    private void WriteLine()
    {
        Console.WriteLine();
    }


    private void WriteLine(Object o)
    {
        Console.WriteLine(o);
    }
}
