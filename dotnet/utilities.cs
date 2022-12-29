public class Utilties {
    public static List<String> ReadFileToLines(string filename) {
         var lines = File.ReadAllLines(filename);
         if (lines == null) {
            throw new Exception("no input read");
         }

         var list = lines.ToList();
         if (String.IsNullOrWhiteSpace(list.Last())) {
            list.RemoveAt(lines.Count() - 1);
         }

         return list;
    }

}