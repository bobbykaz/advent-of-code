package d13

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d13.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	//p1(input)
	p2(input[1])
	return -1
}

func p1(input []string) {
	baseTime, _ := strconv.Atoi(input[0])
	buses := strings.Split(input[1], ",")
	busIDs := make([]int, 0)
	for _, v := range buses {
		if v != "x" {
			id, _ := strconv.Atoi(v)
			busIDs = append(busIDs, id)
		}
	}
	minDiff, minID := baseTime, -1
	for _, v := range busIDs {
		diff := 0
		for diff < baseTime {
			diff += v
		}
		if (diff - baseTime) < minDiff {
			minDiff = (diff - baseTime)
			minID = v
		}
	}

	fmt.Println("Bus ID", minID, "is quickest with time", minDiff, ", product", minID*minDiff)

}

func p2(input string) int64 {
	buses := strings.Split(input, ",")
	busIDs := make([]int64, 0)
	for i, v := range buses {
		if v != "x" {
			id, _ := strconv.ParseInt(v, 10, 64)
			busIDs = append(busIDs, id)
			fmt.Println(id, "@", i)
		} else {
			busIDs = append(busIDs, int64(-1))
		}
	}

	t := int64(1)
	p := int64(1)
	for i, b := range busIDs {
		if b != -1 {
			i64 := int64(i)
			for t%b != 0 {
				fmt.Println("bus", b, "doesnt run at", t, "jumping to", b*t)
				t += p
			}

			fmt.Println("bus", b, "runs at t:", t)
			p *= b

			if busesMeetTimetable(t-i64, busIDs) {
				fmt.Println("Found!", t-i64)
				return t - i64
			}
		}
		t++
	}

	fmt.Println("Uh oh")
	return t
}

func busesMeetTimetable(t int64, buses []int64) bool {
	for i := 0; i < len(buses); i++ {
		if buses[i] != -1 {
			if (t+int64(i))%buses[i] != 0 {
				return false
			}
		}
	}

	return true
}
