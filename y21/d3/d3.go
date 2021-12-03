package d3

import (
	"fmt"
	"strconv"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d3.txt")
	//numbers := utilities.StringsToInts(lines)

	p1 := part1(lines)
	fmt.Println("Part1:", p1)
	p2 := part2(lines)
	fmt.Println("Part2:", p2)
	return p1
}

func part1(lines []string) int {
	zeroes := make([]int, len(lines[0]))
	ones := make([]int, len(lines[0]))
	for i := 0; i < len(lines); i++ {
		current := lines[i]
		for j := 0; j < len(current); j++ {
			switch current[j] {
			case '0':
				zeroes[j] = zeroes[j] + 1
			case '1':
				ones[j] = ones[j] + 1
			}
		}
	}
	common := ""
	uncommon := ""
	for i := 0; i < len(zeroes); i++ {
		if zeroes[i] > ones[i] {
			common += "0"
			uncommon += "1"
		} else {
			common += "1"
			uncommon += "0"
		}
	}
	println(common)
	println(uncommon)
	gamma, _ := strconv.ParseInt(common, 2, 32)
	epsilon, _ := strconv.ParseInt(uncommon, 2, 32)
	fmt.Println("Gamma", gamma, "Epsilon", epsilon, "product", gamma*epsilon)
	return 0
}

func part2(lines []string) int {
	co2 := findCo2(lines)
	oxy := findOxy(lines)

	println("Oxygen", oxy, "Co2", co2, "life support", oxy*co2)
	return 0
}

func findOxy(lines []string) int64 {
	remaining := lines
	for i := 0; i < len(lines[0]); i++ {
		ones := 0
		zeroes := 0
		for k := 0; k < len(remaining); k++ {
			switch remaining[k][i] {
			case '0':
				zeroes++
			case '1':
				ones++
			}
		}
		if ones >= zeroes {
			remaining = removeInvalidEntries(remaining, i, '1')
		} else {
			remaining = removeInvalidEntries(remaining, i, '0')
		}

		if len(remaining) == 1 {
			break
		}
	}
	oxy, _ := strconv.ParseInt(remaining[0], 2, 32)
	return oxy
}

func findCo2(lines []string) int64 {
	remaining := lines
	for i := 0; i < len(lines[0]); i++ {
		ones := 0
		zeroes := 0
		for k := 0; k < len(remaining); k++ {
			switch remaining[k][i] {
			case '0':
				zeroes++
			case '1':
				ones++
			}
		}
		if ones < zeroes {
			remaining = removeInvalidEntries(remaining, i, '1')
		} else {
			remaining = removeInvalidEntries(remaining, i, '0')
		}

		if len(remaining) == 1 {
			break
		}
	}
	co2, _ := strconv.ParseInt(remaining[0], 2, 32)
	return co2
}

func removeInvalidEntries(items []string, bitPosition int, target byte) []string {
	results := make([]string, 0)
	for i := 0; i < len(items); i++ {
		if items[i][bitPosition] == target {
			results = append(results, items[i])
		}
	}

	return results
}
