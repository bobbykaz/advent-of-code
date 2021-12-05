package d5

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d5.txt")
	//numbers := utilities.StringsToInts(lines)
	grid := make([][]int, 1000)
	for i := 0; i < len(grid); i++ {
		grid[i] = make([]int, 1000)
	}
	includeDiag := true
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], " -> ")
		startX, startY, err := utilities.ParseCoord(pts[0], "", ",", "")
		if err != nil {
			fmt.Println("err parsing start coord", err, pts[0], "i", i)
		}

		endX, endY, err := utilities.ParseCoord(pts[1], "", ",", "")
		if err != nil {
			fmt.Println("err parsing end coord", err, pts[1], "i", i)
		}

		if startX == endX || endY == startY {
			//fmt.Println("incrementing line ", startX, startY, "->", endX, endY)
			minX, maxX := utilities.FindMinMax([]int{startX, endX})
			minY, maxY := utilities.FindMinMax([]int{startY, endY})
			for x := minX; x <= maxX; x++ {
				for y := minY; y <= maxY; y++ {
					grid[y][x] = grid[y][x] + 1
				}
			}
		} else if includeDiag {
			fmt.Println("incrementing diag line ", startX, startY, "->", endX, endY)
			xDir := 1
			if endX < startX {
				xDir = -1
			}
			yDir := 1
			if endY < startY {
				yDir = -1
			}
			for i := 0; i <= (endX-startX)*xDir; i++ {
				eX := startX + i*xDir
				eY := startY + i*yDir
				grid[eY][eX] = grid[eY][eX] + 1
			}
		}
	}
	count := 0
	for x := 0; x < len(grid); x++ {
		//fmt.Println(grid[x])
		for y := 0; y < len(grid[x]); y++ {
			if grid[x][y] > 1 {
				count++
			}
		}
	}
	if !includeDiag {
		fmt.Println("Part1:", count)
	} else {
		fmt.Println("Part2:", count)
	}
	return count
}

func part2(lines []string) int {

	return 0
}
