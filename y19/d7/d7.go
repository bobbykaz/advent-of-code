package d7

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
	"github.com/bobbykaz/advent-of-code/y19/common"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19/d7.txt")

	for a := 0; a < 5; a++ {
		acodes := utilities.StringToInts(input[0])
		aprog := common.IntcodeProgram{Program: acodes, Input: []int{a, 0}}
		aprog.Run(false)
		for b := 0; b < 5; b++ {
			bcodes := utilities.StringToInts(input[0])
			bprog := common.IntcodeProgram{Program: bcodes, Input: append([]int{b}, aprog.Output...)}
			bprog.Run(false)
			for c := 0; c < 5; c++ {
				ccodes := utilities.StringToInts(input[0])
				cprog := common.IntcodeProgram{Program: ccodes, Input: append([]int{c}, bprog.Output...)}
				cprog.Run(false)
				for d := 0; d < 5; d++ {
					dcodes := utilities.StringToInts(input[0])
					dprog := common.IntcodeProgram{Program: dcodes, Input: append([]int{d}, cprog.Output...)}
					dprog.Run(false)
					for e := 0; e < 5; e++ {
						ecodes := utilities.StringToInts(input[0])
						eprog := common.IntcodeProgram{Program: ecodes, Input: append([]int{e}, dprog.Output...)}
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

func FeedbackAmplify(a, b, c, d, e int, input string) int {
	finalOutput := 0
	iter := 0
	acodes := utilities.StringToInts(input)
	aprog := common.IntcodeProgram{Program: acodes, Input: []int{a}, ReturnOutput: true, Resume: true}
	bcodes := utilities.StringToInts(input)
	bprog := common.IntcodeProgram{Program: bcodes, Input: []int{b}, ReturnOutput: true, Resume: true}
	ccodes := utilities.StringToInts(input)
	cprog := common.IntcodeProgram{Program: ccodes, Input: []int{c}, ReturnOutput: true, Resume: true}
	dcodes := utilities.StringToInts(input)
	dprog := common.IntcodeProgram{Program: dcodes, Input: []int{d}, ReturnOutput: true, Resume: true}
	ecodes := utilities.StringToInts(input)
	eprog := common.IntcodeProgram{Program: ecodes, Input: []int{e}, ReturnOutput: true, Resume: true}
	for true {
		//fmt.Printf("[%d,%d,%d,%d,%d]: iter:%d;\n %d -> Prog A\n", a, b, c, d, e, iter, finalOutput)
		aprog.Input = append(aprog.Input, finalOutput)
		aout := aprog.Run(false)
		if aprog.HALTED {
			return finalOutput
		}

		//fmt.Println(aout, "-> Prog B")
		bprog.Input = append(bprog.Input, aout)
		bout := bprog.Run(false)
		if bprog.HALTED {
			return finalOutput
		}

		//fmt.Println(bout, " -> Prog C")
		cprog.Input = append(cprog.Input, bout)
		cout := cprog.Run(false)
		if cprog.HALTED {
			return finalOutput
		}

		//fmt.Println(cout, "-> Prog D")
		dprog.Input = append(dprog.Input, cout)
		dout := dprog.Run(false)
		if dprog.HALTED {
			return finalOutput
		}

		//fmt.Println(dout, "-> Prog E")
		eprog.Input = append(eprog.Input, dout)
		eout := eprog.Run(false)
		if eprog.HALTED {
			return finalOutput
		}
		finalOutput = eout
		iter++
	}

	return finalOutput
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19/d7.txt")
	program := input[0]
	max := 0
	for a := 5; a < 10; a++ {
		for b := 5; b < 10; b++ {
			for c := 5; c < 10; c++ {
				for d := 5; d < 10; d++ {
					for e := 5; e < 10; e++ {
						if a != b && b != c && a != c && a != d && b != d && c != d && e != a && e != b && e != c && e != d {
							output := FeedbackAmplify(a, b, c, d, e, program)
							fmt.Println("Permutation", a, b, c, d, e, ": output :", output)
							if output > max {
								max = output
							}
						}
					}
				}
			}
		}
	}

	return max
}
