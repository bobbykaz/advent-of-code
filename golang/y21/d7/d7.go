package d7

import (
	"fmt"
	"math"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d7.txt")
	crabs := utilities.StringToInts(lines[0])
	fmt.Println(part1(crabs))
	return part2(crabs)
}

func part1(crabs []int) int {
	min, max := utilities.FindMinMax(crabs)
	fmt.Println("Bounds", min, max)
	minFuel := math.MaxInt32
	for t := min; t <= max; t++ {
		fuel := 0
		for i := 0; i < len(crabs); i++ {
			fuel += int(math.Abs(float64(crabs[i]) - float64(t)))
		}
		if fuel < minFuel {
			fmt.Println("New Min fuel at pos", t, "value ", fuel)
			minFuel = fuel
		}
	}
	fmt.Println("Part 1:", minFuel)
	return minFuel
}

func part2(crabs []int) int {
	min, max := utilities.FindMinMax(crabs)
	fmt.Println("Bounds", min, max)
	minFuel := math.MaxInt32
	for t := min; t <= max; t++ {
		fuel := 0
		for i := 0; i < len(crabs); i++ {
			n := int(math.Abs(float64(crabs[i]) - float64(t)))
			fuel += ((n * (n + 1)) / 2)
		}
		if fuel < minFuel {
			fmt.Println("New Min fuel at pos", t, "value ", fuel)
			minFuel = fuel
		}
	}
	fmt.Println("Part 2:", minFuel)
	return minFuel
}
