package d7

import (
	"fmt"
	"sort"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input7.txt")
	nodeMap := make(map[string]*node, len(input))
	for i := 0; i < len(input); i++ {
		parent, child := process(input[i])
		pN, prs := nodeMap[parent]
		if !prs {
			pN = &node{NodeID: parent, Time: 0, Children: make([]*node, 0), Parents: make([]*node, 0), Done: false}
			char := parent[0]
			pN.Time = (int)(char-'A') + 1 + 60
			nodeMap[parent] = pN
		}

		cN, prs := nodeMap[child]
		if !prs {
			cN = &node{NodeID: child, Time: 0, Children: make([]*node, 0), Parents: make([]*node, 0), Done: false}
			char := child[0]
			cN.Time = (int)(char-'A') + 1 + 60
			nodeMap[child] = cN
		}

		fmt.Println("Parent:", parent, "child:", child)
		pN.relate(cN)
	}
	fmt.Println("nodes made")
	nodesReady := make([]string, 0)
	nodesInProgress := make([](*node), 0)
	nodesDone := make([]string, 0)
	for _, v := range nodeMap {
		if v.canBeDone() {
			nodesReady = append(nodesReady, v.NodeID)
		}
	}
	sort.Strings(nodesReady)

	//Ready to start processing
	// 1. fill workers to capacity if possible
	// 2. complete one node.
	// 3. check if any other nodes are ready to be processed
	// 4. add them to Ready and sort
	totalTime := 0
	workers := 5

	for len(nodesReady) > 0 || len(nodesInProgress) > 0 {

		//Fill workers
		for len(nodesInProgress) < workers && len(nodesReady) > 0 {
			node := nodeMap[nodesReady[0]]
			nodesInProgress = append(nodesInProgress, node)
			nodesReady = nodesReady[1:]
		}

		sort.Slice(nodesInProgress, func(i, j int) bool { return lessNodeInProgress(nodesInProgress[i], nodesInProgress[j]) })

		//Process one node
		node, remaining := dequeueNode(nodesInProgress)
		nodesInProgress = remaining
		for i := 0; i < len(nodesInProgress); i++ {
			nodesInProgress[i].Time = nodesInProgress[i].Time - node.Time
		}
		totalTime += node.Time
		node.Time = 0
		node.Done = true
		fmt.Println("Completed node", node.NodeID, "after time", totalTime)
		//

		for i := 0; i < len(node.Children); i++ {
			if node.Children[i].canBeDone() {
				nodesReady = append(nodesReady, node.Children[i].NodeID)
			}
		}
		sort.Strings(nodesReady)
		nodesDone = append(nodesDone, node.NodeID)
	}

	fmt.Println("Order:", nodesDone)
}

func dequeueNode(arr []*node) (*node, []*node) {
	return arr[0], arr[1:]
}

func dequeue(arr []int) (int, []int) {
	return arr[0], arr[1:]
}

type node struct {
	NodeID   string
	Time     int
	Done     bool
	Parents  [](*node)
	Children [](*node)
}

//returns a node, and the next index of the list
func process(line string) (string, string) {
	//fmt.Println("1: ", line)
	pLine := strings.TrimPrefix(line, "Step ")
	pLine = strings.Trim(pLine, "\r")
	//fmt.Println("2: ", pLine)
	pLine = strings.TrimSuffix(pLine, " can begin.")
	pLine = strings.Trim(pLine, "\r")
	//fmt.Println("3: ", pLine)
	parts := strings.Split(pLine, " must be finished before step ")
	return strings.Trim(strings.TrimSpace(parts[0]), "\r"), strings.Trim(strings.TrimSpace(parts[1]), "\r")
}

func (n *node) relate(child *node) {
	n.Children = append(n.Children, child)
	child.Parents = append(child.Parents, n)
}

func (n *node) canBeDone() bool {
	//fmt.Println("Checking if done: ", n.NodeID)
	for i := 0; i < len(n.Parents); i++ {
		//fmt.Println("parent ", n.Parents[i].NodeID, " done: ", n.Parents[i].Done)
		if !((n.Parents[i]).Done) {
			return false
		}
	}

	return !n.Done
}

func traverseSumMetadata(n *node) int {

	if n.Children != nil {
		for i := 0; i < len(n.Children); i++ {
			traverseSumMetadata(n.Children[i])
		}
	}

	return -1
}

func lessNodeInProgress(a, b *node) bool {
	if a.Time == b.Time {
		return a.NodeID < b.NodeID
	}

	return a.Time < b.Time
}
