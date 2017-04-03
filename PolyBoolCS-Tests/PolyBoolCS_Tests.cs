using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using PolyBoolCS;
using Newtonsoft.Json;

namespace PolyBoolCS_Tests
{
	[TestClass]
	public class PolyBoolCS_Tests
	{
		[TestMethod]
		public void DebugScratchpad()
		{
			var demo = DemoData.polyCases.Where( x => x.name == "Two Triangles With Common Edge" ).First();
			var buildLog = new BuildLog();
			var lib = new PolyBool() { BuildLog = buildLog };
			var result = lib.intersect( demo.poly1, demo.poly2 );

			var json = buildLog.ToJSON();
			using( var file = File.CreateText( "DebugLog.json" ) )
			{
				file.Write( json );
			}

			Assert.IsNotNull( result );

			validateResult( result, demo.validate[ 0 ], demo.name, "intersect" );
		}

		[TestMethod]
		public void TestDemoData_Intersect()
		{
			foreach( var demo in DemoData.polyCases )
			{
				var result = new PolyBool().intersect( demo.poly1, demo.poly2 );
				Assert.IsNotNull( result );

				validateResult( result, demo.validate[ 0 ], demo.name, "intersect" );
			}
		}

		[TestMethod]
		public void TestDemoData_Union()
		{
			foreach( var demo in DemoData.polyCases )
			{
				var result = new PolyBool().union( demo.poly1, demo.poly2 );
				Assert.IsNotNull( result );

				validateResult( result, demo.validate[ 1 ], demo.name, "union" );
			}
		}

		[TestMethod]
		public void TestDemoData_Difference()
		{
			foreach( var demo in DemoData.polyCases )
			{
				var result = new PolyBool().difference( demo.poly1, demo.poly2 );
				Assert.IsNotNull( result );

				validateResult( result, demo.validate[ 2 ], demo.name, "difference" );
			}
		}

		[TestMethod]
		public void TestDemoData_DifferenceRev()
		{
			foreach( var demo in DemoData.polyCases )
			{
				var result = new PolyBool().differenceRev( demo.poly1, demo.poly2 );
				Assert.IsNotNull( result );

				validateResult( result, demo.validate[ 3 ], demo.name, "differenceRev" );
			}
		}

		[TestMethod]
		public void TestDemoData_Xor()
		{
			foreach( var demo in DemoData.polyCases )
			{
				var result = new PolyBool().xor( demo.poly1, demo.poly2 );
				Assert.IsNotNull( result );

				validateResult( result, demo.validate[ 4 ], demo.name, "xor" );
			}
		}

		private void validateResult( Polygon result, JsonPoly validationData, string demoName, string operation )
		{
			var regions = validationData.regions;
			Assert.AreEqual( regions.Count, result.regions.Count, string.Format( "C# conversion did not return the same number of regions in the '{0}' demo for the '{1}' operation", demoName, operation ) );

			for( int i = 0; i < regions.Count; i++ )
			{
				validateRegion( result.regions[ i ], regions[ i ], demoName, operation );
			}
		}

		private void validateRegion( PointList result, double[][] validationData, string demoName, string operation )
		{
			Assert.AreEqual( validationData.Length, result.Count, string.Format( "C# conversion did not return the same number of points in the '{0}' demo for the '{1}' operation", demoName, operation ) );

			for( int i = 0; i < validationData.Length; i++ )
			{
				var validationPoint = validationData[ i ];
				var resultPoint = result[ i ];

				Assert.AreEqual( validationPoint[ 0 ], resultPoint[ 0 ], 1e-9f, string.Format( "Demo: '{0}', Op: '{1}', Point: {2}", demoName, operation, i ) );
				Assert.AreEqual( validationPoint[ 1 ], resultPoint[ 1 ], 1e-9f, string.Format( "Demo: '{0}', Op: '{1}', Point: {2}", demoName, operation, i ) );
			}
		}
	}
}
