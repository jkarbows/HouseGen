using UnityEngine;

namespace ProcGenKit.WorldBuilding
{
    public delegate GameObject GeneratorFunction(InstancePainter ip, GameObject stamp);

    public enum AlgorithmName
    {
        Test,
        GrowingTree,
        SpacePartition,
        WaveFunctionCollapse
    }

    public class GeneratorAlgorithm : ScriptableObject
    {
        public InstancePainter ip;
        public GameObject stamp;

        public virtual GameObject Generate(InstancePainter instancePainter, GameObject newStamp)
        {
            this.ip = instancePainter;
            this.stamp = newStamp;

            return this.stamp;
        }
    }
}