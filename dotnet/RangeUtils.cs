using Exception = System.Exception;

namespace RangeUtils;

// 2023, d18 for partial ranges
// 2025, d5 for merging ranges
public record Range(long Low, long High) {
    public long Length => High - Low + 1;

    public Range Copy() {
        return new Range(Low, High);
    }
    public bool CompletelyContains(Range other) {
        return this.Low <= other.Low && this.High >= other.High;
    }
            
    public bool Intersects(Range other)
    {
        if (this.CompletelyContains(other) || other.CompletelyContains(this))
            return true;
                
        // (this.start <= other.start <= this.end <= other.end
        var p1 = this.Low <= other.Low
                 &&other.Low <= this.High
                 && this.High <= other.High;

        // (other.start <= this.start <= other.end <= this.end
        var p2 = other.Low <= this.Low
                 && this.Low <= other.High
                 && other.High <= this.High;

        return p1 || p2;
    }

    public Range GetMergedRange(Range other, bool failIfNotIntersecting = true)
    {
        if (failIfNotIntersecting &&!this.Intersects(other))
            throw new Exception($"Range {this} does not intersect with {other}, cannot merge");
        
        long newLow = long.Min(this.Low, other.Low);
        long newHigh = long.Max(this.High, other.High);
        return new Range(newLow, newHigh);
    }

    public bool ContainsPoint(long pt) {
        return Low <= pt && High >= pt;
    }

    public override string ToString()
    {
        return $"({Low}, {High})";
    }
}