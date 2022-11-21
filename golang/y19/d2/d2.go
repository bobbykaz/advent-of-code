package d2

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
	"github.com/bobbykaz/advent-of-code/y19/common"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19/d2.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	intcodes[1] = 12
	intcodes[2] = 2
	prog := common.IntcodeProgram{Program: intcodes}
	out := prog.Run(true)
	fmt.Println("result", out)
	return 0
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19/d2.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	for noun := 0; noun < 100; noun++ {
		for verb := 0; verb < 100; verb++ {
			intcodes = utilities.StringToInts(input[0])
			intcodes[1] = noun
			intcodes[2] = verb
			prog := common.IntcodeProgram{Program: intcodes}
			out := prog.Run(false)
			if out == 19690720 {
				fmt.Println("noun", noun, "verb", verb, "generated result", out)
				return 1
			}
		}
		fmt.Println("Exhausted Noun", noun)
	}
	return 0
}
