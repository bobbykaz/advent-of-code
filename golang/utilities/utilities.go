package utilities

import (
	"fmt"
	"io/ioutil"
	"math"
	"sort"
	"strconv"
	"strings"
)

//ReadFileIntoLines reads a text file into lines. Assumes LF line ending, will ignore a single blank line at the end of the file.
func ReadFileIntoLines(filename string) []string {
	b, err := ioutil.ReadFile(filename)
	if err != nil {
		panic(err)
	}
	file := string(b)
	file = strings.ReplaceAll(file, "\r", "")
	lines := strings.Split(file, "\n")
	if lines[len(lines)-1] == "" {
		lines = lines[:(len(lines) - 1)]
	}
	return lines
}

//GroupLinesByLineSeparator parses an input file of text lines into blocks based on a specific line separator
func GroupLinesByLineSeparator(lines []string, separator string) [][]string {
	groups := make([][]string, 0)
	group := make([]string, 0)
	for _, s := range lines {
		if s == separator {
			groups = append(groups, group)
			group = make([]string, 0)
		} else {
			group = append(group, s)
		}
	}

	//final group
	if len(group) > 0 {
		groups = append(groups, group)
	}

	return groups
}

//StringToInts parses a comma-separated string of ints
func StringToInts(str string) []int {
	ints := strings.Split(str, ",")
	var output = make([]int, len(ints))
	for i, s := range ints {
		temp, err := strconv.Atoi(s)
		if err != nil {
			panic(err)
		}
		output[i] = temp
	}

	return output
}

//StringsToInts parses a slice of input into ints
func StringsToInts(strs []string) []int {
	var output = make([]int, len(strs))
	for i, s := range strs {
		temp, err := strconv.Atoi(s)
		if err != nil {
			panic(err)
		}
		output[i] = temp
	}

	return output
}

//StringsToInts64 parses a slice of input into int64s
func StringsToInts64(strs []string) []int64 {
	var output = make([]int64, len(strs))
	for i, s := range strs {
		temp, err := strconv.ParseInt(s, 10, 64)
		if err != nil {
			panic(err)
		}
		output[i] = temp
	}

	return output
}

//StringContainsAllRunes returns true only if all of rs is found in str
func StringContainsAllRunes(str string, rs []rune) bool {
	for i := 0; i < len(rs); i++ {
		if !strings.ContainsRune(str, rs[i]) {
			return false
		}
	}
	return true
}

//StringContainsAnyRune returns true if any one of rs is found in str
func StringContainsAnyRune(str string, rs []rune) bool {
	for i := 0; i < len(rs); i++ {
		if strings.ContainsRune(str, rs[i]) {
			return true
		}
	}
	return false
}

//StringSliceContains returns true str is found in strs
func StringSliceContains(str string, strs []string) bool {
	for i := 0; i < len(strs); i++ {
		if str == strs[i] {
			return true
		}
	}
	return false
}

//StringSliceCountInstances counts the number of occurrences of str in strs
func StringSliceCountInstances(str string, strs []string) int {
	sum := 0
	for i := 0; i < len(strs); i++ {
		if str == strs[i] {
			sum++
		}
	}
	return sum
}

//IntSliceEqual compares two int slices
func IntSliceEqual(a, b []int) bool {
	if len(a) != len(b) {
		return false
	}

	for i := range a {
		if a[i] != b[i] {
			return false
		}
	}

	return true
}

//FindMinMax returns the min and max of a int slice
func FindMinMax(ints []int) (int, int) {
	min, max := ints[0], ints[0]
	for _, v := range ints {
		if v > max {
			max = v
		}
		if v < min {
			min = v
		}
	}
	return min, max
}

//FindMinMax64 returns the min and max of a int64 slice
func FindMinMax64(ints []int64) (int64, int64) {
	min, max := ints[0], ints[0]
	for _, v := range ints {
		if v > max {
			max = v
		}
		if v < min {
			min = v
		}
	}
	return min, max
}

//GCD finds the Greatest Common Denominator of a set of ints
func GCD(ints []int) int {
	if len(ints) < 2 {
		panic("GCD needs at least 2 inputs")
	}
	if len(ints) == 2 {
		a, b := ints[0], ints[1]
		for b != 0 {
			t := b
			b = a % b
			a = t
		}
		return a
	} else {
		subGCD := GCD(ints[1:])
		return GCD([]int{subGCD, ints[0]})
	}
}

//GCD64 finds the Greatest Common Denominator of a set of 64 bit ints
func GCD64(ints []int64) int64 {
	if len(ints) < 2 {
		panic("GCD64 needs at least 2 inputs")
	}
	if len(ints) == 2 {
		a, b := ints[0], ints[1]
		for b != 0 {
			t := b
			b = a % b
			a = t
		}
		return a
	} else {
		subGCD := GCD64(ints[1:])
		return GCD64([]int64{subGCD, ints[0]})
	}
}

