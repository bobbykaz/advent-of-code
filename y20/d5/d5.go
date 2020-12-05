package d5

import (
	"fmt"
	"math"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20d5.txt")

	max := 0
	for _, s := range input {
		id := SeatID(s)
		if id > max {
			max = id
		}
	}

	return max
}

func SeatID(str string) int {
	rowID := 0
	for i := 0; i < 7; i++ {
		pw := int(math.Pow(2, float64(6-i)))
		switch rune(str[i]) {
		case 'F':
			rowID += 0
		case 'B':
			rowID += pw
		default:
			panic("uh oh")
		}
	}
	col := 0
	for i := 0; i < 3; i++ {
		pw := int(math.Pow(2, float64(2-i)))
		switch rune(str[7+i]) {
		case 'L':
			col += 0
		case 'R':
			col += pw
		default:
			panic("uh oh")
		}
	}

	seatID := rowID*8 + col
	//fmt.Println(str, "row", rowID, "col", col, "ID", seatID)
	return seatID
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20d5.txt")

	seats := make([]int, 0)
	for _, s := range input {
		id := SeatID(s)
		seats = append(seats, id)
	}

	sort.Ints(seats)

	for i := 1; i < len(seats); i++ {
		if seats[i]-seats[i-1] != 1 {
			fmt.Println("Missing seat between", seats[i], "and", seats[i-1])
			return seats[i-1] + 1
		}
	}

	return -1
}
