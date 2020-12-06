package d6

import (
	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20d6.txt")

	groups := utilities.GroupLinesByLineSeparator(input, "")

	sum := 0
	for _, g := range groups {
		sum += groupQs(g)
	}

	return sum
}

func groupAnswers(g []string) map[rune]int {
	dict := make(map[rune]int)
	for _, s := range g {
		for _, c := range s {
			_, exists := dict[c]
			if exists {
				dict[c]++
			} else {
				dict[c] = 1
			}
		}
	}

	return dict
}

func groupQs(group []string) int {
	dict := groupAnswers(group)

	return len(dict)
}

func groupQs2(group []string) int {
	dict := groupAnswers(group)

	sum := 0
	for _, v := range dict {
		if v == len(group) {
			sum++
		}
	}

	return sum
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20d6.txt")

	groups := utilities.GroupLinesByLineSeparator(input, "")

	sum := 0
	for _, g := range groups {
		sum += groupQs2(g)
	}

	return sum
}
