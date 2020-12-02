package d5

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
	"github.com/bobbykaz/advent-of-code/y19/common"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19d5.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	prog := common.IntcodeProgram{Program: intcodes, Input: []int{1}}
	out := prog.Run(true)
	fmt.Println("result", out)
	return out
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19d5.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	prog := common.IntcodeProgram{Program: intcodes, Input: []int{5}}
	out := prog.Run(true)
	fmt.Println("result", out)
	return out
	return 0
}
