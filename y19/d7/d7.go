package d7

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
	"github.com/bobbykaz/advent-of-code/y19/common"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19d7.txt")

	for a := 0; a < 5; a++ {
		acodes := utilities.StringToInts(input[0])
		aprog := common.IntcodeProgram{Program: acodes, Input: []int{a, 0}}
		aprog.Run(false)
		for b := 0; b < 5; b++ {
			bcodes := utilities.StringToInts(input[0])
			bprog := common.IntcodeProgram{Program: bcodes, Input: []int{b, aprog.Output}}
			bprog.Run(false)
			for c := 0; c < 5; c++ {
				ccodes := utilities.StringToInts(input[0])
				cprog := common.IntcodeProgram{Program: ccodes, Input: []int{c, bprog.Output}}
				cprog.Run(false)
				for d := 0; d < 5; d++ {
					dcodes := utilities.StringToInts(input[0])
					dprog := common.IntcodeProgram{Program: dcodes, Input: []int{d, cprog.Output}}
					dprog.Run(false)
					for e := 0; e < 5; e++ {
						ecodes := utilities.StringToInts(input[0])
						eprog := common.IntcodeProgram{Program: ecodes, Input: []int{e, dprog.Output}}
						eprog.Run(false)
						if a != b && b != c && a != c && a != d && b != d && c != d && e != a && e != b && e != c && e != d {
							fmt.Println("Permutation", a, b, c, d, e, ": output :", eprog.Output)
						}
					}
				}
			}
		}
	}

	return -1
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19d7.txt")

	for a := 0; a < 5; a++ {
		acodes := utilities.StringToInts(input[0])
		aprog := common.IntcodeProgram{Program: acodes, Input: []int{a, 0}}
		aprog.Run(false)
		for b := 0; b < 5; b++ {
			bcodes := utilities.StringToInts(input[0])
			bprog := common.IntcodeProgram{Program: bcodes, Input: []int{b, aprog.Output}}
			bprog.Run(false)
			for c := 0; c < 5; c++ {
				ccodes := utilities.StringToInts(input[0])
				cprog := common.IntcodeProgram{Program: ccodes, Input: []int{c, bprog.Output}}
				cprog.Run(false)
				for d := 0; d < 5; d++ {
					dcodes := utilities.StringToInts(input[0])
					dprog := common.IntcodeProgram{Program: dcodes, Input: []int{d, cprog.Output}}
					dprog.Run(false)
					for e := 0; e < 5; e++ {
						ecodes := utilities.StringToInts(input[0])
						eprog := common.IntcodeProgram{Program: ecodes, Input: []int{e, dprog.Output}}
						eprog.Run(false)
						if a != b && b != c && a != c && a != d && b != d && c != d && e != a && e != b && e != c && e != d {
							fmt.Println("Permutation", a, b, c, d, e, ": output :", eprog.Output)
						}
					}
				}
			}
		}
	}

	return -1
}
