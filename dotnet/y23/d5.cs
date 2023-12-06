using System.Data;
using System.Xml.XPath;

namespace y23 {
    public class D5 : AoCDay
    {
        public D5(): base(23, 5) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = GetInputBlocks(lines);
            var seedStr = blocks[0][0].Split(": ")[1];
            var seeds = Utilties.StringToNums<long>(seedStr, " ");
            seeds.ForEach(s => Print($"{s}, "));
            var blockMaps = new List<BlockMap>();
            for(var i = 1; i < blocks.Count; i++) {
                blockMaps.Add(new BlockMap(blocks[i]));
            }

            foreach(var map in blockMaps) {
                var newSeeds = seeds.Select(l => map.Map(l)).ToList();
                PrintLn(map.Name);
                seeds = newSeeds;
                seeds.ForEach(s => Print($"{s}, "));
                PrintLn("");
            }

            var min = seeds.Min();

            return $" {min}";
        }

        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            var blocks = GetInputBlocks(lines);
            var seedStr = blocks[0][0].Split(": ")[1];
            var seedPairs = Utilties.StringToNums<long>(seedStr, " ");
            var pairs = new List<SeedPair>();
            for(var i = 0; i < seedPairs.Count; i+=2) {
                pairs.Add(new SeedPair{ Start = seedPairs[i], Len = seedPairs[i+1]});
            }

            pairs = pairs.OrderBy(p => p.Start).ToList();
            PrintLn($"{pairs.Select(p=>p.Len).Sum()}");

            var blockMaps = new List<BlockMap>();
            for(var i = 1; i < blocks.Count; i++) {
                var map = new BlockMap(blocks[i]);
                blockMaps.Add(map);
            }   

            blockMaps.Reverse();
            PrintLn($"blocks: {blockMaps.Count}");

            //transfrom the rules of one map into the destination format of the following map
            foreach(var sp in pairs) {
                PrintLn($"{sp.Start} to {sp.End} ");
            }

            long n = 60200000;

            while(true) {
                long current = n;
                foreach(var map in blockMaps) {
                    current = map.Unmap(current);
                }

                foreach(var pair in pairs) {
                    if(current >= pair.Start && current < pair.End)
                        return $"{n} -> {current}";
                }
                n++;
                if(n % 100000 == 0) {
                    PrintLn($"Attempt {n}...");
                }
            }
        }

        // turn "current" into one or more rules with destinations matching the same destination format as "targets"
        public List<BlockRule> TransformRules(BlockRule current, List<BlockRule> targets) {
            var orderedTargets = targets.OrderBy(r => r.SrcRangeStart).ToList();
            var outputs = new List<BlockRule>();
            //edge cases: no overlap at all: current is before targets
            if(current.DstRangeEnd <= orderedTargets.First().SrcRangeStart) {
                outputs.Add(current);
                return outputs;
            }

            //edge cases: no overlap at all: current is after targets
            if(current.DstRangeStart >= orderedTargets.Last().SrcRangeEnd) {
                outputs.Add(current);
                return outputs;
            }

            while(current.SrcRangeLen > 0) {
                // current Rules Destination must be inside a "SrcRang" of some target, otherwise its outside entirely
            }

            throw new NotImplementedException();
        }

        public class SeedPair {
            public long Start {get; set;}
            public long Len {get; set;}
            public long End {get {return Start + Len;}}
        }

        public List<List<string>> GetInputBlocks( List<string> input) {
            var rslt = new List<List<string>>();
            var current = new List<string>();
            foreach(var str in input) {
                if (str == "") {
                    rslt.Add(current);
                    current = new List<string>();
                } else {
                    current.Add(str);
                }
            }
            if (current.Count > 0) rslt.Add(current);
            return rslt;
        }

        public class BlockRule {
            public long DstRangeStart {get; set;}
            public long DstRangeEnd {get {return DstRangeStart + SrcRangeLen;}}
            public long SrcRangeStart {get; set;}
            public long SrcRangeLen {get; set;}

            public long SrcRangeEnd { get {return SrcRangeStart + SrcRangeLen;}}
        }

        public class BlockMap {
            public string Name {get; set;}
            public List<BlockRule> Rules {get; set;}

            public BlockMap(List<string> input) {
                Name = input[0];
                Rules = new List<BlockRule>();
                for(var x = 1; x < input.Count; x++) {
                    var nums = Utilties.StringToNums<long>(input[x], " ");
                    Rules.Add(new BlockRule{
                        DstRangeStart = nums[0],
                        SrcRangeStart = nums[1],
                        SrcRangeLen = nums[2],
                    });
                }
                Rules = Rules.OrderBy(r => r.SrcRangeStart).ToList();
            }

            public long Map(long input) {
                foreach(var rule in Rules) {
                    if(input >= rule.SrcRangeStart && input < rule.SrcRangeEnd) {
                        return (input - rule.SrcRangeStart) + rule.DstRangeStart;
                    }
                }
                return input;
            }

            public long Unmap(long output) {
                foreach(var rule in Rules) {
                    if(output >= rule.DstRangeStart && output < rule.DstRangeEnd) {
                        return (output - rule.DstRangeStart) + rule.SrcRangeStart;
                    }
                }
                return output;
            }
        }

        
    }
}