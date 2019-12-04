package d1

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19d1.txt")
	modules := utilities.StringsToInts(input)
	mass := 0
	for i := 0; i < len(modules); i++ {
		module := modules[i]
		fuel := (module / 3) - 2
		mass += module + fuel
	}

	fmt.Println("Final basic fuel mass: ", mass)
	return mass
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y19d1.txt")
	modules := utilities.StringsToInts(input)
	mass := 0
	for i := 0; i < len(modules); i++ {
		currentMass := modules[i]
		for fuel := (currentMass / 3) - 2; fuel >= 0; fuel = (currentMass / 3) - 2 {
			mass += fuel
			currentMass = fuel
		}
	}

	fmt.Println("Final cumulative fuel mass: ", mass)
	return mass
}
