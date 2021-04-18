using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class CoordinateTests
    {
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
            var coordinate = new Coordinate(givenLabel);
            coordinate.Label.ShouldBeEquivalentTo(givenLabel);
        }
        
        [Test]
        public void Coordinate_Equals_GivenOtherCoordinateWithDifferentLabel_ReturnsFalse()
        {
            var coordinate = new Coordinate("B2");
            var otherCoordinate = new Coordinate("F7");
            coordinate.Equals(otherCoordinate).ShouldBeFalse();
        }
        
        [Test]
        public void Coordinate_Equals_GivenOtherCoordinateWithEqualLabel_ReturnsTrue()
        {
            var coordinate = new Coordinate("F7");
            var otherCoordinate = new Coordinate("F7");
            coordinate.Equals(otherCoordinate).ShouldBeTrue();
        }
    }
}