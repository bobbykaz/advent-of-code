package d14

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d14.txt")

	rules := lines[2:]
	ruleMap := make(map[string]rune)
	for _, s := range rules {
		pts := strings.Split(s, " -> ")
		r := rune(pts[1][0])
		ruleMap[pts[0]] = r
	}

	countPolymerDynamic(lines[0], ruleMap, 40)

	return 0
}

func naive(poly string, ruleMap map[string]rune) int {
	polymer := buildPoly(poly)

	for i := 0; i < 10; i++ {
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
	return max - min
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

func countPolymerDynamic(polymer string, ruleMap map[string]rune, levels int) int64 {
	scoreMap := dynamicTreeCount(ruleMap, levels)
	finalScore := make(map[rune]int64)
	for _, c := range polymer {
		_, exists := finalScore[c]
		if exists {
			finalScore[c] = 1 + finalScore[c]
		} else {
			finalScore[c] = 1
		}
	}

	for i := 0; i < len(polymer)-1; i++ {
		curr := key(rune(polymer[i]), rune(polymer[i+1]))
		ps, exists := scoreMap[curr]
		if exists {
			finalScore = combineGenLetters(finalScore, ps.GeneratedLetters)
		}
	}

	fmt.Println("Final scores:", finalScore)
	min, max := int64(-1), int64(-1)
	for _, v := range finalScore {
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
	return max - min
}

func dynamicTreeCount(ruleMap map[string]rune, levels int) scoreMap {
	scores := make([]scoreMap, levels)
	for i := 0; i < levels; i++ {
		scores[i] = make(scoreMap)
	}

	//seed initial steps
	for k, v := range ruleMap {
		ps := pairScore{Key: k, C1: rune(k[0]), C2: rune(k[1])}
		ps.GeneratedLetters = make(map[rune]int64)
		ps.GeneratedLetters[v] = 1
		scores[0][k] = ps
	}

	for i := 1; i < levels; i++ {
		//for any rule AB -> C, the total generated letters at step N is
		// equal to  C + (AC at N-1) + (CB at n-1)
		for k, v := range ruleMap {
			ps := pairScore{Key: k, C1: rune(k[0]), C2: rune(k[1])}
			genLet := make(map[rune]int64)
			genLet[v] = 1
			key1 := key(ps.C1, v)
			sub1, exists1 := scores[i-1][key1]
			if exists1 {
				genLet = combineGenLetters(genLet, sub1.GeneratedLetters)
			}
			key2 := key(v, ps.C2)
			sub2, exists2 := scores[i-1][key2]
			if exists2 {
				genLet = combineGenLetters(genLet, sub2.GeneratedLetters)
			}
			ps.GeneratedLetters = genLet
			scores[i][k] = ps
		}
	}

	return scores[levels-1]
}

type scoreMap map[string]pairScore

type pairScore struct {
	Key              string
	C1               rune
	C2               rune
	GeneratedLetters map[rune]int64
}

func key(a, b rune) string {
	return fmt.Sprintf("%c%c", a, b)
}

func combineGenLetters(a, b map[rune]int64) map[rune]int64 {
	result := make(map[rune]int64)
	for k, v := range a {
		result[k] = v
	}

	for k, v := range b {
		_, exists := result[k]
		if exists {
			result[k] = result[k] + v
		} else {
			result[k] = v
		}
	}

	return result
}
