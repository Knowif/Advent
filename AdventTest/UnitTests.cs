using System;
using Advent;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventTest
{
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void UniqueNameTest()
		{
			foreach (Room room in GameMap.Rooms)
			{
				CollectionAssert.AllItemsAreUnique(
					room.Objects.Select(x => x.Name).ToArray());
			}

			// portable items are required to have unique names
			CollectionAssert.AllItemsAreUnique(
				GameMap.Objects.Where(x => x.IsTakable).Select(x => x.Name).ToArray());
		}

		[TestMethod]
		public void AttributeValidalityTest()
		{
			foreach (AObject obj in GameMap.Objects)
			{
				Assert.IsFalse(obj.OnTaking != AObject.DefaultHandler && !obj.IsTakable,
					$"{obj.Name} failed #1");
				Assert.IsFalse(obj.OnEntering != AObject.DefaultHandler && !obj.IsEnterable,
					$"{obj.Name} failed #2");
				Assert.IsFalse((obj.OnOpening != AObject.DefaultHandler 
					|| obj.OnClosing != AObject.DefaultHandler) && !obj.IsOpenable,
					$"{obj.Name} failed #3");
				Assert.IsFalse((obj.OnTurningOn != AObject.DefaultHandler
					|| obj.OnTurningOff != AObject.DefaultHandler) && !obj.IsSwitch,
					$"{obj.Name} failed #4");
			}
		}

		[TestMethod]
		public void LinkConsistencyTest()
		{
			var openables = GameMap.Objects.Where(x => x.IsOpenable);
			foreach (AObject obj in openables)
			{
				AObject side = null, side2 = null;
				try
				{
					side = obj.LinkedSide();
					if (side == null || obj.SkipConsistencyTest || side.SkipConsistencyTest)
						continue;
					side2 = side.LinkedSide();
					if (side2 == null) Assert.Fail($"Inconsistent door-linking: {obj.Name} => {side.Name} but {side.Name} => null");
					if (side2 != obj) Assert.Fail($"Inconsistent door-linking: {obj.Name} => {side.Name} but {side.Name} => {side2.Name}");
				} catch (AssertFailedException e) 
				{
					throw e;
				} catch (Exception e)
				{
					// use sideOk to determine whether obj.LinkedSide threw or side.LinkedSide threw.
					Assert.Fail($"{(side ?? obj).Name} threw {e.Message} when LinkedSide being called");
				}
			}
		}
	}
}
