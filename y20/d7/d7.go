package d7

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d7.txt"

type bagNode struct {
	Color    string
	Contents []*bagDetails
	Parents  []*bagNode
}

type bagDetails struct {
	N     int
	Color string
	Ptr   *bagNode
}

func buildBag(rule string) bagNode {
	b := bagNode{}

	pts := utilities.Split(rule, " bags contain ")
	b.Color = pts[0]
	b.Contents = make([]*bagDetails, 0)
	b.Parents = make([]*bagNode, 0)
	if pts[1] != "no other bags." {
		details := strings.Split(pts[1], ", ")
		for _, s := range details {
			b.Contents = append(b.Contents, parseBagDetails(s))
		}
	}
	return b
}

// str: 4 wavy teal bag(s)(.)
func parseBagDetails(str string) *bagDetails {
	pts := strings.Split(str, " bag")
	rule := strings.SplitN(pts[0], " ", 2)
	n, _ := strconv.Atoi(rule[0])
	d := bagDetails{N: n, Color: rule[1]}
	return &d
}

func findParents(b *bagNode) int {
	colors := make([]string, 0)
	for _, bp := range b.Parents {
		colors = append(colors, bp.Color)
		colors = append(colors, recurseParent(bp)...)
	}

	uniq := make(map[string]bool)
	for _, s := range colors {
		uniq[s] = true
	}

	fmt.Println(b.Color, "has parents", colors, "for total", len(uniq))
	return len(uniq)
}

func recurseParent(b *bagNode) []string {
	colors := make([]string, 0)
	for _, bp := range b.Parents {
		colors = append(colors, bp.Color)
		colors = append(colors, recurseParent(bp)...)
	}
	return colors
}

func countChildren(b *bagNode) int {
	if b == nil {
		return 1
	}
	sum := 0
	for _, bd := range b.Contents {
		sum += bd.N * countChildren(bd.Ptr)
	}
	return sum + 1
}

func Part1() int {
	input := utilities.ReadFileIntoLines(inputFile)
	rules := make(map[string]*bagNode)
	//Build
	for _, s := range input {
		b := buildBag(s)
		rules[b.Color] = &b
	}

	//Connect pointers

	for _, v := range rules {
		for _, bd := range v.Contents {
			child, exists := rules[bd.Color]
			if exists {
				bd.Ptr = child
				child.Parents = append(child.Parents, v)
			} else {
				fmt.Println("Could not find bag ", bd.Color)
			}
		}
	}

	start, exists := rules["shiny gold"]

	if exists {
		findParents(start)
		fmt.Println("kids:", countChildren(start)-1) //dont count shiny gold itself
	}

	return -1
}
