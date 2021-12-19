package d15

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d15.txt")
	g := utilities.StringsToNGrid(lines)
	g = get5xGrid(g)
	return calcRisk(g)
}

func get5xGrid(g utilities.NGrid) utilities.NGrid {
	grids := make([][]utilities.NGrid, 5)
	for i := 0; i < 5; i++ {
		grids[i] = make([]utilities.NGrid, 5)
		for j := 0; j < 5; j++ {
			grids[i][j] = copyNGrid(g, i+j)
		}
	}

	return utilities.GetCompositeNGrid(grids, false)
}

func copyNGrid(g utilities.NGrid, modifier int) utilities.NGrid {
	newG := utilities.NGrid{G: nil, Height: g.Height, Width: g.Width}
	newG.G = make([][]int, g.Height)
	for i := 0; i < g.Height; i++ {
		newG.G[i] = make([]int, g.Width)
		for j := 0; j < g.Width; j++ {
			v := g.G[i][j] + modifier
			if v > 9 {
				v = (v % 10) + 1
			}
			newG.G[i][j] = v
		}
	}
	return newG
}

func calcRisk(g utilities.NGrid) int {
	eR, eC := g.Height-1, g.Width-1
	seen := make(map[string]int)
	seen[key(0, 0)] = 0
	greedyList := findLowestRiskNeighbors(&g, riskSet{0, 0, 0}, &seen)
	greedyList = sortRiskSet(greedyList)
	for len(greedyList) > 0 {
		next := greedyList[0]
		fmt.Println(next)
		if next.R == eR && next.C == eC {
			return next.TotalRisk
		}
		greedyList = greedyList[1:]
		newNeighbors := findLowestRiskNeighbors(&g, next, &seen)
		greedyList = append(greedyList, newNeighbors...)
		evaluableRisk := next.TotalRisk
		for len(greedyList) > 0 && greedyList[0].TotalRisk == evaluableRisk {
			fmt.Println(next)
			next = greedyList[0]
			if next.R == eR && next.C == eC {
				return next.TotalRisk
			}
			greedyList = greedyList[1:]
			newNeighbors = findLowestRiskNeighbors(&g, next, &seen)
			greedyList = append(greedyList, newNeighbors...)
		}
		greedyList = sortRiskSet(greedyList)
	}
	fmt.Println("uhhhh we ran out of items?")
	return 0
}

func sortRiskSet(r []riskSet) []riskSet {
	sort.Slice(r, func(i, j int) bool { return r[i].TotalRisk < r[j].TotalRisk })
	return r
}

type riskSet struct {
	R         int
	C         int
	TotalRisk int
}

func findLowestRiskNeighbors(g *utilities.NGrid, curr riskSet, seen *map[string]int) []riskSet {
	result := make([]riskSet, 0)
	if curr.TotalRisk > (*seen)[key(curr.R, curr.C)] {
		return result
	}
	row := curr.R
	col := curr.C
	risk := curr.TotalRisk
	//top
	if (row - 1) >= 0 {
		r, c := row-1, col
		newRisk := risk + g.G[r][c]
		ltr, exists := (*seen)[key(r, c)]
		if !exists || newRisk < ltr {
			(*seen)[key(r, c)] = newRisk
			result = append(result, riskSet{TotalRisk: newRisk, R: r, C: c})
		}
	}
	//left
	if (col - 1) >= 0 {
		r, c := row, col-1
		newRisk := risk + g.G[r][c]
		ltr, exists := (*seen)[key(r, c)]
		if !exists || newRisk < ltr {
			(*seen)[key(r, c)] = newRisk
			result = append(result, riskSet{TotalRisk: newRisk, R: r, C: c})
		}
	}
	//right
	if (col + 1) < len(g.G[row]) {
		r, c := row, col+1
		newRisk := risk + g.G[r][c]
		ltr, exists := (*seen)[key(r, c)]
		if !exists || newRisk < ltr {
			(*seen)[key(r, c)] = newRisk
			result = append(result, riskSet{TotalRisk: newRisk, R: r, C: c})
		}
	}
	//bottom
	if (row + 1) < len(g.G) {
		r, c := row+1, col
		newRisk := risk + g.G[r][c]
		ltr, exists := (*seen)[key(r, c)]
		if !exists || newRisk < ltr {
			(*seen)[key(r, c)] = newRisk
			result = append(result, riskSet{TotalRisk: newRisk, R: r, C: c})
		}
	}

	return result
}

func key(a, b int) string {
	return fmt.Sprintf("%d-%d", a, b)
}
