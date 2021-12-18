package d14

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d14.txt")
	polymer := buildPoly(lines[0])
	rules := lines[2:]
	ruleMap := make(map[string]rune)
	for _, s := range rules {
		pts := strings.Split(s, " -> ")
		r := rune(pts[1][0])
		ruleMap[pts[0]] = r
	}

	for i := 0; i < 40; i++ {
		step(polymer, ruleMap)
	}

	n := polymer
	counts := make(map[rune]int)
	for n != nil {
		_, exists := counts[n.C]
		if exists {
			counts[n.C] = counts[n.C] + 1
		} else {
			counts[n.C] = 1
		}
		n = n.Next
	}
	min, max := -1, -1
	for _, v := range counts {
		if min == -1 {
			min = v
		}
		if v < min {
			min = v
		}
		if v > max {
			max = v
		}
	}
	fmt.Println("Min", min, "max", max, "diff", max-min)
	return 0
}

func step(polymer *node, ruleMap map[string]rune) *node {
	current := polymer
	for current.Next != nil {
		target := current
		current = current.Next
		key := fmt.Sprintf("%c%c", target.C, target.Next.C)
		r, exists := ruleMap[key]
		if exists {
			//fmt.Printf("Rule %s causes Inserting %c after %c\n", key, r, target.C)
			target.Insert(r)
		}
	}
	current = polymer
	for current != nil {
		//fmt.Printf("%c", current.C)
		current = current.Next
	}
	//fmt.Println()
	return polymer
}

func buildPoly(line string) *node {
	var head *node = nil
	var current *node = nil
	for _, c := range line {
		if head == nil {
			head = &node{C: c, Next: nil}
			current = head
		} else {
			current = current.Insert(c)
		}
	}
	return head
}

type node struct {
	C    rune
	Next *node
}

func (n *node) Insert(c rune) *node {
	last := n.Next
	newNode := node{C: c, Next: last}
	n.Next = &newNode
	return &newNode
}
