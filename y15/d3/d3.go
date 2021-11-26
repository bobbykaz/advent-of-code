package d3

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	fmt.Println()
	lines := utilities.ReadFileIntoLines("input/y15/d3.txt")
	line := lines[0]

	delivery := p2(line)
	max, multi, total := 0, 0, 0
	for k, v := range delivery {
		println(k, ":", v)
		if v > 1 {
			multi++
		}

		if v > max {
			max = v
		}
		total += v
	}

	fmt.Println("Max:", max, "; multi:", multi, "total houses", len(delivery), "total presents", total)

	return 0
}

func p1(dir string) map[string]int {
	x, y := 0, 0
	delivery := make(map[string]int)
	delivery["0:0"] = 1
	for i := 0; i < len(dir); i++ {
		switch dir[i] {
		case '<':
			x--
		case '>':
			x++
		case '^':
			y++
		case 'v':
			y--
		}
		coord := fmt.Sprintf("%d:%d", x, y)
		v, exists := delivery[coord]
		if exists {
			delivery[coord] = v + 1
		} else {
			delivery[coord] = 1
		}
	}
	return delivery
}

func p2(dir string) map[string]int {
	x, y, rx, ry := 0, 0, 0, 0
	isP2 := false
	delivery := make(map[string]int)
	delivery["0:0"] = 1
	for i := 0; i < len(dir); i++ {
		var coord string
		if isP2 {
			switch dir[i] {
			case '<':
				x--
			case '>':
				x++
			case '^':
				y++
			case 'v':
				y--
			}
			coord = fmt.Sprintf("%d:%d", x, y)
		} else {
			switch dir[i] {
			case '<':
				rx--
			case '>':
				rx++
			case '^':
				ry++
			case 'v':
				ry--
			}
			coord = fmt.Sprintf("%d:%d", rx, ry)
		}
		isP2 = !isP2
		v, exists := delivery[coord]
		if exists {
			delivery[coord] = v + 1
		} else {
			delivery[coord] = 1
		}
	}
	return delivery
}
