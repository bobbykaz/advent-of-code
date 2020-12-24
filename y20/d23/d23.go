package d23

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var Print = true

func Run() int {
	//input := "3,8,9,1,2,5,4,6,7"
	input := "5,8,9,1,7,4,2,6,3"
	ints := utilities.StringToInts(input)

	Part2(ints)
	log("%d\n", len(input))
	return -1
}

func Part2(ints []int) {
	maxNodes := 1000000
	for i := 10; i <= maxNodes; i++ {
		ints = append(ints, i)
	}
	next, nodeMap := buildListWithMap(ints)
	log("Starting...total nodes:%d\n", len(nodeMap))
	loop := 0
	for loop < 10*maxNodes {
		sublist := next.RemoveNextN(3)
		targetV := next.V - 1
		if targetV <= 0 {
			targetV = maxNodes
		}
		target := sublist.FindForward(targetV)
		for target != nil {
			targetV--
			if targetV <= 0 {
				targetV = maxNodes
			}
			target = sublist.FindForward(targetV)
		}
		target = nodeMap[targetV]
		if target == nil {
			fmt.Printf("Couldnt find target: %d on loop %d \n", targetV, loop)
			sublist.PrintForward()
		}
		target.Insert(sublist)
		next = next.Next
		loop++
	}
	next = nodeMap[1]
	log("Next 3 cups: %d -> %d -> %d\n", next.V, next.Next.V, next.Next.Next.V)
	p := next.Next.V * next.Next.Next.V
	log("Product: %d\n", p)
}

func Part1(ints []int) {
	next := buildList(ints)
	next.PrintForward()
	next.PrintBackward()
	log("Starting...\n")
	loop := 0
	for loop < 100 {
		sublist := next.RemoveNextN(3)
		targetV := next.V - 1
		target := next.FindForward(targetV)
		for target == nil && targetV > 1 {
			targetV--
			target = next.FindForward(targetV)
		}
		if target == nil {
			target = next.FindMaxForward()
		}
		target.Insert(sublist)
		next = next.Next
		loop++
	}
	next.PrintForward()
}

func buildListWithMap(ints []int) (*linkNode, map[int]*linkNode) {
	nodeMap := make(map[int]*linkNode)
	var firstN *linkNode
	var lastN *linkNode
	for _, val := range ints {
		newNode := linkNode{V: val, Prev: lastN, Next: nil}
		nodeMap[val] = &newNode
		if firstN == nil {
			firstN = &newNode
		}
		if lastN != nil {
			lastN.Next = &newNode
		}

		lastN = &newNode
	}

	firstN.Prev = lastN
	lastN.Next = firstN
	return firstN, nodeMap
}

func buildList(ints []int) *linkNode {
	var firstN *linkNode
	var lastN *linkNode
	for _, val := range ints {
		newNode := linkNode{V: val, Prev: lastN, Next: nil}
		if firstN == nil {
			firstN = &newNode
		}
		if lastN != nil {
			lastN.Next = &newNode
		}

		lastN = &newNode
	}

	firstN.Prev = lastN
	lastN.Next = firstN
	return firstN
}

func (node *linkNode) RemoveNextN(n int) *linkNode {
	removed := node.Next
	removed.Prev = nil
	node.Next = nil
	last := removed
	for i := 0; i < n; i++ {
		last = last.Next
	}
	//[1 |2 3 4| 5 6]
	last.Prev.Next = removed
	removed.Prev = last.Prev
	last.Prev = node
	node.Next = last
	return removed
}

func (node *linkNode) Insert(sublist *linkNode) {
	insertEnd := node.Next
	sublistEnd := sublist.Prev

	insertEnd.Prev = sublistEnd
	sublistEnd.Next = insertEnd

	node.Next = sublist
	sublist.Prev = node
}

type linkNode struct {
	V    int
	Prev *linkNode
	Next *linkNode
}

func (n *linkNode) PrintForward() {
	n.printForwardHelper(n.V)
}

func (n *linkNode) printForwardHelper(stopAt int) {
	fmt.Printf("%d --> ", n.V)
	if n.Next == nil {
		fmt.Print("nil \n")
	} else if n.Next.V == stopAt {
		fmt.Print("... \n")
	} else {
		n.Next.printForwardHelper(stopAt)
	}
}

func (n *linkNode) PrintBackward() {
	n.printBackwardHelper(n.V)
}

func (n *linkNode) printBackwardHelper(stopAt int) {
	fmt.Printf("%d --> ", n.V)
	if n.Prev == nil {
		fmt.Print("nil \n")
	} else if n.Prev.V == stopAt {
		fmt.Print("... \n")
	} else {
		n.Prev.printBackwardHelper(stopAt)
	}
}

func (n *linkNode) FindForward(val int) *linkNode {
	return n.findForwardHelper(val, n)
}

func (n *linkNode) findForwardHelper(val int, stopAt *linkNode) *linkNode {
	if n.Next == nil {
		return nil
	} else if n.Next.V == val {
		return n.Next
	} else if n.Next == stopAt {
		return nil
	} else {
		return n.Next.findForwardHelper(val, stopAt)
	}
}

func (n *linkNode) FindMaxForward() *linkNode {
	return n.findMaxForwardHelper(n, n)
}

func (n *linkNode) findMaxForwardHelper(currentMax, stopAt *linkNode) *linkNode {
	if n.Next == nil {
		return currentMax
	} else if n.Next.V > currentMax.V {
		return n.Next.findMaxForwardHelper(n.Next, stopAt)
	} else if n.Next == stopAt {
		return currentMax
	} else {
		return n.Next.findMaxForwardHelper(currentMax, stopAt)
	}
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
