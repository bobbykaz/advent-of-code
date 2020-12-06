package d6

import (
	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20d6.txt")

	groups := make([]string, 0)
	group := ""
	for _, s := range input {
		if s == "" {
			groups = append(groups, group)
			group = ""
		} else {
			group = group + s
		}
	}
	groups = append(groups, group)

	sum := 0
	for _, g := range groups {
		sum += groupQs(g)
	}

	return sum
}

func groupQs(group string) int {
	dict := make(map[rune]int)
	for _, c := range group {
		dict[c] = 1
	}

	sum := 0
	for _, v := range dict {
		sum += v
	}

	return sum
}

func groupQs2(group []string) int {
	dict := make(map[rune]int)
	for _, s := range group {
		for _, c := range s {
			_, exists := dict[c]
			if exists {
				dict[c]++
			} else {
				dict[c] = 1
			}
		}
	}

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

	groups := make([][]string, 0)
	group := make([]string, 0)
	for _, s := range input {
		if s == "" {
			groups = append(groups, group)
			group = make([]string, 0)
		} else {
			group = append(group, s)
		}
	}
	groups = append(groups, group)

	sum := 0
	for _, g := range groups {
		sum += groupQs2(g)
	}

	return sum
}
