using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO: Need to properly think how to implement end of game.

namespace TicTacToe
{
    public enum OX { N, O, X }
    public enum Result { Null=-2, Loss, Tie, Win }
    public enum Player { Main, CPU }

    public partial class MainForm : Form
    {
        // Player Settings.
        public static Player firstPlayer = Player.Main;
        public static OX choice = OX.O;

        // Scoreboard values.
        private Dictionary<Result, int> results;
        // Table of OX choices, for efficient checking.
        private OX[,] gameTable;
        // The coordinates of the remaining cells.
        private HashSet<int[]> remainingCells;

        // The labels used at each cell, to represent the game board.
        public readonly Button[,] CellButtons;

        public MainForm()
        {
            InitializeComponent();

            // Initialize scoreboard.
            results = new Dictionary<Result, int>
            {
                { Result.Loss, 0 },
                { Result.Tie, 0 },
                { Result.Win, 0 }
            };

            CellButtons = new Button[3, 3]
            {
                { Button00, Button10, Button20 },
                { Button01, Button11, Button21 },
                { Button02, Button12, Button22 }
            };

            ResetGame();
            // Also want to reset appearances.
            //ResetAppearance();
        }

        private void ResetAppearance()
        {
            throw new NotImplementedException();
        }

        private void GameTable_MouseClick(object sender, MouseEventArgs e)
        {
            
        }



        private TableLayoutPanelCellPosition CPUCell()
        {
            // Get a randomized set of coordinates for a remaining cell and remove it
            // from the list of remaining cells.
            Random random = new Random();
            int index = random.Next() % remainingCells.Count;
            int[] cellCoords = remainingCells.ToArray()[index];

            // Update the computer's cell.
            Button button = CellButtons[cellCoords[0], cellCoords[1]];

            return  GameTable.GetCellPosition(button);
        }

        private void MovePlayer(Player player, TableLayoutPanelCellPosition cell)
        {
            OX ox = Marker(player);

            // Fill the cell of the player/computer.
            if (player == Player.Main) { FillCell(cell, choice); }
            else { FillCell(cell, OtherChoice(choice)); }

            // Check if the game ends and update appropriately.
            var result = GetResult(cell, player);

            if (result != Result.Null)
            {
                // If player made last move, it's either a win or a tie.
                ResetGame(GetResult(cell, player));
            }
        }

        private OX Marker(Player player)
        {
            if (player == Player.Main) { return choice; }
            return OtherChoice(choice);
        }

        private OX OtherChoice(OX choice)
        {
            if (choice == OX.O) { return OX.X; }
            return OX.X;
        }

        private void ResetGame(Result result=Result.Null)
        {
            // Update scoreboard if necessary.
            if (result != Result.Null) { UpdateScoreboard(result); }

            // Reset the game table.
            gameTable = new OX[3, 3]
            {
                { OX.N, OX.N, OX.N },
                { OX.N, OX.N, OX.N },
                { OX.N, OX.N, OX.N }
            };
            
            // Update remaining variables.
            remainingCells = new HashSet<int[]>
            {
                Coords(0,0), Coords(1,0), Coords(2,0),
                Coords(0,1), Coords(1,1), Coords(2,1),
                Coords(0,2), Coords(1,2), Coords(2,2)
            };
        }

        private int[] Coords(int v1, int v2)
        {
            return new int[2] { v1, v2 };
        }

        private void UpdateScoreboard(Result result)
        {
            results[result]++;
            Scoreboard.Text = "Wins\n{}\n\nTies\n{}\n\nLosses\n{}";
            String.Format(Scoreboard.Text,
                            StringValue(Result.Win),
                            StringValue(Result.Tie),
                            StringValue(Result.Loss)
                         );
        }

        private object StringValue(Result r) => results[r].ToString();
        
        /// <summary>
        /// Returns the result of the last move made, provided the cell of the last move.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private Result GetResult(TableLayoutPanelCellPosition cell, Player player)
        {
            int row = cell.Row;
            int col = cell.Column;

            // Check horizontal and vertical containing the cell.
            OX[] horizontal = new OX[3];
            OX[] vertical = new OX[3];
            for (int i = 0; i < 3; i++)
            {
                horizontal[i] = gameTable[row, i];
                vertical[i] = gameTable[i, col];
            }
            if (ThreeConsecutive(horizontal).Length != 0)
            {
                return FinalResult(player);
            }

            // Check if a diagonal is filled.
            if (DiagonalOver(row, col)) { return FinalResult(player); };

            // If not, check if board is full and return a tie game if it is.
            if (remainingCells.Count == 0) { return FinalResult(); }

            // If game is not over, return Null Result.
            return Result.Null;
        }

