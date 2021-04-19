using System.Text.RegularExpressions;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public record Coordinate
    {
        public string Label { get; }

        public Coordinate(string label)
        {
            IsNotNullOrEmpty(label);
            IsNoLongerThan3Chars(label);
            MatchesLabelRules(label);
            Label = label;
        }

        public int Y => Label[0];
        public int X => int.Parse(Label[1..]);

        public override string ToString() => Label;

        private static void IsNotNullOrEmpty(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new InvalidCoordinateException($"label was {label}");
        }

        private static void IsNoLongerThan3Chars(string label)
        {
            if (label.Length > 3)
                throw new InvalidCoordinateException($"{label} is too long. Expected example: B10, C6");
        }

        private static void MatchesLabelRules(string label)
        {
            switch (label.Length)
            {
                case 2 when DoNotMatch2CharLabelRule(label):
                case 3 when DoNotMatch3CharLabelRule(label):
                    throw new InvalidCoordinateException(
                        $"{label} does not match battleships rule. Expected example: A1, J10, C6, F7");
            }
        }

        private static bool DoNotMatch3CharLabelRule(string label) => !new Regex(@"[A-J]10").IsMatch(label);

        private static bool DoNotMatch2CharLabelRule(string label) => !new Regex(@"[A-J][1-9]").IsMatch(label);
    }
}