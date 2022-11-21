package d11

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d11.txt")
	octos := utilities.StringsToNGrid(lines)
	flashes := 0
	for i := 0; true; i++ {
		if i%100 == 0 {
			fmt.Println("Flashes after", i, "steps:", flashes)
		}
		flashesThisStep := processStep(&octos)
		flashes += flashesThisStep
		if flashesThisStep == 100 {
			fmt.Println("All octos flashed on step", i+1)
			return flashes
		}
	}

	return flashes
}

func processStep(g *utilities.NGrid) int {
	flashed := make(map[string]bool, 0)
	for row := 0; row < len(g.G); row++ {
		for col := 0; col < len(g.G[row]); col++ {
			processOcto(g, &flashed, row, col)
		}
	}
	flashCount := 0
	for row := 0; row < len(g.G); row++ {
		for col := 0; col < len(g.G[row]); col++ {
			if g.G[row][col] > 9 {
				g.G[row][col] = 0
				flashCount++
			}
		}
	}
	return flashCount
}

func processOcto(g *utilities.NGrid, flashed *map[string]bool, row int, col int) {
	g.G[row][col] = g.G[row][col] + 1
	_, alreadyFlashed := (*flashed)[key(row, col)]
	if g.G[row][col] > 9 && !alreadyFlashed {
		(*flashed)[key(row, col)] = true
		adj := findNeighbors(g, row, col)
		for i := 0; i < len(adj); i++ {
			processOcto(g, flashed, adj[i].R, adj[i].C)
		}

	}
}

type cell struct {
	R int
	C int
}

func findNeighbors(g *utilities.NGrid, row, col int) []cell {
	result := make([]cell, 0)
	for r := row - 1; r <= row+1; r++ {
		for c := col - 1; c <= col+1; c++ {
			if !(r == row && c == col) {
				if r >= 0 && r < g.Height && c >= 0 && c < g.Width {
					result = append(result, cell{R: r, C: c})
				}
			}
		}
	}
	return result
}

func key(r, c int) string {
	return fmt.Sprintf("%d-%d", r, c)
}
