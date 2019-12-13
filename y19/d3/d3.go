package d3

import (
	"fmt"
	"math"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y19d3.txt")
	print := false
	wire1 := strings.Split(input[0], ",")
	wire2 := strings.Split(input[1], ",")
	circuit := make(map[string]wirePart)
	x, y, total := 0, 0, 0
	for _, s := range wire1 {
		x, y, total = processSegment(circuit, s, 1, x, y, total, print)
	}
	fmt.Println("wire 1 done.")
	x, y, total = 0, 0, 0
	for _, s := range wire2 {
		x, y, total = processSegment(circuit, s, 2, x, y, total, print)
	}
	fmt.Println("wire 2 done.")

	return 0
}

// Returns final x,y,total
func processSegment(circuit map[string]wirePart, part string, wireNumber int, x int, y int, total int, print bool) (int, int, int) {
	dir := part[0]
	amt, _ := strconv.Atoi(part[1:])
	for i := 0; i < amt; i++ {
		switch dir {
		case 'L':
			x--
			break
		case 'R':
			x++
			break
		case 'D':
			y--
			break
		case 'U':
			y++
			break
		}

		key := coordKey(x, y)
		_, keyExists := circuit[key]
		total++
		if keyExists {
			currentPart := circuit[key]
			if currentPart.WireNumber != wireNumber {
				currentPart.Xs++
				circuit[key] = currentPart
				fmt.Println("...Intersection at ", x, ",", y,
					"; manhattan:", (int(math.Abs(float64(x))) + int(math.Abs(float64(y)))),
					"; old length:", currentPart.Length, " current: ", (total),
					"combo:", currentPart.Length+total)
			}
		} else {
			circuit[key] = wirePart{Xs: 0, WireNumber: wireNumber, Length: (total)}
		}
	}
	if print {
		fmt.Println("Part ", part, "processed to (", x, ",", y, ") for total", (total))
	}
	return x, y, total
}

func coordKey(x int, y int) string {
	return fmt.Sprintf("%d-%d", x, y)
}

type wirePart struct {
	Length     int
	WireNumber int
	Xs         int
}
