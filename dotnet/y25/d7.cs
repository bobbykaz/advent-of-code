using Grids;
using Vec;

namespace y25 {
    public class D7: AoCDay {

        public D7(): base(25, 7) {
            _DebugPrinting = true;
            _UseSample = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var g = Utilties.RectangularCharGridFromLines(lines);
            var start = g.AllCells().First(c => c.V == 'S');
            PrintLn($"Start: {start}");
            var beamQueue = new Queue<Cell<Char>>();
            beamQueue.Enqueue(start);
            var exits = new List<Cell<Char>>();
            var splits = 0;
            var travelled = 0;
            var seen = new HashSet<string>();
            while (beamQueue.Count > 0)
            {
                var beam = beamQueue.Dequeue();
                var nextCell = g.GetNeighbor(beam.R, beam.C, Dir.S);
                while (nextCell != null && nextCell.Value.V != '^')
                {
                    travelled += 1;
                    nextCell = g.GetNeighbor(nextCell.Value.R, nextCell.Value.C, Dir.S);
                }

                if (nextCell == null)
                {
                    PrintLn($"Beam {beam} exited");
                    exits.Add(beam);
                }
                else if (nextCell.Value.V == '^')
                {
                    var cell = nextCell.Value;
                    if (!seen.Contains(cell.Key))
                    {
                        splits++;
                        PrintLn($"beam {beam} split at {nextCell.Value}; total splits {splits}");
                        beamQueue.Enqueue(new Cell<char>(cell.R+1, cell.C-1, '|'));
                        beamQueue.Enqueue(new Cell<char>(cell.R+1, cell.C+1, '|'));
                        seen.Add(cell.Key);
                    }
                    else
                    {
                        PrintLn($"Skipping cell {cell} - already seen");
                    }
                }
                else
                {
                    throw new Exception($"Unknown beam {beam} ended up at {nextCell}");
                }
            }

            return $"total {splits}";
        }

        private class BeamNode(int row, int col)
        {
            public int R => row;
            public int C => col;
            public long Value = 0;
            public bool Calculated = false;
            public List<BeamNode> Children = [];
            public string key = $"{row}-{col}";
            public override string ToString()
            {
                return $"{row}-{col} ({Value})";
            }
        }
        
        public override string P2()
        {
            var lines = InputAsLines();
            var g = Utilties.RectangularCharGridFromLines(lines);
            var start = g.AllCells().First(c => c.V == 'S');
            PrintLn($"Start: {start}");
            var root = new BeamNode(start.R, start.C);
            var beamMap = new Dictionary<string, BeamNode>();
            beamMap.Add(root.key, root);
            
            var beamQueue = new Queue<BeamNode>();
            beamQueue.Enqueue(root);
            
            while (beamQueue.Count > 0)
            {
                var beam = beamQueue.Dequeue();
                var nextCell = g.GetNeighbor(beam.R, beam.C, Dir.S);
                while (nextCell != null && nextCell.Value.V != '^')
                {
                    nextCell = g.GetNeighbor(nextCell.Value.R, nextCell.Value.C, Dir.S);
                }

                if (nextCell == null)
                {
                    PrintLn($"Beam {beam} exited");
                    beam.Value = 1;
                    beam.Calculated = true;
                }
                else if (nextCell.Value.V == '^')
                {
                    // when hitting a splitter, create a new split at that spot if one doesnt exist, and immediately
                    // link it to the two child nodes of the continuing beams and enqueue them.
                    // if it already exists, just link them up.
                    var cell = nextCell.Value;
                    if (!beamMap.ContainsKey(cell.Key))
                    {
                        PrintLn($"beam {beam} split at {nextCell.Value};");
                        var splitter = new BeamNode(cell.R, cell.C);
                        beam.Children.Add(splitter);
                        beamMap.Add(splitter.key, splitter);
                        var child1 = new BeamNode(cell.R+1, cell.C-1);
                        if (!beamMap.ContainsKey(child1.key))
                        {
                            beamMap.Add(child1.key, child1);
                            splitter.Children.Add(child1);
                            beamQueue.Enqueue(child1);
                        }
                        else
                        {
                            splitter.Children.Add(beamMap[child1.key]);
                        }

                        var child2 = new BeamNode(cell.R+1, cell.C+1);
                        if (!beamMap.ContainsKey(child2.key))
                        {
                            beamMap.Add(child2.key, child2);
                            splitter.Children.Add(child2);
                            beamQueue.Enqueue(child2);
                        }
                        else
                        {
                            splitter.Children.Add(beamMap[child2.key]);
                        }
                    }
                    else
                    {
                        PrintLn($"beam {beam} hit existing splitter at {nextCell.Value};");
                        beam.Children.Add(beamMap[cell.Key]);
                    }
                }
                else
                {
                    throw new Exception($"Unknown beam {beam} ended up at {nextCell}");
                }
            }

            PrintLn($"===========");
            
            foreach (var node in beamMap.Values)
            {
                var childStr = string.Join(",", node.Children.Select(n => n.key));
                PrintLn($"{node} : ( {childStr} )");
            }
            
            //root has a set up tree
            // calculate from the leaves
            while (calculate(beamMap))
            {
            }

            
            
            return $"total {root.Value}";
        }

        private bool calculate(Dictionary<string, BeamNode> beamMap)
        {
            bool updated = false;
            foreach (var node in beamMap.Values.Where(n => !n.Calculated))
            {
                if (node.Children.All(child => child.Calculated))
                {
                    node.Value = node.Children.Sum(child => child.Value);
                    node.Calculated = true;
                    updated = true;
                    PrintLn($"Node {node} set to value {node.Value}");
                }   
            }

            return updated;
        }
    }
}