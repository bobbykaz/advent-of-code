package d22

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d22.txt"

var Print = true

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	players := utilities.GroupLinesByLineSeparator(input, "")

	d1 := utilities.StringsToInts(players[0][1:])
	d2 := utilities.StringsToInts(players[1][1:])

	recursiveCombat(d1, d2, 0)

	return -1
}

func recursiveCombat(d1, d2 []int, depth int) bool {
	roundmap := make(map[string]bool)

	rounds := 1
	for !(len(d1) == 0 || len(d2) == 0) {
		turnKey := fmt.Sprintf("%v || %v", d1, d2)
		_, exists := roundmap[turnKey]
		if exists {
			//loop detected!
			return true
		}
		roundmap[turnKey] = true
		Print = rounds%500 == 0 && rounds > 1
		log("-- Round %d, depth %d --\n", rounds, depth)
		log("P1: %v\n", d1)
		log("P2: %v\n", d2)
		log("P1 plays %d\n P2 plays %d\n", d1[0], d2[0])
		p1Wins := d1[0] >= d2[0]
		if len(d1)-1 >= d1[0] && len(d2)-1 >= d2[0] {
			log("Recursing!\n")
			c1 := make([]int, d1[0])
			copy(c1, d1[1:])
			c2 := make([]int, d2[0])
			copy(c2, d2[1:])
			p1Wins = recursiveCombat(c1, c2, depth+1)
			log("Resuming...\n")
			log("P1: %v\n", d1)
			log("P2: %v\n", d2)
			log("P1 plays %d\n P2 plays %d\n", d1[0], d2[0])

		}
		if p1Wins {
			t := d1[0]
			b := d2[0]
			d1 = d1[1:]
			d2 = d2[1:]
			d1 = append(d1, t, b)
			log("P1 wins round %d\n\n", rounds)
		} else {
			t := d2[0]
			b := d1[0]
			d1 = d1[1:]
			d2 = d2[1:]
			d2 = append(d2, t, b)
			log("P2 wins round %d\n\n", rounds)
		}
		rounds++
	}
	fmt.Printf("Recursive game depth %d over : P1:%d P2:%d\n\n", depth, len(d1), len(d2))

	if depth == 0 {
		Print = true
		log("Over after %d rounds; P1: %d P2: %d\n", rounds, len(d1), len(d2))
		log("P1: %v\n", d1)
		log("P2: %v\n", d2)
		p1Score := 0
		iter := 1
		for i := len(d1) - 1; i >= 0; i-- {
			p1Score += iter * d1[i]
			iter++
		}
		log("P1 Score: %d\n", p1Score)

		p2Score := 0
		iter = 1
		for i := len(d2) - 1; i >= 0; i-- {
			p2Score += iter * d2[i]
			iter++
		}
		log("P2 Score: %d\n", p2Score)
	}

	return len(d1) > len(d2)
}

func combat(d1, d2 []int) {
	rounds := 0
	for !(len(d1) == 0 || len(d2) == 0) {
		Print = rounds%1000 == 0
		log("P1 plays %d\n P2 plays %d\n", d1[0], d2[0])
		if d1[0] >= d2[0] {
			t := d1[0]
			b := d2[0]
			d1 = d1[1:]
			d2 = d2[1:]
			d1 = append(d1, t, b)
			log("P1 wins round %d\n", rounds)
		} else {
			t := d2[0]
			b := d1[0]
			d1 = d1[1:]
			d2 = d2[1:]
			d2 = append(d2, t, b)
			log("P2 wins round %d\n", rounds)
		}

		log("P1: %v\n", d1)
		log("P2: %v\n", d2)
		rounds++
	}
	Print = true
	log("Over after %d rounds; P1: %d P2: %d\n", rounds, len(d1), len(d2))
	p1Score := 0
	iter := 1
	for i := len(d1) - 1; i >= 0; i-- {
		p1Score += iter * d1[i]
		iter++
	}
	log("P1 Score: %d\n", p1Score)

	p2Score := 0
	iter = 1
	for i := len(d2) - 1; i >= 0; i-- {
		p2Score += iter * d2[i]
		iter++
	}
	log("P2 Score: %d\n", p2Score)
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
