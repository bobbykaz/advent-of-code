package d9

import (
	"fmt"
)

func Part1() {
	finalScore := playGame(428, 72061)
	//finalScore := playGame(9, 1618)
	fmt.Println("final high score", finalScore)
}

func playGame(numPlayers int, finalMarbleScore int) int {
	players := make([]int, numPlayers)
	currentPlayer := 1 % numPlayers
	zeroMarble := node{Value: 0, Prev: nil, Next: nil}
	oneMarble := node{Value: 1, Prev: &zeroMarble, Next: &zeroMarble}
	zeroMarble.Next = &oneMarble
	zeroMarble.Prev = &oneMarble
	currentMarble := &oneMarble
	nextMarbleValue := 2
	for nextMarbleValue < finalMarbleScore+1 {
		nextMarble, score := playRound(currentMarble, nextMarbleValue)

		if score > 0 {
			players[currentPlayer] = players[currentPlayer] + score
			//fmt.Println("Player", (currentPlayer + 1), "scored", score, ", total: ", players[currentPlayer], "- next marble:", nextMarble.Value)
			//printMarbles(currentPlayer, nextMarble)
		}
		currentPlayer = (currentPlayer + 1) % numPlayers
		currentMarble = nextMarble
		nextMarbleValue++
	}

	maxScore := players[0]
	for i := 0; i < numPlayers; i++ {
		//fmt.Println("Player", i+1, "score:", players[i])
		if players[i] > maxScore {
			maxScore = players[i]
		}
	}
	return maxScore
}

func printMarbles(player int, marble *node) {
	fmt.Printf("[%d]  ", player+1)
	currentVal := marble.Value
	minVal := currentVal
	minNode := marble
	node := marble
	for node.Next != marble {
		if node.Value < minVal {
			minVal = node.Value
			minNode = node
		}
		node = node.Next
	}

	node = minNode
	for node.Next != minNode {
		if node.Value == currentVal {
			fmt.Printf("(%d) ", node.Value)
		} else {
			fmt.Printf("%d ", node.Value)
		}
		node = node.Next
	}
	if node.Value == currentVal {
		fmt.Printf("(%d) ", node.Value)
	} else {
		fmt.Printf("%d ", node.Value)
	}
	fmt.Println()
}

func playRound(currentMarble *node, nextMarbleValue int) (*node, int) {
	if nextMarbleValue%23 == 0 {
		score := nextMarbleValue
		nodeToRemove := currentMarble
		for i := 0; i < 7; i++ {
			nodeToRemove = nodeToRemove.Prev
		}
		nextMarble := nodeToRemove.Next
		//fmt.Println("--Scoring", score, "+", nodeToRemove.Value, "for total", (score + nodeToRemove.Value))
		score += nodeToRemove.Value
		remove(nodeToRemove)
		return nextMarble, score
	}

	nextMarble := node{Value: nextMarbleValue, Prev: nil, Next: nil}
	relate(currentMarble.Next, &nextMarble)
	return &nextMarble, 0
}

func relate(old, next *node) {
	origNext := old.Next
	old.Next = next
	next.Prev = old
	next.Next = origNext
	if origNext != nil {
		origNext.Prev = next
	}
}

func remove(node *node) {
	prev := node.Prev
	next := node.Next

	prev.Next = next
	next.Prev = prev

	node.Prev = nil
	node.Next = nil
}

type node struct {
	Prev  *node
	Next  *node
	Value int
}
