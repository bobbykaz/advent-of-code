package d2

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d2.txt")

	p1 := part1(lines)
	fmt.Println("Part1:", p1)
	p2 := part2(lines)
	fmt.Println("Part2:", p2)
	return p1
}

func part1(lines []string) int {
	x := 0
	d := 0
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], " ")
		val, _ := strconv.Atoi(pts[1])
		switch pts[0] {
		case "forward":
			x += val
		case "up":
			d -= val
		case "down":
			d += val
		}
	}
	println("x:", x, "d:", d, "product:", x*d)
	return x * d
}

func part2(lines []string) int {
	x := 0
	d := 0
	a := 0
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], " ")
		val, _ := strconv.Atoi(pts[1])
		switch pts[0] {
		case "forward":
			x += val
			d += val * a
		case "up":
			a -= val
		case "down":
			a += val
		}
	}
	println("pt 2 x:", x, "d:", d, "product:", x*d)
	return x * d
}
