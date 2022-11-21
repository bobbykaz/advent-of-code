package d13

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d13.txt")
	coords := make([]string, 0)
	instructions := make([]string, 0)
	ins := false
	for i := 0; i < len(lines); i++ {
		str := lines[i]
		if str == "" {
			ins = true
			i++
			str = lines[i]
		}
		if ins {
			instructions = append(instructions, str)
		} else {
			coords = append(coords, str)
		}
	}
	fmt.Println(instructions)
	g := makeGrid(coords)
	grid := Grid{G: g}

	for _, s := range instructions {
		fmt.Println(s)
		pts := strings.Split(s, "=")
		fmt.Println(pts)
		val, _ := strconv.Atoi(pts[1])
		switch pts[0][len(pts[0])-1] {
		case 'x':
			grid.FlipGridX(val)
		case 'y':
			grid.FlipGridY(val)
		default:
			panic("error parsing instruction")
		}
	}
	total := 0
	for r, _ := range grid.G {
		for c, _ := range grid.G[r] {
			char := "."
			if grid.G[r][c] > 0 {
				char = "#"
				total += 1
			}
			if c < 60 && r < 10 {
				fmt.Print(char)
			}
		}
		if r < 10 {
			fmt.Println()
		}
	}

	return total
}

type Grid struct {
	G [][]int
}

func (g *Grid) FlipGridY(y int) {
	for c := range g.G[0] {
		for i := y + 1; i < len(g.G); i++ {
			diff := i - y
			if y-diff < 0 {
				break
			}
			g.G[y-diff][c] += g.G[i][c]
			g.G[i][c] = 0
		}
	}
}

func (g *Grid) FlipGridX(x int) {
	for r := range g.G {
		for j := x + 1; j < len(g.G[r]); j++ {
			diff := j - x
			if x-diff < 0 {
				break
			}
			g.G[r][x-diff] += g.G[r][j]
			g.G[r][j] = 0
		}
	}
}

func makeGrid(coords []string) [][]int {
	mX, mY := 0, 0
	for _, s := range coords {
		pts := strings.Split(s, ",")
		x, _ := strconv.Atoi(pts[0])
		y, _ := strconv.Atoi(pts[1])
		if x > mX {
			mX = x
		}
		if y > mY {
			mY = y
		}
	}

	grid := make([][]int, mY+1)
	for i, _ := range grid {
		grid[i] = make([]int, mX+1)
	}

	for _, s := range coords {
		pts := strings.Split(s, ",")
		x, _ := strconv.Atoi(pts[0])
		y, _ := strconv.Atoi(pts[1])

		grid[y][x] = grid[y][x] + 1
	}

	return grid
}
