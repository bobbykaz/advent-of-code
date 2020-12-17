package d15

import "fmt"

var inputFile = "input/y20/d15.txt"

func Run() int {
	input := []int{20, 0, 1, 11, 6, 3}
	//input := []int{0, 3, 6}
	history := make(map[int][]int)
	turn := 1
	lastNumber := 0
	for i := 0; i < len(input)-1; i++ {
		history[input[i]] = []int{turn}
		lastNumber = input[i]
		turn++
	}
	lastNumber = input[len(input)-1]
	turn++
	new := true
	for turn <= 30000001 {
		lastTurn, exists := history[lastNumber]
		if !exists {
			//fmt.Printf("...Adding new number %d:%d\n", lastNumber, turn-1)
			history[lastNumber] = []int{turn - 1}
			lastTurn, _ = history[lastNumber]
			new = true
		} else {
			//fmt.Printf("...appending %d:%d -> %v\n", lastNumber, turn-1, history[lastNumber])
			history[lastNumber] = append(history[lastNumber], turn-1)
			lastTurn, _ = history[lastNumber]
		}

		if exists && !new {
			diff := (turn - 1) - lastTurn[len(lastTurn)-2]
			if turn%100000 == 0 {
				fmt.Printf("Turn %d: last number was %d which was spoken on turn %d - next number %d\n", turn, lastNumber, lastTurn[len(lastTurn)-2], diff)
			}
			lastNumber = diff
			new = false
		} else {
			if turn%100000 == 0 {
				fmt.Printf("Turn %d: last number was %d which hasnt been spoken - new number 0 \n", turn, lastNumber)
			}
			lastNumber = 0
			new = false
		}
		turn++
	}

	return -1
}

func p1(input []string) {

}

func p2(input []string) {

}
