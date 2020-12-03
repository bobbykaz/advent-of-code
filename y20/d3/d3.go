package d3

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20d3.txt")

	return TreesOnPath(1, 3, input)
}

func TreesOnPath(rmod int, cmod int, input []string) int {
	rows := len(input)
	cols := len(input[0])
	r, c := 0, 0
	trees := 0
	for r < rows-rmod {
		r += rmod
		c += cmod
		target := input[r][c%cols]
		if rune(target) == '#' {
			trees++
		}
	}
	fmt.Println("Slope: right", cmod, "down", rmod, "trees: ", trees)
	return trees
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20d3.txt")
	s1 := TreesOnPath(1, 1, input)
	s2 := TreesOnPath(1, 3, input)
	s3 := TreesOnPath(1, 5, input)
	s4 := TreesOnPath(1, 7, input)
	s5 := TreesOnPath(2, 1, input)
	return s1 * s2 * s3 * s4 * s5
}
