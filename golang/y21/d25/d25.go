package d25

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d25.txt")
	g := utilities.StringsToGrid(lines)
	keepGoing := true
	step := 0

	fmt.Println("START", g.Height, g.Width)
	g.Print()
	fmt.Println("=========================")

	for keepGoing {
		keepGoing = false
		step++
		toMove := make([]move, 0)
		for r := 0; r < g.Height; r++ {
			for c := 0; c < g.Width; c++ {
				nextCol := c + 1
				if nextCol == g.Width {
					nextCol = 0
				}
				if g.G[r][c] == '>' && g.G[r][nextCol] == '.' {
					keepGoing = true
					toMove = append(toMove, move{pos{r, c}, pos{r, nextCol}})
				}
			}
		}

		for _, m := range toMove {
			g.G[m.from.r][m.from.c] = '.'
			g.G[m.to.r][m.to.c] = '>'
		}

		toMove = make([]move, 0)
		for r := 0; r < g.Height; r++ {
			nextRow := (r + 1)
			if nextRow == g.Height {
				nextRow = 0
			}
			for c := 0; c < g.Width; c++ {
				if g.G[r][c] == 'v' && g.G[nextRow][c] == '.' {
					keepGoing = true
					toMove = append(toMove, move{pos{r, c}, pos{nextRow, c}})
				}
			}
		}

		for _, m := range toMove {
			g.G[m.from.r][m.from.c] = '.'
			g.G[m.to.r][m.to.c] = 'v'
		}

		//g.Print()
		//fmt.Println("=========================")
	}
	g.Print()
	return step
}

type move struct {
	from pos
	to   pos
}

type pos struct {
	r int
	c int
}
