using System.Collections.Generic;

namespace PBManager.Core.Consts
{
    public static class PieChartColor
    {
        public const string red = "#f56954";
        public const string green = "#00a65a";
        public const string orange = "#f39c12";
        public const string lightblue = "#00c0ef";
        public const string blue = "#3c8dbc";
        public const string grey = "#d2d6de";

        public static Dictionary<string, string> PieColors = new Dictionary<string, string>
        {
            {"red", "#f56954"},
            {"green", "#00a65a"},
            {"orange", "#f39c12"},
            {"lightblue", "#00c0ef"},
            {"blue", "#3c8dbc"},
            {"grey", "#d2d6de"}
        };
    }
}