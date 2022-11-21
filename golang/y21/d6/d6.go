package d6

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d6.txt")
	currentFish := utilities.StringToInts(lines[0])
	fmt.Println(currentFish)
	return part2(currentFish)
}

func naive(currentFish []int) int {
	for d := 0; d < 80; d++ {
		fmt.Println("Day", d)
		newFish := make([]int, 0)
		for i := 0; i < len(currentFish); i++ {
			if currentFish[i] == 0 {
				newFish = append(newFish, 8)
				currentFish[i] = 6
			} else {
				currentFish[i] = currentFish[i] - 1
			}
		}
		currentFish = append(currentFish, newFish...)
	}

	fmt.Println("Part 1:", len(currentFish))
	return len(currentFish)
}

func part2(currentFish []int) int {
	fishPerDay := make([]int, 10)
	for i := 0; i < len(currentFish); i++ {
		fishPerDay[currentFish[i]] = fishPerDay[currentFish[i]] + 1
	}

	for d := 0; d < 256; d++ {
		fmt.Println("Day", d, "Start", fishPerDay)
		today := fishPerDay[0]
		fishPerDay[9] += today
		fishPerDay[7] += today
		fishPerDay = fishPerDay[1:]
		fishPerDay = append(fishPerDay, 0)
		fmt.Println("Day", d, "end", fishPerDay)
	}

	total := 0
	for i := 0; i < len(fishPerDay); i++ {
		total += fishPerDay[i]
	}

	fmt.Println("Part 2", total)
	return total
}
