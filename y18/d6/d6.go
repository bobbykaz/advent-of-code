package d6

import (
	"fmt"
	"sort"
	"strconv"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input6.txt")
	sort.Strings(input)
	points := make([]point, 0, len(input))
	fmt.Println("Made ", len(input), " points")
	for i := 0; i < len(input); i++ {
		x, y := getCoord(input[i])
		pt := point{X: x, Y: y}
		points = append(points, pt)
	}

	//minX, minY := 0, 0
	maxX, maxY := points[0].X, points[0].Y

	for i := 0; i < len(points); i++ {
		if points[i].X > maxX {
			maxX = points[i].X
		}

		if points[i].Y > maxY {
			maxY = points[i].Y
		}
	}
	fmt.Println("Bounds ", maxX, ",", maxY)
	grid := make([][]int, maxY)
	fmt.Println("Setup grid...")
	for i := 0; i < maxY; i++ {
		grid[i] = make([]int, maxX)
	}

	//dangerousGrid(grid, points)
	safeGrid(grid, points)

}

func dangerousGrid(grid [][]int, points []point) {
	fmt.Println("Calculating distance to POIs...")
	grid = findClosestPoints(grid, points)

	fmt.Println("Totaling each value...")
	areas := make([]int, len(points))
	for i := 0; i < len(areas); i++ {
		areas[i] = 0
	}
	fmt.Println("Calculating areas")
	for i := 0; i < len(grid); i++ {
		for j := 0; j < len(grid[i]); j++ {
			if grid[i][j] != -1 {
				areas[grid[i][j]]++
			}
		}
	}

	//dont count any point where the edge of the grid is closest to that point - these are 'infinite'
	fmt.Println("ignoring areas on edge of the grid")
	//top and bottom
	for i := 0; i < len(grid[0]); i++ {
		if grid[0][i] != -1 {
			areas[grid[0][i]] = -1
		}
		if grid[(len(grid) - 1)][i] != -1 {
			areas[grid[len(grid)-1][i]] = -1
		}
	}

	//left and right
	for i := 0; i < len(grid); i++ {
		if grid[i][0] != -1 {
			areas[grid[i][0]] = -1
		}
		if grid[i][len(grid[i])-1] != -1 {
			areas[grid[i][len(grid[i])-1]] = -1
		}
	}

	fmt.Println("Areas: ")
	for i := 0; i < len(areas); i++ {
		fmt.Printf("%d: %d\n", i, areas[i])
	}
}

func safeGrid(grid [][]int, points []point) {
	findSafePoints(grid, points)
}

type point struct {
	X int
	Y int
}

func findClosestPoints(grid [][]int, pts []point) [][]int {
	for i := 0; i < len(grid); i++ {
		for j := 0; j < len(grid[i]); j++ {
			grid[i][j] = findClosestPoint(i, j, pts)
		}
	}
	return grid
}

func findSafePoints(grid [][]int, pts []point) [][]int {
	totalSafePoints := 0
	for i := 0; i < len(grid); i++ {
		for j := 0; j < len(grid[i]); j++ {
			if pointIsSafe(i, j, pts) {
				grid[i][j] = 1
				totalSafePoints++
			} else {
				grid[i][j] = 0
			}

		}
	}
	fmt.Println("Found safe points: ", totalSafePoints)
	return grid
}

func pointIsSafe(i int, j int, points []point) bool {
	totalDist := 0
	for p := 0; p < len(points); p++ {
		curdist := distBetweenPoints(point{X: j, Y: i}, points[p])
		totalDist += curdist
	}

	return totalDist < 10000
}

func findClosestPoint(i int, j int, points []point) int {
	minDist := distBetweenPoints(point{X: j, Y: i}, points[0])
	minPoint := 0
	secMinDist := minDist + 1
	for p := 1; p < len(points); p++ {
		curdist := distBetweenPoints(point{X: j, Y: i}, points[p])
		if curdist < minDist {
			minDist = curdist
			minPoint = p
		} else if curdist < secMinDist {
			secMinDist = curdist
		}
	}

	if minDist == secMinDist {
		return -1 // tie for max distance
	} else {
		return minPoint
	}
}

func getCoord(line string) (int, int) {
	first := strings.Split(line, ", ")
	//
	left, _ := strconv.Atoi(first[0])
	right, _ := strconv.Atoi(first[1])
	return left, right
}

func distBetweenPoints(a, b point) int {
	xDist := a.X - b.X
	yDist := a.Y - b.Y
	if xDist < 0 {
		xDist *= -1
	}

	if yDist < 0 {
		yDist *= -1
	}

	return xDist + yDist
}
