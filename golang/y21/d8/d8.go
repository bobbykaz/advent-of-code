package d8

import (
	"fmt"
	"sort"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d8.txt")

	fmt.Println("Part1:", part1(lines))

	fmt.Println("Part2:", part2(lines))
	return 0
}

func part1(lines []string) int {
	uniq := 0
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], " | ")
		answers := strings.Split(pts[1], " ")
		for j := 0; j < len(answers); j++ {
			l := len(answers[j])
			if l == 2 || l == 3 || l == 4 || l == 7 {
				uniq++
			}
		}
	}
	return uniq
}

func part2(lines []string) int {
	total := 0
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], " | ")
		signals := strings.Split(pts[0], " ")
		solution := solve(signals)
		output := strings.Split(pts[1], " ")
		answer := getNumber(solution, output)
		fmt.Println("...", answer)
		total += answer
	}
	return total
}

/*
 aaaa
b    c
b    c
 dddd
e    f
e    f
 gggg
*/
//output will be characters in above positions, ex, abcdefg if untransposed
func solve(signals []string) string {

	sorted := make([]string, 0)
	for i := 0; i < len(signals); i++ {
		sigAsRune := runeList([]rune(signals[i]))
		sort.Sort(sigAsRune)
		sorted = append(sorted, string(sigAsRune))
	}
	sort.Sort(byLen(sorted))
	fmt.Println(sorted)
	//order is digit 1, digit 7, digit 4, | 012
	//digits (2,3,5, sub order not guaranteed) |345
	l51 := sorted[3]
	l52 := sorted[4]
	l53 := sorted[5]
	// digits(0,6,9 sub order not guaranteed) |678
	// digit 8 | 9

	//find 1 - that gives us c, f
	//find 7 - definitely gives us a
	aRunes := removeRunes([]rune(sorted[1]), []rune(sorted[0]))
	a := aRunes[0]
	// find common letter between len-4 (4) and len-5 (2,3,5) - that gives us d
	dRunes := commonRunes([]rune(sorted[2]), []rune(l51))
	dRunes = commonRunes(dRunes, []rune(l52))
	dRunes = commonRunes(dRunes, []rune(l53))
	d := dRunes[0]
	// remove d and c,f from len-4 (4) - we get b
	bRunes := removeRunes([]rune(sorted[2]), dRunes)
	bRunes = removeRunes(bRunes, []rune(sorted[0]))
	// b
	b := bRunes[0]
	// the len-5s(0,6,9)- common runes are adg, remove ad and you get g
	gRunes := commonRunes([]rune(l51), []rune(l52))
	gRunes = commonRunes(gRunes, []rune(l53))
	gRunes = removeRunes(gRunes, []rune{a, d})
	g := gRunes[0]

	// len-5s - the one that has abdg also contains f
	fTarget := l51
	if utilities.StringContainsAllRunes(l52, []rune{a, b, d, g}) {
		fTarget = l52
	} else if utilities.StringContainsAllRunes(l53, []rune{a, b, d, g}) {
		fTarget = l53
	}
	fRunes := removeRunes([]rune(fTarget), []rune{a, b, d, g})
	f := fRunes[0]
	//go back to len-2, remove f, you get c
	cRunes := removeRunes([]rune(sorted[0]), fRunes)
	c := cRunes[0]
	// len-7 - remove all known to get e
	eRunes := removeRunes([]rune(sorted[9]), []rune{a, b, c, d, f, g})
	e := eRunes[0]

	str := fmt.Sprintf("%c%c%c%c%c%c%c", a, b, c, d, e, f, g)
	println("Solution:", str)
	return str
}

func removeRunes(from, toRemove []rune) []rune {
	result := make([]rune, 0)
	for i := 0; i < len(from); i++ {
		s := string(from[i])
		if !utilities.StringContainsAnyRune(s, toRemove) {
			result = append(result, from[i])
		}
	}
	return result
}

func commonRunes(a, b []rune) []rune {
	result := make([]rune, 0)
	for i := 0; i < len(a); i++ {
		for j := 0; j < len(b); j++ {
			if a[i] == b[j] {
				result = append(result, a[i])
				j = len(b)
			}
		}
	}
	return result
}

type byLen []string

func (a byLen) Len() int           { return len(a) }
func (a byLen) Less(i, j int) bool { return len(a[i]) < len(a[j]) }
func (a byLen) Swap(i, j int)      { a[i], a[j] = a[j], a[i] }

type runeList []rune

func (s runeList) Less(i, j int) bool { return s[i] < s[j] }
func (s runeList) Len() int           { return len(s) }
func (s runeList) Swap(i, j int)      { s[i], s[j] = s[j], s[i] }

func getNumber(solution string, digits []string) int {
	a := getDigit(solution, digits[0]) * 1000
	b := getDigit(solution, digits[1]) * 100
	c := getDigit(solution, digits[2]) * 10
	d := getDigit(solution, digits[3])

	return a + b + c + d
}

func getDigit(solution, digit string) int {
	l := len(digit)
	switch l {
	case 2:
		return 1
	case 3:
		return 7
	case 4:
		return 4
	case 7:
		return 8
	}
	rs := []rune(solution)
	if utilities.StringContainsAllRunes(digit, []rune{rs[0], rs[1], rs[2], rs[4], rs[5], rs[6]}) &&
		!utilities.StringContainsAnyRune(digit, []rune{rs[3]}) {
		return 0
	}
	if utilities.StringContainsAllRunes(digit, []rune{rs[0], rs[2], rs[3], rs[4], rs[6]}) &&
		!utilities.StringContainsAnyRune(digit, []rune{rs[1], rs[5]}) {
		return 2
	}
	if utilities.StringContainsAllRunes(digit, []rune{rs[0], rs[2], rs[3], rs[5], rs[6]}) &&
		!utilities.StringContainsAnyRune(digit, []rune{rs[1], rs[4]}) {
		return 3
	}
	if utilities.StringContainsAllRunes(digit, []rune{rs[0], rs[1], rs[3], rs[5], rs[6]}) &&
		!utilities.StringContainsAnyRune(digit, []rune{rs[2], rs[4]}) {
		return 5
	}
	if utilities.StringContainsAllRunes(digit, []rune{rs[0], rs[1], rs[3], rs[4], rs[5], rs[6]}) &&
		!utilities.StringContainsAnyRune(digit, []rune{rs[2]}) {
		return 6
	}

	return 9
}
