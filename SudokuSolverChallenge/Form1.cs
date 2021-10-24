using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SudokuSolverChallenge
{
    public partial class SudokuChallengeMain : Form
    {
        Sudoku sudoku = new Sudoku();
        String textFile = "";
        public SudokuChallengeMain()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SudokuChallengeMain_Load(object sender, EventArgs e)
        {
            updateBoard();       
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Text | *.txt",
                ValidateNames = true,
                Multiselect = false
            })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    tbFilePath.Text = ofd.FileName;
                    if (File.Exists(ofd.FileName))
                    {
                        textFile = File.ReadAllText(ofd.FileName);
                        
                        sudoku.Parse(textFile, sudoku.Board);
                        updateBoard();
                    }
                }
            }
        }

        private void updateBoard()
        {
            Label cell;
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //casting Label.
                    int labelIndex = (i * 9) + j;
                    int labelIndex2 = (labelIndex - 80) * -1; //reverse the index order to fix backwards bug
                    //labelIndex = (labelIndex - labelIndex - 1) * -1;
                    if (tbSudokuBoard.Controls[labelIndex2] is Label)
                        cell = (Label)tbSudokuBoard.Controls[labelIndex2];
                    else
                        continue;

                    //cell.Text = board[i, j].ToString();
                    String cellValue = sudoku.Board[i, j].ToString();
                    if (cellValue =="0") 
                        cell.Text = "";
                    else 
                        cell.Text = cellValue;
                }
            }

            
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            bool solved = sudoku.Solve(sudoku.Board, sudoku.SolvedBoard);
            Console.WriteLine(solved);
            if (solved)
            {
                Array.Copy(sudoku.SolvedBoard, sudoku.Board, 81);
                updateBoard();
            }
        }
    }
}
