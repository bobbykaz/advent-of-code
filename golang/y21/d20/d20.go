package d20

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d20.txt")
	alg := lines[0]
	grid := utilities.StringsToGrid(lines[2:])

	gptr := &grid
	for i := 0; i < 50; i++ {
		pad := '.'
		if i%2 == 1 {
			pad = '#'
		}
		//gptr.Print()
		//fmt.Println("===================")
		gptr = step(gptr, alg, pad)
	}

	lit := 0
	for _, r := range gptr.G {
		for _, c := range r {
			if c == '#' {
				lit++
			}
		}
	}
	return lit
}

func step(g *utilities.Grid, alg string, padder rune) *utilities.Grid {
	ng := utilities.PadGrid(*g, padder, 1)
	copy := utilities.PadGrid(*g, padder, 1)

	for r := 0; r < ng.Height; r++ {
		for c := 0; c < ng.Width; c++ {
			//key
			i := lookupIndex(&copy, r, c, padder)
			ng.G[r][c] = rune(alg[i])
		}
	}

	return &ng
}

func lookupIndex(g *utilities.Grid, row, col int, assumed rune) int {
	key := ""
	for r := row - 1; r <= row+1; r++ {
		for c := col - 1; c <= col+1; c++ {
			if r < 0 || r >= g.Height || c < 0 || c >= g.Width {
				key = fmt.Sprintf("%s%c", key, assumed)
			} else {
				key = fmt.Sprintf("%s%c", key, g.G[r][c])
			}
		}
	}

	//fmt.Printf("(%d,%d) -> %s\n", row, col, key)

	key = strings.Replace(key, ".", "0", -1)
	key = strings.Replace(key, "#", "1", -1)
	bInt, _ := strconv.ParseInt(key, 2, 32)
	return int(bInt)
}
