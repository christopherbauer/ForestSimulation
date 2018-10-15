using System;
using System.Collections;
using System.Collections.Generic;
using ForestSimulation.ForestObject;

namespace ForestSimulation
{
    public interface IForest
    {
        List<ITree> Trees { get; }
        List<ILumberJack> LumberJacks { get; }
        List<IBear> Bears { get; }
        int Bound { get; set; }
        int Lumber { get; set; }
        int Maulings { get; set; }
        List<IForestObject> ForestObjects { get; set; }
        Boolean IsOpenSpot(int desiredX, int desiredY);
        void AddForestObject(IForestObject forestObject);
        List<IForestObject> GridContents(int x, int y);
        List<IForestObject> GridContents(Point location);
        void Remove(IForestObject forestObject);
        void GenerateLumberJack();
        void GenerateBear();
        void Print();
        void InitializeForest();
    }
}