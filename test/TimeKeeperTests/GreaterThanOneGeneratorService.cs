using ForestSimulation.services;

namespace ForestSimulationTests
{
    public class GreaterThanOneGeneratorService : INumberGeneratorService
    {
        private static bool _lastReturnX;
        private static int x = 0;
        private static int y = 0;
        public int GetNextRandomOfBound(int bound)
        {
            var value = _lastReturnX ? y++ : x++;
            _lastReturnX = !_lastReturnX;
            return value;
        }

        public int GetNextRandomInRange(int lowBound, int highBound)
        {
            return GetNextRandomOfBound(0);
        }
    }
}