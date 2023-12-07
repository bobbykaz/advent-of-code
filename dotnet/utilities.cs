using System.Xml.XPath;

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

    public static List<List<string>> GroupInputAsBlocks( List<string> input, string blockSeparator = "") {
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

    public static List<T> StringToNums<T>(string str, string separator = ",") where T: IParsable<T> {
      var pts = str.Split(separator);
      var result = pts.Select(s => T.Parse(s, null)).ToList();
      return result;
    }

/// <summary>
/// Fancy split for multiple separators, processed left to right
/// </summary>
    public static List<String> Split(string str, List<string>pts) {
      var result = new List<string>();
      var current = str;
      for(int i = 0; i < pts.Count; i++) {
         var pivot = current.IndexOf(pts[i]);
         var first = current.Substring(0, pivot);
         result.Add(first);
         current = current.Substring(pivot + pts[i].Length);
      }

      return result;
    }

}