//LCM finds the Least Common Multiple of a set of ints
func LCM(ints []int) int {
	if len(ints) < 2 {
		panic("LCM needs at least 2 inputs")
	}
	if len(ints) == 2 {
		a, b := ints[0], ints[1]
		gcd := GCD([]int{a, b})
		return a * b / gcd
	} else {
		subLCM := LCM(ints[1:])
		return GCD([]int{subLCM, ints[0]})
	}
}

//LCM64 finds the Least Common Multiple of a set of 64 bit ints
func LCM64(ints []int64) int64 {
	if len(ints) < 2 {
		panic("LCM64 needs at least 2 inputs")
	}
	if len(ints) == 2 {
		a, b := ints[0], ints[1]
		gcd := GCD64([]int64{a, b})
		return a * b / gcd
	} else {
		subLCM := LCM64(ints[1:])
		return GCD64([]int64{subLCM, ints[0]})
	}
}

//IntsToString returns a comma-separated string of the input
func IntsToString(input []int) string {
	var s string = fmt.Sprintf("%d", input[0])
	for _, item := range input[1:] {
		s = fmt.Sprintf("%s,%d", s, item)
	}

	return s
}

//Split is a fancy split for multiple separators, processed left to right
func Split(str string, pts ...string) []string {
	output := make([]string, 0)
	current := str
	for i := 0; i < len(pts); i++ {
		tmp := strings.SplitN(current, pts[i], 2)
		output = append(output, tmp[0])
		current = tmp[1]
	}
	output = append(output, current)
	return output
}

//ParseCoord parses an X,Y style coordinate and also removes the beginning and end strings provided, and trims spaces everywhere
func ParseCoord(coordStr, beginning, separator, end string) (int, int, error) {
	trimmed := strings.TrimSpace(coordStr)
	trimmed = strings.TrimPrefix(trimmed, beginning)
	trimmed = strings.TrimSpace(trimmed)
	trimmed = strings.TrimSuffix(trimmed, end)
	trimmed = strings.TrimSpace(trimmed)
	parts := strings.Split(trimmed, separator)
	x, err := strconv.Atoi(strings.TrimSpace(parts[0]))
	if err != nil {
		return -1, -1, err
	}

	y, err := strconv.Atoi(strings.TrimSpace(parts[1]))
	if err != nil {
		return -1, -1, err
	}

	return x, y, nil
}

//ParseDateStyleString expects string to be equivalent to "2000-10-20". Trims spaces.
func ParseDateStyleString(date string) (int, int, int, error) {
	trimmed := strings.TrimSpace(date)
	parts := strings.Split(trimmed, "-")
	if len(parts) != 3 {
		return -1, -1, -1, fmt.Errorf("expected 3 parts to date %s", trimmed)
	}

	y, err := strconv.Atoi(parts[0])
	if err != nil {
		return -1, -1, -1, err
	}

	m, err := strconv.Atoi(parts[1])
	if err != nil {
		return -1, -1, -1, err
	}

	d, err := strconv.Atoi(parts[2])
	if err != nil {
		return -1, -1, -1, err
	}

	return y, m, d, nil
}

//ParseTimeStyleString expects string to be equivalent to "00:11". Trims spaces.
func ParseTimeStyleString(time string) (int, int, error) {
	trimmed := strings.TrimSpace(time)
	parts := strings.Split(trimmed, ":")
	if len(parts) != 2 {
		return -1, -1, fmt.Errorf("expected 2 parts to date %s", trimmed)
	}

	h, err := strconv.Atoi(parts[0])
	if err != nil {
		return -1, -1, err
	}

	m, err := strconv.Atoi(parts[1])
	if err != nil {
		return -1, -1, err
	}

	return h, m, nil
}

//Intersect returns the intersection of two int-slices
func Intersect(a, b []int) []int {
	sort.Ints(a)
	sort.Ints(b)
	ac, bc := 0, 0
	r := make([]int, 0)
	for ac < len(a) && bc < len(b) {
		av, bv := a[ac], b[bc]
		if av == bv {
			r = append(r, av)
			ac++
			bc++
		} else if av < bv {
			ac++
		} else {
			bc++
		}
	}

	return r
}

//IntersectString returns the intersection of two string-slices
func IntersectString(a, b []string) []string {
	sort.Strings(a)
	sort.Strings(b)
	ac, bc := 0, 0
	r := make([]string, 0)
	for ac < len(a) && bc < len(b) {
		av, bv := a[ac], b[bc]
		if av == bv {
			r = append(r, av)
			ac++
			bc++
		} else if av < bv {
			ac++
		} else {
			bc++
		}
	}

	return r
}

func DedupeStrings(a []string) []string {
	sort.Strings(a)
	r := make([]string, 0)
	prev := a[0]
	r = append(r, prev)
	for i := 0; i < len(a); i++ {
		if a[i] != prev {
			prev = a[i]
			r = append(r, prev)
		}
	}

	return r
}

func ManhattenDistance(x1, y1, x2, y2 int) int {
	return int(math.Abs(float64(x1-x2)) + math.Abs(float64(y1-y2)))
}
