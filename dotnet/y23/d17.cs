using Grids;

namespace y23 {
    public class D17 : AoCDay
    {
        public D17(): base(23, 17) {
            _DebugPrinting = false;
            Grid = new Grid<int>(1,1,1);
        }
        protected Grid<int> Grid;
        protected PriorityQueue<Turn,int> Turns = new PriorityQueue<Turn,int>();
        public override string P1()
        {
            var lines = InputAsLines();
            Grid = Utilties.RectangularNGridFromLines(lines);
            //enqueue both directions with a "0" move in direction to account for starting a run either way
            Turns.Enqueue(new Turn(0,0,0,Dir.S,0, null), 0);
            Turns.Enqueue(new Turn(0,0,0,Dir.E,0, null), 0);
            var final = findMinPath(true);
            
            //PrintPath(final);

            return $"{final.heatloss}";
        }

        public Turn findMinPath(bool isP1) {
            var minMap = new Dictionary<string,Turn>();
            Turn? minTurn = null;
            while(Turns.Count > 0) {
                var current = Turns.Dequeue();
                if(current.r == Grid.LastRowIndex && current.c == Grid.LastColIndex) {
                    if (minTurn == null) {
                        minTurn = current;
                    }
                    if (current.heatloss < minTurn.heatloss)
                        minTurn = current;
                } else {
                    //PrintLn($"{next.r}-{next.c} : {next.dir} - {next.heatloss}");
                    var possibleDirs = DIRS.Where(d => d != GridUtilities.OppositeDir(current.dir)).ToList();
                    var cells = possibleDirs.Select(d => (d,Grid.GetNeighbor(current.r, current.c,d))).ToList();
                    foreach(var (d,cell) in cells) {
                        if(cell != null) {
                            var c = cell.Value;
                            var (newMoves, validMove) = newMovesCanMove(isP1, current, d);
                            if(validMove) {
                                var newTurn = new Turn(c.R, c.C, current.heatloss + c.V, d, newMoves, current);
                                if(isMin(minMap, newTurn)){
                                    Turns.Enqueue(newTurn,newTurn.heatloss);
                                }
                            } else {
                                PrintLn($"not going from {current} {d} because limit");
                            }
                            
                        }
                        
                    }
                }
            }
            return minTurn;
        }

        public (int,bool) newMovesCanMove(bool isP1, Turn current, Dir nextdir) {
            var newMoves = nextdir == current.dir ? current.movesInDir +1 : 1;
            if(isP1) {
                return(newMoves, newMoves<=3);
            } else {
                if(nextdir == current.dir) {
                    return(newMoves, newMoves<=10);
                } else {
                    return(newMoves, current.movesInDir >= 4);
                }
            }
        }

        public void PrintPath(Turn finalTurn) {
            var g = Utilties.RectangularCharGridFromLines(InputAsLines());
            var turn = finalTurn;
            while(turn != null) {
                PrintLn($"{turn}");
                g.G[turn.r][turn.c] = turn.dir switch {
                    Dir.N => '^',
                    Dir.S => 'v',
                    Dir.E => '>',
                    Dir.W => '<',
                    _ => '!',
                };
                turn = turn.prevTurn;
            }

            g.Print();
        }

        public bool isMin(Dictionary<string, Turn> minMap, Turn turn) {
            var key = $"{turn.r}-{turn.c}-{turn.dir}-{turn.movesInDir}";
            if(!minMap.ContainsKey(key)) {
                minMap[key] = turn;
                return true;
            }

            if(turn.heatloss < minMap[key].heatloss) {
                minMap[key] = turn;
                return true;
            }

            return false;
        }

        Dir[] DIRS = new Dir[]{Dir.N, Dir.E, Dir.S, Dir.W};
        public class Turn {
            public int r;
            public int c;
            public int heatloss;
            public Dir dir;
            public int movesInDir;
            public Turn? prevTurn;
            public Turn(int row, int col, int heat, Dir curDir, int moves, Turn? prev) {
                r = row;
                c = col;
                heatloss = heat;
                dir = curDir;
                movesInDir = moves;
                prevTurn = prev;
            }

            public override string ToString()
            {
                return $"({r}-{c}) - {dir} ({movesInDir}) : {heatloss}";
            }
        }
    
        public override string P2()
        {
            var lines = InputAsLines();
            Grid = Utilties.RectangularNGridFromLines(lines);
            Turns = new PriorityQueue<Turn,int>();
            //enqueue both directions with a "0" move in direction to account for starting a run either way
            Turns.Enqueue(new Turn(0,0,0,Dir.S,0, null), 0);
            Turns.Enqueue(new Turn(0,0,0,Dir.E,0, null), 0);
            var final = findMinPath(false);

            return $"{final.heatloss}";
        }
    }
}