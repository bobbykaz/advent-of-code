package d11

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d11.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	fmt.Println("stable after", p1(input))
	p2(input)
	return -1
}

func p1(input []string) int {
	currentG := utilities.StringsToGrid(input)
	nextG := utilities.StringsToGrid(input)
	current := &currentG
	next := &nextG
	fmt.Println("same grids:", current.Equals(next, false))
	i := 0
	for true {
		print := i%5 == 0

		nextCycle(current, next, print)
		i++

		if print {
			fmt.Println(i, "done")
		}

		if current.Equals(next, print) {
			seatCount := 0
			for _, s := range current.G {
				for _, v := range s {
					if v == '#' {
						seatCount++
					}
				}
			}
			current.Print()
			fmt.Println("Occupied seats", seatCount)
			return i
		}

		tmp := current
		current = next
		next = tmp
	}
	return -1
}

func nextCycle(last, next *utilities.Grid, print bool) {
	diff := 0
	for i := 0; i < next.Height; i++ {
		for j := 0; j < next.Width; j++ {
			next.G[i][j] = fillSeat(last, i, j)
			if next.G[i][j] != last.G[i][j] {
				diff++
			}
		}
	}

	if print {
		fmt.Printf("Altered: %d - ", diff)
	}
}

func fillSeat(last *utilities.Grid, i int, j int) rune {
	c := last.G[i][j]
	if c == '.' {
		return '.'
	}

	occupied := 0
	adj := last.Adjacent(i, j)

	for _, v := range adj {
		if v == '#' {
			occupied++
		}
	}

	if c == 'L' && occupied == 0 {
		return '#'
	}

	if c == '#' && occupied >= 4 {
		return 'L'
	}

	return c
}

func p2(input []string) {

}
