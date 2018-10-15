using System;
using System.Collections.Generic;
using System.Linq;
using ForestSimulation.ForestObject;
using ForestSimulation.services;

namespace ForestSimulation
{
    public class Forest : IForest
    {
        private readonly INumberGeneratorService _numberGeneratorService;
        private int _bound;
        private int _lumber;
        private int _maulings;

        public Forest(int bound, INumberGeneratorService numberGeneratorService = null)
        {
            _bound = bound;
            _history = new List<string>();
            _numberGeneratorService = numberGeneratorService ??
                                            new RandomNumberGeneratorService(new Random());
            ForestObjects = new List<IForestObject>();
        }

        public void InitializeForest()
        {
            var totalSpots = (Bound*Bound);

            var initialLumberJackCount = totalSpots*.1;
            for (var i = 0; i < initialLumberJackCount; i++)
            {
                GenerateLumberJack();
            }
            var initialTreeCount = totalSpots*.5;
            for (var i = 0; i < initialTreeCount; i++)
            {
                var assigned = false;
                while (!assigned)
                {
                    var desiredX = _numberGeneratorService.GetNextRandomOfBound(Bound);
                    var desiredY = _numberGeneratorService.GetNextRandomOfBound(Bound);
                    if (IsOpenSpot(desiredX, desiredY))
                    {
                        assigned = true;
                        ForestObjects.Add(new Tree(desiredX, desiredY, new SaplingGeneratorService(), TreeAge.Tree, _numberGeneratorService));
                    }
                }
            }
            var initialBearCount = totalSpots*.02;
            for (var i = 0; i < initialBearCount; i++)
            {
                GenerateBear();
            }
        }

        public void GenerateBear()
        {
            var assigned = false;
            while (!assigned)
            {
                var desiredX = _numberGeneratorService.GetNextRandomOfBound(Bound);
                var desiredY = _numberGeneratorService.GetNextRandomOfBound(Bound);
                if (IsOpenSpot(desiredX, desiredY))
                {
                    ForestObjects.Add(new Bear(desiredX, desiredY, _numberGeneratorService));
                    assigned = true;
                }
            }
        }

        public void GenerateLumberJack()
        {
            var assigned = false;
            while (!assigned)
            {
                var desiredX = _numberGeneratorService.GetNextRandomOfBound(Bound);
                var desiredY = _numberGeneratorService.GetNextRandomOfBound(Bound);
                if (IsOpenSpot(desiredX, desiredY))
                {
                    ForestObjects.Add(new LumberJack(desiredX, desiredY, _numberGeneratorService));
                    assigned = true;
                }
            }
        }

        private List<String> _history; 
        public void Print()
        {
            Console.Clear();
            for (var x = 0; x <= Bound; x++)
            {
                for (var y = 0; y <= Bound; y++)
                {
                    if (IsOpenSpot(x, y))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(".");
                    }
                    else if (GridContents(x, y).Any(o => o is Bear))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("B");
                    }
                    else if (GridContents(x, y).Any(o => o is Tree))
                    {
                        switch (((Tree)GridContents(x, y).First(o => o.GetType() == typeof(Tree))).Age)
                        {
                            case TreeAge.Sapling:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("t");
                                break;
                            case TreeAge.Tree:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("T");
                                break;
                            case TreeAge.ElderTree:
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("T");
                                break;
                        }
                    }
                    else if (GridContents(x, y).Any(o => o is LumberJack))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("L");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public Boolean IsOpenSpot(int desiredX, int desiredY)
        {
            return !ForestObjects.Any(o => o.Location.X == desiredX && o.Location.Y == desiredY);
        }

        public List<IForestObject> GridContents(int x, int y)
        {
            return ForestObjects.Where(o => o.Location.X == x && o.Location.Y == y).ToList();
        }

        public List<IForestObject> GridContents(Point location)
        {
            return GridContents(location.X, location.Y);
        }

        public void Remove(IForestObject forestObject)
        {
            ForestObjects.Remove(forestObject);
        }

        public List<IForestObject> ForestObjects { get; set; }

        public List<ITree> Trees
        {
            get { return GetForestObjectsOfType<ITree>(); }
        }

        public List<ILumberJack> LumberJacks {
            get { return GetForestObjectsOfType<ILumberJack>(); }
        }

        public List<IBear> Bears {
            get { return GetForestObjectsOfType<IBear>(); }
        }

        private List<T> GetForestObjectsOfType<T>() where T : IForestObject
        {
            return (from o in ForestObjects
                    where o is T
                    select o).Cast<T>().ToList();
        }

        public void AddForestObject(IForestObject forestObject)
        {
            ForestObjects.Add(forestObject);
        }

        public int Bound
        {
            get { return _bound; }
            set { _bound = value; }
        }

        public int Lumber
        {
            get { return _lumber; }
            set { _lumber = value; }
        }

        public int Maulings
        {
            get { return _maulings; }
            set { _maulings = value; }
        }
    }
}
