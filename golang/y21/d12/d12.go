package d12

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d12.txt")
	nodeMap := mapCave(lines)
	return exploreCave(nodeMap, true)
}

func mapCave(lines []string) map[string]*node {
	nodeMap := make(map[string]*node, 0)
	for i := 0; i < len(lines); i++ {
		pts := strings.Split(lines[i], "-")
		a := getNode(pts[0], &nodeMap)
		b := getNode(pts[1], &nodeMap)
		a.addPath(b)
		b.addPath(a)
		fmt.Println(lines[i], "->", (*a), (*b))
	}
	return nodeMap
}

func exploreCave(nmap map[string]*node, part2 bool) int {
	start, _ := nmap["start"]
	path := make([]string, 0)
	path = append(path, start.Id)
	if part2 {
		return recursePart2(start, path)
	}
	return recurse(start, path)
}

func recurse(n *node, currentPath []string) int {
	if n.Id == "end" {
		//fmt.Println(currentPath)
		return 1
	}
	sums := 0
	//fmt.Println("Current Node", (*n))
	for i := 0; i < len(n.Paths); i++ {
		if n.Paths[i].IsSmall && !utilities.StringSliceContains(n.Paths[i].Id, currentPath) {
			sums += recurse(n.Paths[i], append(currentPath, n.Paths[i].Id))
		} else if !n.Paths[i].IsSmall {
			sums += recurse(n.Paths[i], append(currentPath, n.Paths[i].Id))
		}
	}
	return sums
}

func recursePart2(n *node, currentPath []string) int {
	if n.Id == "end" {
		//fmt.Println(currentPath)
		return 1
	}
	sums := 0
	//fmt.Println("Current Node", (*n))
	for i := 0; i < len(n.Paths); i++ {
		if n.Paths[i].IsSmall && n.Paths[i].Id != "start" {
			//are there no dupes of small caves yet
			if utilities.StringSliceCountInstances(n.Paths[i].Id, currentPath) == 1 {
				sums += recurse(n.Paths[i], append(currentPath, n.Paths[i].Id))
			} else {
				sums += recursePart2(n.Paths[i], append(currentPath, n.Paths[i].Id))
			}
		} else if !n.Paths[i].IsSmall {
			sums += recursePart2(n.Paths[i], append(currentPath, n.Paths[i].Id))
		}
	}
	return sums
}

func pathContainsTwoOfTheSameSmallCave(path []string) bool {
	for i := 0; i < len(path); i++ {
		if path[i] == strings.ToLower(path[i]) { //its a small cave
			if utilities.StringSliceCountInstances(path[i], path) > 1 {
				fmt.Println("..Path", path, "contains 2 of", path[i])
				return true
			}
		}
	}
	return false
}

func (n *node) containsPath(id string) bool {
	for i := 0; i < len(n.Paths); i++ {
		if n.Paths[i].Id == id {
			return true
		}
	}
	return false
}

func (n *node) addPath(other *node) {
	if !n.containsPath(other.Id) {
		n.Paths = append(n.Paths, other)
	}
}

func getNode(id string, nodeMap *map[string]*node) *node {
	n, exists := (*nodeMap)[id]
	if exists {
		return n
	}
	newN := makeNode(id)
	(*nodeMap)[id] = newN
	return newN
}

func makeNode(id string) *node {
	isSmallCave := (id == strings.ToLower(id))
	n := node{Id: id, Paths: make([]*node, 0), IsSmall: isSmallCave}
	return &n
}

type node struct {
	Id      string
	Paths   []*node
	IsSmall bool
}
