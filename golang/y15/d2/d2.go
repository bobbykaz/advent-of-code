package d2

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	fmt.Println()
	lines := utilities.ReadFileIntoLines("input/y15/d2.txt")
	papersum := 0
	ribbonsum := 0
	for i := 0; i < len(lines); i++ {
		line := lines[i]
		papersum += processLine(line)
		ribbonsum += processLineRibbon(line)
	}
	fmt.Println("total sqft: ", papersum)
	fmt.Println("total ribbon: ", ribbonsum)
	return papersum
}

//sq ft wrapping paper for gift
func processLine(line string) int {
	pts := utilities.Split(line, "x", "x") // should split into 3 pts
	dimensions := utilities.StringsToInts(pts)
	l, w, h := dimensions[0], dimensions[1], dimensions[2]

	sqft := (l*w + w*h + h*l) * 2
	extra, _ := utilities.FindMinMax([]int{l * w, w * h, h * l})
	return sqft + extra
}

//length of ribbon needed
func processLineRibbon(line string) int {
	pts := utilities.Split(line, "x", "x") // should split into 3 pts
	dimensions := utilities.StringsToInts(pts)
	l, w, h := dimensions[0], dimensions[1], dimensions[2]

	vol := (l * w * h)
	extra, _ := utilities.FindMinMax([]int{l + w, w + h, h + l})
	return vol + (extra * 2)
}
