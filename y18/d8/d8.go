package d8

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input8.txt")
	strs := strings.Split(input[0], " ")
	nodeInfo := utilities.StringsToInts(strs)
	fmt.Println("Made ", len(nodeInfo), " pieces of node data")
	root, finalIndex := process(nodeInfo, 0)
	fmt.Println("done parsing tree, made ", len(nodeInfo), " nodes with final index ", finalIndex, " (they should be equal)")
	metaSum := traverseSumMetadata(root)
	fmt.Println("metadata total: ", metaSum)
	valueSum := getNodeValue(root)
	fmt.Println("root value: ", valueSum)
}

var globalNodeID = 0

type node struct {
	NodeID   int
	Value    int
	Children [](*node)
	Metadata []int
}

//returns a node, and the next index of the list
func process(nodeInfo []int, nodeStart int) (*node, int) {
	numKids := nodeInfo[nodeStart]
	numMetas := nodeInfo[nodeStart+1]
	resultNode := node{NodeID: globalNodeID, Value: -1, Children: nil, Metadata: nil}
	globalNodeID++
	if numKids > 0 {
		resultNode.Children = make([](*node), numKids)
	}
	if numMetas > 0 {
		resultNode.Metadata = make([]int, numMetas)
	}

	nextIndex := nodeStart + 2

	for i := 0; i < numKids; i++ {
		child, newNextIndex := process(nodeInfo, nextIndex)
		resultNode.Children[i] = child
		nextIndex = newNextIndex
	}

	for i := 0; i < numMetas; i++ {
		resultNode.Metadata[i] = nodeInfo[nextIndex]
		nextIndex++
	}

	return &resultNode, nextIndex
}

func traverseSumMetadata(n *node) int {
	sumMeta := n.sumMetaData()

	if n.Children != nil {
		for i := 0; i < len(n.Children); i++ {
			sumMeta += traverseSumMetadata(n.Children[i])
		}
	}

	return sumMeta
}

func (n *node) sumMetaData() int {
	sumMeta := 0
	if n.Metadata != nil {
		for i := 0; i < len(n.Metadata); i++ {
			sumMeta += n.Metadata[i]
		}
	}

	return sumMeta
}

func getNodeValue(n *node) int {
	if n.Children == nil {
		value := n.sumMetaData()
		return value
	}

	sumValues := 0
	if n.Metadata != nil {
		for i := 0; i < len(n.Metadata); i++ {
			index := n.Metadata[i]
			fmt.Printf("Processing node %d meta #%d: target child %d; total children %d\n", n.NodeID, i, index, len(n.Children))
			if index > 0 && index <= len(n.Children) {
				index--
				if n.Children[index].Value > -1 {
					fmt.Println("using saved val for node ", n.Children[index].NodeID)
					sumValues += n.Children[index].Value
				} else {
					value := getNodeValue(n.Children[index])
					n.Children[index].SetValue(value)
					sumValues += value
				}
			}
		}
	}

	return sumValues
}

func (n *node) SetValue(val int) {
	n.Value = val
	//fmt.Println("Node ", n.NodeID, ": ", n.Value)
}
