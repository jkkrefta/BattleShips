using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class CoordinateTests
    {
        private const string B2 = "B2";
        private const string F7 = "F7";

        [Test]
        public void Coordinate_GivenNullString_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() => new Coordinate(null));
        }
        
        [Test]
        public void Coordinate_GivenEmptyString_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() => new Coordinate(string.Empty));
        }
        
        [Test]
        public void Coordinate_GivenStringLongerThan3Chars_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() => new Coordinate("1234"));
        }
        
        [TestCase("A0")]
        [TestCase("A11")]
        [TestCase("1A")]
        [TestCase("11A")]
        [TestCase("A#1")]
        [TestCase("J 1")]
        [TestCase("K9")]
        [TestCase("P0")]
        [TestCase("Z11")]
        public void Coordinate_GivenStringNotMatchingLabelRule_Throws(string givenLabel)
        {
            Should.Throw<InvalidCoordinateException>(() => new Coordinate(givenLabel));
        }
        
        [TestCase("A1")]
        [TestCase("C8")]
        [TestCase("J10")]
        public void Coordinate_GivenStringMatchingLabelRule_CreatesInstance(string givenLabel)
        {
            new Coordinate(givenLabel).Label.ShouldBeEquivalentTo(givenLabel);
        }
        
        [Test]
        public void Coordinate_Equals_GivenOtherNonCoordinateObject_ReturnsFalse()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            new Coordinate(B2).Equals("Some string").ShouldBeFalse();
        }
        
        [Test]
        public void Coordinate_Equals_GivenOtherCoordinateWithDifferentLabel_ReturnsFalse()
        {
            var coordinate = new Coordinate(B2);
            var otherCoordinate = new Coordinate(F7);
            coordinate.Equals(otherCoordinate).ShouldBeFalse();
        }
        
        [Test]
        public void Coordinate_Equals_GivenOtherCoordinateWithEqualLabel_ReturnsTrue()
        {
            var coordinate = new Coordinate(F7);
            var otherCoordinate = new Coordinate(F7);
            coordinate.Equals(otherCoordinate).ShouldBeTrue();
        }
        
        [Test]
        public void Coordinate_ToString_ReturnsLabel()
        {
            new Coordinate(F7).ToString().ShouldBeEquivalentTo(F7);
        }
    }
}