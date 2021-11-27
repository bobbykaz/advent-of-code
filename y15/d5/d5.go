package d5

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	fmt.Println()
	lines := utilities.ReadFileIntoLines("input/y15/d5.txt")
	g, b := 0, 0
	for _, l := range lines {
		if checkStrTwice(l) {
			println(l)
			g++
		} else {
			b++
		}
	}

	return g
}

//returns true if string meets conditions
func checkString(str string) bool {
	if strings.Contains(str, "ab") {
		return false
	}

	if strings.Contains(str, "cd") {
		return false
	}

	if strings.Contains(str, "pq") {
		return false
	}

	if strings.Contains(str, "xy") {
		return false
	}

	vowls := 0
	hasDoub := false

	if isVowel(str[0]) {
		vowls++
	}

	for i := 1; i < len(str); i++ {
		if isVowel(str[i]) {
			vowls++
		}

		if str[i] == str[i-1] {
			hasDoub = true
		}
	}
	return hasDoub && vowls >= 3
}

func isVowel(c byte) bool {
	switch c {
	case 'a':
		fallthrough
	case 'e':
		fallthrough
	case 'i':
		fallthrough
	case 'o':
		fallthrough
	case 'u':
		return true
	}
	return false
}

func checkStrTwice(str string) bool {
	return hasSplitPair(str) && hasNonOverlappingPair(str)
}

func hasSplitPair(str string) bool {
	for i := 2; i < len(str); i++ {
		if str[i] == str[i-2] {
			return true
		}
	}
	return false
}

func hasNonOverlappingPair(str string) bool {
	for i := 0; i < len(str)-1; i++ {
		target := str[i : i+2]
		if strings.Contains(str[i+2:], target) {
			return true
		}
	}
	return false
}
