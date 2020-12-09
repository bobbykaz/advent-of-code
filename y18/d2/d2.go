package d2

import (
	"fmt"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input2.txt")
	twos, threes := 0, 0
	for i := 0; i < len(input); i++ {
		v1, v2 := checkId(input[i])

		if v1 {
			twos++
		}

		if v2 {
			threes++
		}
	}

	fmt.Println("Checksum: ", twos*threes)

}

func Part2() {
	input := utilities.ReadFileIntoLines("input/input2.txt")
	for i := 0; i < len(input); i++ {
		first := input[i]
		for j := 0; j < len(input); j++ {
			if i != j {
				second := input[j]
				diff := findFirstDiffPos(first, second)
				fmt.Printf("strings %s, %s differ at %d \n", first, second, diff)
				if first[diff+1:] == second[diff+1:] {
					fmt.Println("Match!")
					i = len(input)
					j = len(input)
				}
			}
		}
	}
}

//First bool returns true if a letter appears twice, second bool if thrice
func checkId(line string) (bool, bool) {
	m := make(map[byte]int)
	twice, thrice := false, false
	for i := 0; i < len(line); i++ {
		_, present := m[line[i]]

		if !present {
			m[line[i]] = 1
		} else {
			m[line[i]]++
		}
	}

	for _, v := range m {
		if v == 2 {
			twice = true
		}
		if v == 3 {
			thrice = true
		}
	}

	return twice, thrice
}

func findFirstDiffPos(a, b string) int {
	//unsafe, assumes good input and strings are equal length
	for i := 0; i < len(a); i++ {
		if a[i] != b[i] {
			return i
		}
	}
	return -1
}
