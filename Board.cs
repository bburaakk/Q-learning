internal class Board
{
    public char[,] board = new char[5, 5];
    public Board()
    {
        BoardCreate();
        AddStartAndFinishLocation();
        AddDangerLocation();
        PrintBoard();
        Console.WriteLine("\n->Board Created!!!!!\n");
    }
    public void BoardCreate(char c = '0')
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = c;
            }
        }
    }
    public void PrintBoard()
    {
        Console.WriteLine();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write("  " + board[i, j] + "  ");
            }
            Console.WriteLine();
        }
    }
    public void AddStartAndFinishLocation(int StartX = 0, int StartY = 0, int FinishX = 4, int FinishY = 4)
    {
        board[StartX, StartY] = 'S';
        board[FinishX, FinishY] = 'F';
    }
    public void AddDangerLocation()
    {
        board[0, 4] = 'D';
        board[2, 1] = 'D';
        board[2, 2] = 'D';
        board[2, 3] = 'D';
        board[4, 0] = 'D';
    }
}
