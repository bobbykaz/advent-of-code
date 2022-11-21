package d1

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20/d1.txt")
	numbers := utilities.StringsToInts(input)
	fmt.Println(bruteForceDuo(numbers))
	fmt.Println(scanDuo(numbers))
	return -1
}

func scanDuo(numbers []int) int {
	sort.Ints(numbers)
	first, last := 0, len(numbers)-1
	for first < last {
		sum := numbers[first] + numbers[last]
		if sum == 2020 {
			fmt.Printf("Scan: %d + %d = 2020; multiplied: %d\n", numbers[first], numbers[last], (numbers[first] * numbers[last]))
			return (numbers[first] * numbers[last])
		}

		if sum < 2020 {
			first++
		} else {
			last--
		}
	}
	fmt.Println("Scan failed")
	return -1
}

func bruteForceDuo(numbers []int) int {
	for i := 0; i < len(numbers); i++ {
		for j := 0; j < len(numbers); j++ {
			if i != j {
				if (numbers[i] + numbers[j]) == 2020 {
					fmt.Printf("Brute force: %d + %d = 2020; multiplied: %d\n", numbers[i], numbers[j], (numbers[i] * numbers[j]))

					return (numbers[i] * numbers[j])
				}
			}
		}
	}
	fmt.Println("brute force failed...how?")
	return -1
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20/d1.txt")
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
