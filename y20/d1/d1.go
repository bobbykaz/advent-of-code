package d1

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20d1.txt")
	numbers := utilities.StringsToInts(input)
	for i := 0; i < len(numbers); i++ {
		for j := 0; j < len(numbers); j++ {
			if i != j {
				if (numbers[i] + numbers[j]) == 2020 {
					fmt.Printf("%d + %d = 2020; multiplied: %d\n", numbers[i], numbers[j], (numbers[i] * numbers[j]))

					return (numbers[i] * numbers[j])
				}
			}
		}
	}
	return -1
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20d1.txt")
	numbers := utilities.StringsToInts(input)
	for i := 0; i < len(numbers); i++ {
		for j := 0; j < len(numbers); j++ {
			if i != j {
				for k := 0; k < len(numbers); k++ {
					if i != k {
						n1 := numbers[i]
						n2 := numbers[j]
						n3 := numbers[k]
						if (n1 + n2 + n3) == 2020 {
							fmt.Printf("%d + %d + %d = 2020; multiplied: %d\n", n1, n2, n3, n1*n2*n3)

							return n1 * n2 * n3
						}
					}
				}
			}
		}
	}
	return -1
}
