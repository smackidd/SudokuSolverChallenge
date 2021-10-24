using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolverChallenge
{
    class Sudoku : ISudokuChallenge
    {
        Random rnd = new Random();
        private int[,] _board = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
        private int[,] _solvedBoard = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

        public int[,] Board { get { return _board; } set { _board = value; } }
        public int[,] SolvedBoard { get { return _solvedBoard; } set { _solvedBoard = value; } }

        public Sudoku()
        {
            
        }
        // content - The body of text to parse into a Sudoku board.
        // board - Where to place the board parsed from content.
        // Returns - TRUE if parse was successful, FALSE if content is not valid input.
        public bool Parse(string content, int[,] board)
        {
            //Parse the data
            List<int> digits = new List<int>();
            foreach (char number in content)
            {
                int digit = 0;
                bool isNumber = Int32.TryParse(number.ToString(), out digit);
                if(isNumber)
                {
                    digits.Add(digit);    
                } 
                else
                {
                    if (number.ToString() == ".")
                    {
                        digit = 0;
                        digits.Add(digit);
                    }
                }
            }

            // Update the board
            if (digits.Count == 81)
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    _board[i, j] = digits.ElementAt<int>((i * 9) + j);
                }
            } else return false;
            return true;
        }

        // inputBoard - An incomplete/unsolved Sudoku board.
        // solvedBoard - Where to place the complete/solved board.
        // Returns - TRUE if a solution was found, FALSE if the puzzle could not be solved.
        public bool Solve(int[,] inputBoard, int[,] solvedBoard)
        {
            int[] emptySquare = nextEmptySquare(inputBoard);
            // if there is no empty square left, the puzzle is solved
            if (emptySquare[0] == 99 && emptySquare[1] == 99) 
            {
                Array.Copy(inputBoard, _solvedBoard, 81);
                return true;
            }

            //Check each value to see if it breaks the board for this particular row and col
            //if not, input value into row and col and recursively Solve the next empty square
            //or backtrack to replace previous numbers if recursion runs into a brick wall
            int row = emptySquare[0];
            int col = emptySquare[1];
            for (int i = 1; i <= 9; i++)
            {
                if (CheckValid(inputBoard, row, col, i)) 
                { 
                    inputBoard[row, col] = i;

                    if (Solve(inputBoard, solvedBoard)) 
                    {
                        Array.Copy(inputBoard, _solvedBoard, 81);
                        return true;
                    };

                    inputBoard[row, col] = 0;
                }
            }

            return false;
        }

        

        private int[] nextEmptySquare(int[,] grid)
        {
            int[] rowCol = new int[2] { 99, 99 };
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i,j] == 0)
                    {
                        rowCol[0] = i;
                        rowCol[1] = j;
                        return rowCol;
                    }
                }
            }
            return rowCol;
        }

        private bool CheckValid(int[,] grid, int row, int col, int val)
        {
            //Check row
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == val && i != col) return false;
            }

            //Check col
            for (int i = 0; i < 9; i++)
            {
                if (grid[i, col] == val && i != row) return false;
            }

            //Check section
            int rowSection = row / 3;
            int colSection = col / 3;
            for (int i = rowSection * 3; i < (rowSection * 3) + 3; i++)
            {
                for (int j = colSection * 3; j < (colSection * 3) + 3; j++)
                {
                    if (i == row && j == col) continue;
                    else if (grid[i, j] == val) return false;
                }
            }

            return true;
        }
        
    }
}
