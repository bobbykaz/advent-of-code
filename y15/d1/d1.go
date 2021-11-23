package d1

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part2() int {
	fmt.Println()
	lines := utilities.ReadFileIntoLines("input/y15/d1.txt")
	line := lines[0]
	total := 0
	firstNeg := false
	for i := 0; i < len(line); i++ {
		if !firstNeg && total < 0 {
			fmt.Println("Negative after ", i)
			firstNeg = true
		}
		if line[i] == '(' {
			total = total + 1
		} else {
			total = total - 1
		}
	}

	return total
}
