using Grids;

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

    public static Grid<char> RectangularCharGridFromLines(List<String> lines) {
        var h = lines.Count;
        var w = lines[0].Length;
        var g = new Grid<char>(w,h,'.');
        for(int r = 0; r < lines.Count; r++) {
            var chars = lines[r].ToCharArray();
            for(int c = 0; c < chars.Length; c++){
                g.G[r][c] = chars[c];
            }
        }

        return g;
    }

    public static Grid<int> RectangularNGridFromLines(List<String> lines) {
        var h = lines.Count;
        var w = lines[0].Length;
        var g = new Grid<int>(w,h,0);
        for(int r = 0; r < lines.Count; r++) {
            var chars = lines[r].ToCharArray();
            for(int c = 0; c < chars.Length; c++){
                g.G[r][c] = int.Parse($"{chars[c]}");
            }
        }

        return g;
    }

    public static Grid<int> NGrid(int w, int h, int def) {
        var g = new Grid<int>(w,h,def);

        return g;
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

    public static long GCD(long[] nums) {
        if(nums.Length < 2) throw new ArgumentException("needs at least 2 nums");

        if(nums.Length == 2) {
            var a = nums[0];
            var b = nums[1];
            while(b!=0) {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        } else {
            var subGCD = GCD(nums.Skip(1).ToArray());
            return GCD(new long[]{subGCD, nums[0]});
        }
    }

    public static long LCM(long[] nums) {
        if(nums.Length < 2) throw new ArgumentException("needs at least 2 nums");

        if(nums.Length == 2) {
            var a = nums[0];
            var b = nums[1];
            var gcd = GCD(new long[]{a, b});

            return a * b / gcd;
        } else {
            var subLCM = LCM(nums.Skip(1).ToArray());
            return LCM(new long[]{subLCM, nums[0]});
        }
    }

}