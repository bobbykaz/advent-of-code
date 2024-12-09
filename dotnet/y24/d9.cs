using System.Data;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using Grids;

namespace y24 {
    public class D9: AoCDay {

        public D9(): base(24, 9) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var input = lines[0];
            bool isFree = false;
            int id = 0;
            List<Block> blocks = [];
            foreach( var next in input.ToCharArray()) {
                var amt = int.Parse($"{next}");
                if(isFree){
                    for(int i = 0; i < amt; i++) {
                        blocks.Add(new Block(-1, true));
                    }
                } else {
                    for(int i = 0; i < amt; i++) {
                        blocks.Add(new Block(id, false));
                    }
                    id++;
                }

                isFree = !isFree;
            }

            defrag(blocks);

           
            return $"{checksum(blocks)}";
        }

        private void defrag(List<Block> blocks) {
            var freePtr = 0;
            while(!blocks[freePtr].isFree) {freePtr++;}

            var endPtr = blocks.Count-1;
            while(blocks[endPtr].isFree) {endPtr--;}

            while(freePtr < endPtr) {
                var f = blocks[freePtr];
                var e = blocks[endPtr];
                f.isFree = false;
                f.Id = e.Id;
                e.isFree = true;
                e.Id = -1;
                while(!blocks[freePtr].isFree) {freePtr++;}
                while(blocks[endPtr].isFree) {endPtr--;}
            }
        }

        private long checksum(List<Block> blocks) {
            var total = 0L;
            for(int i = 0; i < blocks.Count; i++) {
                if(!blocks[i].isFree) {
                    total += blocks[i].Id * i;
                }
            }

            return total;
        }

        public class Block {
            public bool isFree;
            public int Id;

            public int Size;
            public Block(int id, bool free) {
                Id = id;
                isFree = free;
            }

            public Block(int id, bool free, int size) {
                Id = id;
                isFree = free;
                Size = size;
            }
        }


        private void defrag2(List<Block> blocks) {
            var endPtr = blocks.Count-1;
            while(blocks[endPtr].isFree) {endPtr--;}

            var currentId = blocks[endPtr].Id;
            PrintLn($"Starting with ID {currentId}");
            while(currentId >= 0) {
                if(currentId % 5 == 0){
                    PrintLn($"At ID {currentId}");
                }
                // find the block of this ID going from end
                var toMove = blocks[endPtr];
                //find a free space that fits this block from beginning
                for(int i = 0; i < endPtr; i++) {
                    if(blocks[i].isFree) {
                        if(blocks[i].Size >= toMove.Size){
                            PrintLn($"Swapping block id {currentId} at {endPtr} with block {i}");
                            var diff = blocks[i].Size - toMove.Size;
                            toMove.isFree = true;
                            blocks[i].Id = toMove.Id;
                            blocks[i].isFree = false;
                            blocks[i].Size = toMove.Size;
                            if(blocks[i+1].isFree){
                                blocks[i+1].Size += diff;
                            } else {
                                blocks.Insert(i+1, new Block(-1, true, diff));
                                endPtr++;
                            }
                            break;
                        }
                    }
                }
                // decrement current id
                currentId--;
                // find next Id
                while(endPtr >= 0 && blocks[endPtr].Id != currentId) {endPtr--;}
            }
        }
        
        private long checksum2(List<Block> blocks) {
            var total = 0L;
            var i = 0;
            foreach(var b in blocks){
                if (b.isFree) {
                    i += b.Size;
                } else {
                    for(var n = 0; n < b.Size; n++){
                        total += b.Id * i;
                        i++;
                    }
                }
            }

            return total;
        }

        private void PrintBlocks(List<Block> bs) {
            if(_DebugPrinting){
                foreach(var b in bs){
                    for(int i = 0; i < b.Size; i++){
                        if (b.isFree) {
                            Print(".");
                        } else {
                            Print($"{b.Id}");
                        }        
                    }
                }
                PrintLn("");
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var input = lines[0];
            bool isFree = false;
            int id = 0;
            List<Block> blocks = [];
            foreach( var next in input.ToCharArray()) {
                var amt = int.Parse($"{next}");
                if(isFree){
                    blocks.Add(new Block(-1, true, amt));
                } else {
                    blocks.Add(new Block(id, false, amt));
                    id++;
                }

                isFree = !isFree;
            }

            PrintBlocks(blocks);
            
            defrag2(blocks);

            PrintBlocks(blocks);
           
            return $"{checksum2(blocks)}";
        }

    }
}