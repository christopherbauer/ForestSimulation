using System;
using System.Collections.Generic;
using System.Linq;
using ForestSimulation.services;

namespace ForestSimulation.ForestObject
{
    public abstract class ForestObjectBase : IForestObject
    {
        private Point _location;
        protected INumberGeneratorService NumberGeneratorService;

        protected ForestObjectBase(int x, int y, INumberGeneratorService numberGeneratorService = null)
        {
            Location = new Point(x, y);
            NumberGeneratorService = numberGeneratorService ?? new RandomNumberGeneratorService(new Random());
        }

        public Point Location {
            get { return _location; }
            set { _location = value; }
        }

        public List<Point> GetAdjacentSpots(int bound)
        {
            var spotPossibilities = new List<Point>();
            var pointXY = new Point(Math.Max(0, _location.X - 1), Math.Max(0, _location.Y - 1));
            var pointX2Y2 = new Point(Math.Min(bound, _location.X + 1), Math.Min(bound, _location.Y + 1));
            for (var x = pointXY.X; x <= pointX2Y2.X; x++)
            {
                for (var y = pointXY.Y; y <= pointX2Y2.Y; y++)
                {
                    if (x != _location.X || y != _location.Y) //if not the spot of the current object
                        spotPossibilities.Add(new Point(x, y));
                }
            }
            return spotPossibilities;
        }

        public virtual void Tick(IForest forest)
        {
        }

        protected virtual bool CheckGridAtLocationFor<T>(IEnumerable<IForestObject> gridContents) where T : IForestObject
        {

            if (gridContents == null)
            {
                return true;
            }
            if (!gridContents.Any())
            {
                return true;
            }
            return !gridContents.Any(o => o is T);
        }
    }
}