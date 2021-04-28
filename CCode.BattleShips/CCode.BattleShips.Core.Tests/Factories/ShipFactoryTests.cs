using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.Constants;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Factories
{
    public class ShipFactoryTests
    {
        private readonly ShipFactory _shipFactory = new(new ShipLayoutValidator());
        
        private readonly Coordinate _c2 = new("C2");
        private readonly Coordinate _c3 = new("C3");
        private readonly Coordinate _c4 = new("C4");
        private readonly Coordinate _c5 = new("C5");
        private readonly Coordinate _c6 = new("C6");

        [Test]
        public void Create_GivenDestroyerAndNullCoordinates_Throws()
        {
            Should.Throw<ArgumentNullException>(() => _shipFactory.Create(ShipDefinitions.Destroyer, null));
        }

        [Test]
        public void Create_GivenNullAndCoordinates_Throws()
        {
            Should.Throw<ArgumentNullException>(() => _shipFactory.Create(null, new List<Coordinate>{_c2, _c3}));
        }

        [Test]
        public void Create_GivenDestroyerAnd2Coordinates_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() => _shipFactory.Create(ShipDefinitions.Destroyer, new List<Coordinate>{_c2, _c3}));
        }
        
        [Test]
        public void Create_GivenDestroyerAnd5Coordinates_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() => _shipFactory.Create(ShipDefinitions.Destroyer, new List<Coordinate>{_c2, _c3, _c4, _c5, _c6}));
        }
    }
}