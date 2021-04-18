using System.Text.RegularExpressions;

namespace CCode.BattleShips.Core
{
    public record Coordinate
    {
        public readonly string Label;
        
        public Coordinate(string label)
        {
            IsNotNullOrEmpty(label);
            IsNoLongerThan3Chars(label);
            MatchesLabelRules(label);
            Label = label;
        }

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
                case 2 when Matches2CharLabelRule(label):
                case 3 when Matches3CharLabelRule(label):
                    throw new InvalidCoordinateException(
                        $"{label} does not match battleships rule. Expected example: A1, J10, C6, F7");
            }
        }

        private static bool Matches3CharLabelRule(string label) => !new Regex(@"[A-J]10").IsMatch(label);

        private static bool Matches2CharLabelRule(string label) => !new Regex(@"[A-J][1-9]").IsMatch(label);
    }
}