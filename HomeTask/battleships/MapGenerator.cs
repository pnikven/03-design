using System;
using System.Linq;

namespace battleships
{
	public interface IMapGenerator
	{
		Map GenerateMap();
	}

	public class MapGenerator : IMapGenerator
	{
		private readonly Settings settings;
		private readonly Random random;

		public MapGenerator(Settings settings)
		{
			this.settings = settings;
			random = new Random(settings.RandomSeed);
		}

		public Map GenerateMap()
		{
			var map = new Map(settings.Width, settings.Height);
			foreach (var size in settings.Ships.OrderByDescending(s => s))
				PlaceShip(map, size);
			return map;
		}

		private void PlaceShip(Map map, int size)
		{
			var cells = Vector.Rect(0, 0, settings.Width, settings.Height).OrderBy(v => random.Next());
			foreach (var loc in cells)
			{
				var horizontal = random.Next(2) == 0;
				if (map.Set(loc, size, horizontal) || map.Set(loc, size, !horizontal)) return;
			}
			throw new Exception("Can't put next ship on map. No free space");
		}
	}
}