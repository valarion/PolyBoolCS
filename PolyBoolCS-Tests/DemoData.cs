using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PolyBoolCS;
using Newtonsoft.Json;

namespace PolyBoolCS_Tests
{
	public class JsonDemoData
	{
		public string name;
		public JsonPoly poly1;
		public JsonPoly poly2;
		public JsonPoly[] validate;
	}

	public class JsonPoly
	{
		public bool inverted;
		public List<double[][]> regions;
	}

	public class DemoCase
	{
		public string name;
		public Polygon poly1;
		public Polygon poly2;
		public JsonPoly[] validate;
	}

	/// <summary>
	/// Converts JSON/Javascript test cases from the original polybooljs demo into C# DemoCase instances.
	/// What can I say, I was too lazy to convert all of that data to C# by hand <grin/>
	/// </summary>
	public class DemoData
	{
		public static List<DemoCase> polyCases = new List<DemoCase>();

		static DemoData()
		{
			using( var file = File.OpenText( "DemoData.js" ) )
			{
				var testData = JsonConvert.DeserializeObject<JsonDemoData[]>( file.ReadToEnd() );

				foreach( var demo in testData )
				{
					polyCases.Add( convertToDemoCase( demo ) );
				}
			}
		}

		private static DemoCase convertToDemoCase( JsonDemoData data )
		{
			var demoCase = new DemoCase()
			{
				name = data.name,
				poly1 = convertToPolygon( data.poly1 ),
				poly2 = convertToPolygon( data.poly2 ),
				validate = data.validate
			};

			return demoCase;
		}

		private static Polygon convertToPolygon( JsonPoly jsonPoly )
		{
			var poly = new Polygon()
			{
				inverted = jsonPoly.inverted,
				regions = convertRegions( jsonPoly.regions )
			};

			return poly;
		}

		private static List<PointList> convertRegions( List<double[][]> regionData )
		{
			var regions = new List<PointList>();

			foreach( var region in regionData )
			{
				var list = new PointList( region.Length );

				foreach( var pointData in region )
				{
					var point = new Point( pointData[ 0 ], pointData[ 1 ] );
					list.Add( point );
				}

				regions.Add( list );
			}

			return regions;
		}
	}
}
