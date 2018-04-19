using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public static class BackgroundTheme {
        public static Color Dark = Color.FromArgb(32, 32, 32);
        public static Color Light = Color.FromArgb(224, 224, 224);
    }

    public static class TextTheme
    {
        public static Color Dark = Color.FromArgb(224, 224, 224);
        public static Color Light = Color.FromArgb(32, 32, 32);
    }

    public class WindowTheme
    {
        public Color TextColor { get; }
        public Color GrayColor { get; }
        public Color BackColor { get; }

        public WindowTheme(Color textColor, Color grayColor, Color backColor)
        {
            TextColor = textColor; GrayColor = grayColor; BackColor = backColor;
        }
    }

    public static class Theme
    {
        public static readonly WindowTheme DarkTheme = new WindowTheme(TextTheme.Dark, Color.Black, BackgroundTheme.Dark);
        public static readonly WindowTheme LightTheme = new WindowTheme(TextTheme.Light, Color.White, BackgroundTheme.Light);
    }

    public static class ColorTheme
    {
        public static Color Red = Color.FromArgb(128, 32, 32);
        public static Color Blue = Color.FromArgb(32, 32, 128);
        public static Color Green = Color.FromArgb(32, 96, 32);
    }

    // Use a struct and a constructor.
    public static class Settings
    {
        // Theme Settings.
        public static WindowTheme WindowColors = Theme.DarkTheme;
        public static Color ThemeColor = ColorTheme.Red;
    }
}
