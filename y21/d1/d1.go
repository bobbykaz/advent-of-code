package d1

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y21/d1.txt")
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

	return -1
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y21/d1.txt")
	numbers := utilities.StringsToInts(input)
	for i := 0; i < len(numbers); i++ {
		println(numbers[i])
	}
	return -1
}
