using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe_Attempt2
{
    public enum OX { N, O, X }
    public enum Result { Null = -2, Loss, Tie, Win }
    public enum Player { Human, CPU }

    public partial class GameWindow : Form
    {
        private readonly Button[,] Gameboard;
        // Stores the coordinates of the remaining cells.
        private HashSet<int[]> RemainingCoordinates;
        private OX PlayerOX; private OX CpuOX;

        public GameWindow()
        {
            InitializeComponent();

            Gameboard = new Button[3, 3]
            {
                { Button00, Button10, Button20 },
                { Button01, Button11, Button12 },
                { Button02, Button12, Button22 }
            };

            RemainingCoordinates = new HashSet<int[]>
            {
                Coords(0,0),
                Coords(1,0),
                Coords(2,0),
                Coords(0,1),
                Coords(1,1),
                Coords(1,2),
                Coords(0,2),
                Coords(1,2),
                Coords(2,2)
            };

            PlayerOX = OX.O;
            CpuOX = OX.X;
        }

        private int[] Coords(int v1, int v2)
        {
            return new int[2] { v2, v2 };
        }

        private void PlayerClick(Button button)
        {
            // Move the different game players.
            MovePlayer(Player.Human, button);
            MovePlayer(Player.CPU, CPUButton());
        }

        // Select a random remaining button for CPU to select.
        private Button CPUButton()
        {
            int[][] arr = RemainingCoordinates.ToArray();
            Random random = new Random();
            int[] coords = arr[random.Next() % arr.Length];

            return Gameboard[coords[0], coords[1]];
        }

        private void MovePlayer(Player player, Button button)
        {
            button.Text = "H"; // For testing purposes.
            //button.Text = PlayerText(player);
        }

        private string PlayerText(Player player)
        {
            if (player == Player.Human) { return ToString(PlayerOX); }
            else if (player == Player.CPU) { return ToString(CpuOX); }
            return " ";
        }

        /// <summary>
        /// Interprets an OX as a string.
        /// </summary>
        /// <param name="ox"></param>
        /// <returns></returns>
        private string ToString(OX ox)
        {
            if (ox == OX.O) { return "O"; }
            else if (ox == OX.X) { return "X"; }
            return " ";
        }

        // The button clicks.
        private void Button00_Click(object sender, EventArgs e)
        {
            Button00.Text = "H";
            //PlayerClick(Button00);
        }

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
