namespace Visualization {
    public class Graph {
        public static void UndirectedGraphvizVisualization(string outputFilename, List<(string, string)> connectedNodes) {
            var dotFileContents = "";
            foreach(var pair in connectedNodes) {
                dotFileContents += $"{pair.Item1} -- {pair.Item2}\n";
            }
            dotFileContents = $"graph {{ \n{dotFileContents} }}";
            File.WriteAllText($"{outputFilename}.dot", dotFileContents);

            Console.WriteLine("Run the following from a CMD prompt (not powershell)");
            Console.WriteLine($" \"C:\\Program Files\\Graphviz\\bin\\sfdp.exe\" -x -Goverlap=scale -Tpdf -o {outputFilename}.pdf {outputFilename}.dot");
        }

        public static void DirectedGraphvizVisualization(string outputFilename, List<(string, string)> connectedNodes) {
            var dotFileContents = "";
            foreach(var pair in connectedNodes) {
                dotFileContents += $"{pair.Item1} -> {pair.Item2}\n";
            }
            dotFileContents = $"digraph {{ \n{dotFileContents} }}";
            File.WriteAllText($"{outputFilename}.dot", dotFileContents);

            Console.WriteLine("Run the following from a CMD prompt (not powershell)");
            Console.WriteLine($" \"C:\\Program Files\\Graphviz\\bin\\sfdp.exe\" -x -Goverlap=scale -Tpdf -o {outputFilename}.pdf {outputFilename}.dot");
        }
    }
}