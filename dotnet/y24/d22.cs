namespace y24 {
    public class D22: AoCDay {

        public D22(): base(24, 22) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();;
            var total = 0L;
            foreach(var secret in lines){
                var s = long.Parse(secret);
                for(int i = 0; i < 2000; i ++){
                    s = EvolveSecret(s);
                }
                total += s;
            }
            
            return $"{total}";
        }

        private long EvolveSecret(long baseSecret) {
            var secret = baseSecret;
            var s1 = secret * 64; 
            secret = Mix(s1, secret);
            secret = Prune(secret);

            var s4 = secret / 32;
            secret = Mix(s4, secret);
            secret = Prune(secret); 

            var s5 = secret * 2048;
            secret = Mix(s5, secret);
            secret = Prune(secret);
            return secret;
        }

        private long PriceFromSecret(long secret){
            return secret % 10;
        }

        private long Prune(long s) {
            return s % 16777216;
        }

        private long Mix(long s, long secret) {
            return s ^ secret;
        }

        private class Monkey {
            public List<long> Prices = [];
            public List<long> Deltas = [];
            public Dictionary<ChangeOrder, long> PriceMap = [];
            
            public void MapChanges(HashSet<ChangeOrder> allOrders) {
                for(int i = 4; i < Prices.Count; i++) {
                    var price = Prices[i];
                    var co = new ChangeOrder(Deltas[i-3],Deltas[i-2],Deltas[i-1],Deltas[i]);
                    //Console.WriteLine($"Price {price} at {co}");
                    if(!PriceMap.ContainsKey(co)){
                        PriceMap[co] = price;
                        allOrders.Add(co);
                    }
                }
            }
        }

        private record struct ChangeOrder(long a, long b, long c, long d);
        public override string P2()
        {
            var lines = InputAsLines();
            HashSet<ChangeOrder> allOrders = [];
            List<Monkey> monkeys = [];

            foreach(var secret in lines){
                var s = long.Parse(secret);
                List<long> prices = [];
                prices.Add(PriceFromSecret(s));
                List<long> deltas = [];
                deltas.Add(long.MaxValue);
                for(int i = 0; i < 2000; i ++){
                    s = EvolveSecret(s);
                    var thisPrice = PriceFromSecret(s);
                    var lastPrice = prices[prices.Count() - 1];
                    prices.Add(thisPrice);
                    deltas.Add(thisPrice - lastPrice);
                }
                var m = new Monkey {Prices = prices, Deltas = deltas};
                m.MapChanges(allOrders);
                monkeys.Add(m);
            }
            var total = 0L;
            foreach(var co in allOrders) {
                var temp = 0L;
                foreach(var m in monkeys){
                    if(m.PriceMap.ContainsKey(co)){
                        temp += m.PriceMap[co];
                    }
                }
                if (temp > total) {
                    PrintLn($"new max: {temp} from {co}");
                    total = temp;
                }
            }
            
            return $"{total}";
        }

    }
}