        private Result FinalResult() => Result.Tie;

        private Result FinalResult(Player player)
        {
            if (player == Player.Main) { return Result.Win; }
            return Result.Loss;
        }

        private bool DiagonalOver(int row, int col)
        {
            if ((row + col % 2) == 0)
            {
                int diag1 = ((row + col) / 2);
                int diagTest = diag1 % 2; // 0: forward diagonal, 1: reverse diagonal.

                if (ThreeConsecutive(Diagonal(diagTest==0)).Length != 0) { return true; }

                if ((row * col % 2) != 0)
                {
                    // Do another forward diagonal, since previous case only covered reverse diagonal.
                    if (ThreeConsecutive(Diagonal(true)).Length != 0) { return true; }
                }
            }
            return false;
        }

        // Returns elements of forward diagonal if diagTest = true, 
        // and reverse diagonal if diagTest = false.
        private OX[] Diagonal(bool diagTest)
        {
            OX[] diag = new OX[3];
            if (diagTest)
            {
                for (int i = 0; i < 3; i++)
                {
                    diag[i] = gameTable[i, i];
                }
                return diag;
            }
            for (int i = 0; i < 3; i++)
            {
                diag[i] = gameTable[i, 3-i];
            }
            return diag;
        }

        private OX[] ThreeConsecutive(OX[] oX)
        {
            OX first = oX[0];
            for (int i = 0; i < oX.Length; i++)
            {
                if(oX[i] != first) { return new OX[] { }; }
            }
            return oX;
        }

        private void FillCell(TableLayoutPanelCellPosition cell, OX choice)
        {
            //// Store a button inside the cell.
            //cellTable[cell.Row, cell.Column].Gr
            Button button = (Button) GameTable.GetControlFromPosition(cell.Column, cell.Row);
            button.Text = ToString(choice);

            // Update the gameTable.
            gameTable[cell.Row, cell.Column] = choice;
            // Remove from remaining cells.
            remainingCells.Remove(new int[2] { cell.Row, cell.Column });
        }

        private string ToString(OX choice)
        {
            if (choice == OX.N)
            {
                return " ";
            }
            if (choice == OX.O)
            {
                return "O";
            } 
            return "X";
        }

        // Gets the cell that the mouse clicked.
        private TableLayoutPanelCellPosition GetCell(Point point)
        {
            int col = point.X / (GameTable.Width / GameTable.ColumnCount);
            int row = point.Y / (GameTable.Height / GameTable.RowCount);

            return new TableLayoutPanelCellPosition(col, row);
        }

        private void PlayerFirstButton_Click(object sender, EventArgs e)
        {
            if (firstPlayer == Player.CPU)
            {
                // Update colors.
                PlayerFirstButton.ForeColor = Color.White;
                CPUFirstButton.ForeColor = Color.Black;
                PlayerFirstButton.BackColor = Settings.WindowColors.BackColor;
                CPUFirstButton.BackColor = Settings.ThemeColor;
            }
        }

        private void PlayerClick(Button button)
        {
            TableLayoutPanelCellPosition cell = GameTable.GetCellPosition(Button00);

            // If the left mouse button was clicked,
            if (MainForm.MouseButtons == MouseButtons.Left)
            {
                // Get the cell position that was clicked.
                MovePlayer(Player.Main, cell);
                MovePlayer(Player.CPU, CPUCell());
            }
        }

        private void Button00_Click(object sender, EventArgs e) => PlayerClick(Button00);

        private void Button10_Click(object sender, EventArgs e) => PlayerClick(Button10);

        private void Button20_Click(object sender, EventArgs e) => PlayerClick(Button20);

        private void Button01_Click(object sender, EventArgs e) => PlayerClick(Button01);

        private void Button11_Click(object sender, EventArgs e) => PlayerClick(Button11);

        private void Button21_Click(object sender, EventArgs e) => PlayerClick(Button21);

        private void Button02_Click(object sender, EventArgs e) => PlayerClick(Button02);

        private void Button12_Click(object sender, EventArgs e) => PlayerClick(Button12);

        private void Button22_Click(object sender, EventArgs e) => PlayerClick(Button22);
    }
}
