package d9

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d9.txt")
	grid := utilities.StringsToNGrid(lines)
	risk := 0
	basins := make([]int, 0)
	for row := 0; row < len(grid.G); row++ {
		for col := 0; col < len(grid.G[row]); col++ {
			if isLocalMin(&grid, row, col) {
				r := (1 + grid.G[row][col])
				b := findBasinSize(&grid, row, col)
				fmt.Println("Low point", row, col, "; risk", r, "; basin size", b)
				risk += r
				basins = append(basins, b)
			}
		}
	}

	sort.Ints(basins)
	fmt.Println("Risk Sum", risk)
	fmt.Println("Basins:", basins)
	fmt.Println("Biggest basins", basins[len(basins)-1], basins[len(basins)-2], basins[len(basins)-3])
	fmt.Println("Basin Product", basins[len(basins)-1]*basins[len(basins)-2]*basins[len(basins)-3])

	return 0
}

type cell struct {
	V int
	R int
	C int
}

func findNeighbors(g *utilities.NGrid, row, col int) []cell {
	result := make([]cell, 0)
	//top
	if (row - 1) >= 0 {
		result = append(result, cell{V: g.G[row-1][col], R: row - 1, C: col})
	}
	//left
	if (col - 1) >= 0 {
		result = append(result, cell{V: g.G[row][col-1], R: row, C: col - 1})
	}
	//right
	if (col + 1) < len(g.G[row]) {
		result = append(result, cell{V: g.G[row][col+1], R: row, C: col + 1})
	}
	//bottom
	if (row + 1) < len(g.G) {
		result = append(result, cell{V: g.G[row+1][col], R: row + 1, C: col})
	}

	return result
}

func isLocalMin(g *utilities.NGrid, row, col int) bool {
	adj := findNeighbors(g, row, col)
	for i := 0; i < len(adj); i++ {
		if !(g.G[row][col] < adj[i].V) {
			return false
		}
	}

	return true
}

//input MUST already be a low point
func findBasinSize(g *utilities.NGrid, row, col int) int {
	fmt.Println("Checking basin", row, col)
	seen := make(map[string]bool, 0)
	next := make([]cell, 0)
	next = append(next, cell{V: g.G[row][col], R: row, C: col})
	seen[key(row, col)] = true

	basinSize := 1
	for len(next) > 0 {
		current := next[0]
		next = next[1:]
		cn := findNeighbors(g, current.R, current.C)
		for i := 0; i < len(cn); i++ {
			_, exists := seen[key(cn[i].R, cn[i].C)]
			if !exists && cn[i].V > current.V && cn[i].V != 9 {
				seen[key(cn[i].R, cn[i].C)] = true
				next = append(next, cn[i])
				basinSize++
			}
		}
	}
	return basinSize
}

func key(r, c int) string {
	return fmt.Sprintf("%d-%d", r, c)
}
