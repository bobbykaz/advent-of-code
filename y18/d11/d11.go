package d11

import "fmt"

func Part1() {
	powerGrid(18)
	powerGrid(9110)
}
func powerGrid(serial int) {
	fmt.Println("finding max power of grid serial:", serial)
	grid := make([][]int, 302)
	//0 row, col ignored
	for y := 0; y < 302; y++ {
		grid[y] = make([]int, 302)
		for x := 0; x < 302; x++ {
			rackID := x + 10
			pl := rackID * y
			pl += serial
			pl = pl * rackID
			hundPart := (pl / 100) % 10
			grid[y][x] = hundPart - 5
		}
	}

	// part 1
	maxP := 0
	for y := 1; y < 299; y++ {
		for x := 1; x < 299; x++ {
			power := grid[y][x] + grid[y+1][x] + grid[y+2][x] +
				grid[y][x+1] + grid[y+1][x+1] + grid[y+2][x+1] +
				grid[y][x+2] + grid[y+1][x+2] + grid[y+2][x+2]

			if power > maxP {
				maxP = power
				fmt.Println("new max:", maxP, "at", x, ",", y)
			}
		}
	}

	//convert to partial sum table
	for y := 1; y < 301; y++ {
		for x := 1; x < 301; x++ {
			up := grid[y-1][x]
			left := grid[y][x-1]
			corner := grid[y-1][x-1]
			grid[y][x] = grid[y][x] + up + left - corner
		}
	}
	maxP = 0
	for len := 1; len < 301; len++ {
		for y := 1; y < 301-len; y++ {
			for x := 1; x < 301-len; x++ {
				targetSum := grid[y+len-1][x+len-1]

				up := grid[y-1][x+len-1]
				left := grid[y+len-1][x-1]
				corner := grid[y-1][x-1]
				power := targetSum - up - left + corner
				if power > maxP {
					maxP = power
					fmt.Println("new max:", maxP, "at", x, ",", y, ",", len)
				}
			}
		}
	}
}
