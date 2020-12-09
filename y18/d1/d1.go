package d1

import (
	"fmt"
	"strconv"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/input1.txt")
	freq := 0
	for i := 0; i < len(input); i++ {
		output, err := processLine(input[i])
		if err != nil {
			fmt.Printf("error converting %s", input[i])
			panic(err)
		}

		freq += output
	}

	fmt.Println("Final freq: ", freq)
	return freq
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/input1.txt")
	intSet := make(map[int]bool)
	freq := 0
	for i := 0; true; i++ {
		if i >= len(input) {
			fmt.Println("wrapping around...")
			i = 0
		}

		output, err := processLine(input[i])
		if err != nil {
			fmt.Printf("error converting %s", input[i])
			panic(err)
		}

		freq += output

		_, present := intSet[freq]
		if present {
			fmt.Println("Duplicate freq found: ", freq)
			return freq
		}
		intSet[freq] = true
	}

	fmt.Println("dead code")
	panic(nil)
}

func processLine(line string) (int, error) {
	number := line[1:]
	if line[0] == '+' {
		return strconv.Atoi(number)
	} else {
		return strconv.Atoi(line)
	}
}
