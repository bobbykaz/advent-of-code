package d9

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d9.txt"

func Run() int64 {
	input := utilities.ReadFileIntoLines(inputFile)
	ints := utilities.StringsToInts64(input)

	invalid := p1(ints)
	weakness := p2(ints, invalid)
	return weakness
}

func p1(ints []int64) int64 {
	sums := make([][]int64, len(ints))
	fmt.Println("Creating sums")
	for i := 0; i < len(ints); i++ {
		sums[i] = make([]int64, 0, 24)
		for k := 1; k < 25; k++ {
			if (i + k) < len(ints) {
				sums[i] = append(sums[i], (ints[i] + ints[i+k]))
			}
		}
	}
	fmt.Println("Scanning")

	for i := 25; i < len(ints); i++ {
		sumCheckStart := i - 25
		valid := false
		for sumCheck := i - 25; sumCheck < i; sumCheck++ {
			sumDiff := 25 - (sumCheck - sumCheckStart)
			for s, v := range sums[sumCheck] {
				if s < sumDiff {
					if ints[i] == v {
						valid = true
						sumCheck = i
						//fmt.Println("Code #", i, ":", ints[i], "is sum of", sumCheck, ",", (sumCheck + s + 1))
						break
					}
				}
			}
		}
		if !valid {
			fmt.Println("Code #", i, ":", ints[i], "is invalid")
			return ints[i]
		}
	}
	return -1
}

func p2(ints []int64, target int64) int64 {
	start, end := ints[0], ints[1]
	si, se := 0, 1
	sum := start + end
	for true {
		if sum == target {
			min, max := utilities.FindMinMax64(ints[si:se])
			fmt.Println("Sequence:", si, "->", se, "; ", start, "/", max, ";", (min + max))
			return (min + max)
		} else if sum < target {
			se++
			end = ints[se]
			sum += end
		} else { // sum > target
			si++
			sum -= start
			start = ints[si]
		}
	}
	return -1
}
