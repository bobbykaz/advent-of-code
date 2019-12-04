package d2

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19d2.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	out := runProgramWithInput(intcodes, 12, 2, true)
	fmt.Println("result", out)
	return 0
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19d2.txt")
	intcodes := utilities.StringToInts(input[0])
	fmt.Println("total intcodes found", len(intcodes))
	for noun := 0; noun < 100; noun++ {
		for verb := 0; verb < 100; verb++ {
			intcodes = utilities.StringToInts(input[0])
			out := runProgramWithInput(intcodes, noun, verb, false)
			if out == 19690720 {
				fmt.Println("noun", noun, "verb", verb, "generated result", out)
				return 1
			}
		}
		fmt.Println("Exhausted Noun", noun)
	}
	return 0
}

func runProgramWithInput(intcodes []int, noun int, verb int, print bool) int {
	intcodes[1] = noun
	intcodes[2] = verb
	for i := 0; i < len(intcodes); i += 4 {
		opCode := intcodes[i]
		i1 := intcodes[i+1]
		i2 := intcodes[i+2]
		out := intcodes[i+3]
		switch opCode {
		case 1:
			intcodes[out] = intcodes[i1] + intcodes[i2]
			break
		case 2:
			intcodes[out] = intcodes[i1] * intcodes[i2]
			break
		case 99:
			if print {
				fmt.Println("HALT:")
				printProgram(intcodes)
			}
			return intcodes[0]
		default:
			fmt.Println("Error at pos", i, "opcode", opCode)
		}
	}
	return -1
}

func printProgram(prog []int) {
	for i := 0; i < len(prog)-4; i += 4 {
		fmt.Println(prog[i], ",", prog[i+1], ",", prog[i+2], ",", prog[i+3])
	}
	fmt.Println("==============================")
}
