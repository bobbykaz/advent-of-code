using System.Numerics;
using Grids;
using Vec;

namespace y25 {
    public class D8: AoCDay {

        private int DistancesToEval()
        {
            if (_UseSample)
                return 10;
            return 1000;
        }
        public D8(): base(25, 8) {
            _DebugPrinting = false;
            _UseSample = false;
        }

        private List<Dist> GetDistances(List<Vec3> vecs)
        {
            var distances = new List<Dist>();
            for (int i = 0; i < vecs.Count; i++)
            {
                var va = vecs[i];
                for (int j = i; j < vecs.Count; j++)
                {
                    if (i != j)
                    {
                        var vb = vecs[j];
                        var dist = va.EuclideanDistance(vb);
                        distances.Add(new Dist(dist, va, vb));
                    }
                }
            }
            distances = distances.OrderBy(d => d.value).ToList();
            return distances;
        }

        private void Connect(Dictionary<Vec3, Circuit> vecMap, HashSet<Circuit> allCircuits, Dist dist)
        {
            PrintLn($"\n Dist {dist.value}: {dist.a} and {dist.b}");
            var existingCircuitA = vecMap.ContainsKey(dist.a) ? vecMap[dist.a] : null;
            var existingCircuitB = vecMap.ContainsKey(dist.b) ? vecMap[dist.b] : null;
                
            if (existingCircuitA != null && existingCircuitB != null)
            {
                if (existingCircuitA != existingCircuitB)
                {
                    // combine
                    existingCircuitA.points.UnionWith(existingCircuitB.points);
                    foreach (var v in existingCircuitB.points)
                    {
                        vecMap[v] = existingCircuitA;
                    }

                    allCircuits.Remove(existingCircuitB);
                }
            } 
            else if (existingCircuitA != null || existingCircuitB != null)
            {
                var existingCircuit = existingCircuitA ?? existingCircuitB;
                existingCircuit.points.Add(dist.a);
                existingCircuit.points.Add(dist.b);
                vecMap[dist.a] = existingCircuit;
                vecMap[dist.b] = existingCircuit;
            }
            else
            {
                PrintLn($"..Creating new circuit - {dist.a} and {dist.b}");
                var newCircuit = new Circuit();
                allCircuits.Add(newCircuit);
                newCircuit.points.Add(dist.a);
                newCircuit.points.Add(dist.b);
                vecMap[dist.a] = newCircuit;
                vecMap[dist.b] = newCircuit;
            }

            PrintCircs(allCircuits);
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var vecs = lines.Select(l => Vec3.Parse(l)).ToList();
            var distances = GetDistances(vecs);
            PrintLn($"distances: {distances.Count}");
            var vm = new Dictionary<Vec3, Circuit>();
            var circs = new HashSet<Circuit>();
            for (int i = 0; i < DistancesToEval(); i++)
            {
                Connect(vm, circs, distances[i]);
            }
            
            var allCircuits = circs.OrderByDescending(c => c.points.Count).ToList();
            
            PrintLn($"allCircuits: {allCircuits.Count}");
            foreach (var c in allCircuits)
            {
                PrintLn($"{c.points.Count}");
                PrintLn($"   {string.Join(" | ",c.points)}");
                
            }
            
            var total = allCircuits[0].points.Count * allCircuits[1].points.Count * allCircuits[2].points.Count();
            return $"total {total}";
        }

        private void PrintCircs(IEnumerable<Circuit> circs)
        {
            var allCircuits = circs.OrderByDescending(c => c.points.Count).ToList();
            foreach (var c in allCircuits)
            {
                PrintLn($"    {c.points.Count} :   {string.Join(" | ",c.points)}");
            }
        }
        
        public record Dist(double value, Vec3 a, Vec3 b);
        
        public class Circuit
        {
            public HashSet<Vec3> points = [];
        }
        
        public override string P2()
        {
            var lines = InputAsLines();
            var vecs = lines.Select(l => Vec3.Parse(l)).ToList();
            var distances = GetDistances(vecs);
            PrintLn($"distances: {distances.Count}");
            var vm = new Dictionary<Vec3, Circuit>();
            var circs = new HashSet<Circuit>();
            for (int i = 0; i < distances.Count; i++)
            {
                Connect(vm, circs, distances[i]);
                if (circs.First().points.Count() == vecs.Count)
                {
                    _DebugPrinting = true;
                    var circ = circs.First();
                    PrintLn($"All junctions connected: {circ.points.Count}");
                    PrintLn($"Dist: {distances[i]}");
                    return $"{distances[i].a.X * distances[i].b.X}";
                }
            }

            throw new Exception("Hopefully we never get here");
        }
    }
}