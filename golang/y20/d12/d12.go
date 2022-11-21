package d12

import (
	"fmt"
	"math"
	"strconv"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d12.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	r1 := p1(input)
	fmt.Println("Part 1", r1)
	r2 := p2(input)
	fmt.Println("Part 2", r2)
	return -1
}

func p1(input []string) int {
	r, c, d := 0, 0, 90
	for _, s := range input {
		r, c, d = parseStr(s, r, c, d)
		fmt.Printf("%s -> (%d,%d)@%d\n", s, r, c, d)
	}

	return int(math.Abs(float64(r)) + math.Abs(float64(c)))
}

func parseStr(str string, r int, c int, dir int) (int, int, int) {
	char := rune(str[0])
	n, _ := strconv.Atoi(str[1:])
	switch char {
	case 'N':
		return r - n, c, dir
	case 'S':
		return r + n, c, dir
	case 'E':
		return r, c + n, dir
	case 'W':
		return r, c - n, dir
	case 'L':
		return r, c, (dir - n) % 360
	case 'R':
		return r, c, (dir + n) % 360
	case 'F':
		return nav(r, c, dir, n)
	}
	panic("uh oh")
}

func nav(r, c, d, n int) (int, int, int) {
	if d < 0 {
		d = 360 + d
	}

	switch d {
	case 0:
		return r - n, c, d
	case 90:
		return r, c + n, d
	case 180:
		return r + n, c, d
	case 270:
		return r, c - n, d
	}
	panic(fmt.Sprintf("uhoh2: %d %d %d %d", r, c, d, n))
}

func p2(input []string) int {
	sr, sc := 0, 0
	wr, wc := -1, 10
	for _, s := range input {
		sr, sc, wr, wc = parseStr2(s, sr, sc, wr, wc)
		fmt.Printf("%s -> (%d,%d) W(%d, %d)\n", s, sr, sc, wr, wc)
	}

	return int(math.Abs(float64(sr)) + math.Abs(float64(sc)))
}

func parseStr2(str string, sr, sc, wr, wc int) (int, int, int, int) {
	char := rune(str[0])
	n, _ := strconv.Atoi(str[1:])
	switch char {
	case 'N':
		return sr, sc, wr - n, wc
	case 'S':
		return sr, sc, wr + n, wc
	case 'E':
		return sr, sc, wr, wc + n
	case 'W':
		return sr, sc, wr, wc - n
	case 'L':
		wr, wc = rotate(wr, wc, -n)
		return sr, sc, wr, wc
	case 'R':
		wr, wc = rotate(wr, wc, n)
		return sr, sc, wr, wc
	case 'F':
		return nav2(sr, sc, wr, wc, n)
	}
	panic("uh oh")
}

func rotate(wr, wc, dir int) (int, int) {
	if dir < 0 {
		dir = 360 + dir
	}

	switch dir {
	case 0:
		return wr, wc
	case 90:
		return wc, -wr
	case 180:
		return -wr, -wc
	case 270:
		return -wc, wr
	}
	panic("uhoh3")
}

func nav2(sr, sc, wr, wc, n int) (int, int, int, int) {
	return sr + n*wr, sc + n*wc, wr, wc
}
