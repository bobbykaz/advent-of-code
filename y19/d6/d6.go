package d6

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	orbits := utilities.ReadFileIntoLines("input/y19/d6.txt")
	orbitMap := make(map[string]*Node)
	orbitMap["COM"] = &Node{Name: "COM", Parent: nil, Depth: 0}
	for _, s := range orbits {
		parts := strings.Split(s, ")")
		var parent, child *Node
		_, pOK := orbitMap[parts[0]]
		if pOK {
			parent = orbitMap[parts[0]]
		} else {
			//fmt.Println("Creating parent ", parts[0])
			parent = &Node{Name: parts[0], Parent: nil, Depth: -1}
			orbitMap[parent.Name] = parent
		}

		_, cOK := orbitMap[parts[1]]
		if cOK {
			child = orbitMap[parts[1]]
			child.Parent = parent
		} else {
			//fmt.Println("Creating child ", parts[1])
			child = &Node{Name: parts[1], Parent: parent, Depth: -1}
			orbitMap[child.Name] = child
		}

		if child.Depth == -1 && parent.Depth >= 0 {
			child.Depth = parent.Depth + 1
		}
	}

	for _, v := range orbitMap {
		if v.Depth == -1 {
			fixDepths(v)
		}
	}

	depthSum := 0

	for _, v := range orbitMap {
		depthSum += v.Depth
	}

	you := orbitMap["YOU"]
	printParents(you)
	san := orbitMap["SAN"]
	printParents(san)
	return depthSum
}

func fixDepths(c *Node) {
	if c != nil {
		if c.Parent != nil && c.Parent.Depth == -1 {
			fixDepths(c.Parent)
		}
		if c.Parent != nil && c.Parent.Depth >= 0 {
			c.Depth = c.Parent.Depth + 1
		}
	}
}
func printParents(c *Node) {
	printParentsHelper(c, true)
}

func printParentsHelper(c *Node, root bool) {
	if c != nil {
		if c.Parent != nil {
			fmt.Print(c.Name, ">")
			printParentsHelper(c.Parent, false)
		}
		if root {
			fmt.Print("\n")
		}
	}
}

//Node represents a tree structure
type Node struct {
	Name   string
	Parent *Node
	Depth  int
}
