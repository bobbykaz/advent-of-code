package d5

import (
	"fmt"
	"sort"
	"strconv"
	"strings"

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
	s := strings.ReplaceAll(str, "F", "0")
	s = strings.ReplaceAll(s, "B", "1")
	s = strings.ReplaceAll(s, "L", "0")
	s = strings.ReplaceAll(s, "R", "1")
	i, err := strconv.ParseInt(s, 2, 32)
	if err != nil {
		fmt.Println("error parsing", err)
		return -1
	}
	return int(i)
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
