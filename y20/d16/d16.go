package d16

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d16.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	groups := utilities.GroupLinesByLineSeparator(input, "")
	fmt.Println("Groups:", len(groups))

	rules := make([][]int, 0)
	ruleNames := make([]string, 0)
	for _, s := range groups[0] {
		//"class: min-max or min-max"
		pts := strings.Split(s, ": ")
		rule := utilities.Split(pts[1], "-", " or ", "-")
		ruleInts := utilities.StringsToInts(rule)
		rules = append(rules, ruleInts)
		ruleNames = append(ruleNames, pts[0])
	}

	myTicket := utilities.StringToInts(groups[1][1])
	fmt.Println("me:", myTicket)

	otherTicketStr := groups[2][1:]
	validTickets := make([][]int, 0)
	sum := 0
	for _, t := range otherTicketStr {
		ticket := utilities.StringToInts(t)
		valid, num := checkTicket(ticket, rules)
		if !valid {
			sum += num
		} else {
			validTickets = append(validTickets, ticket)
		}
	}

	fmt.Println("Invalid sum:", sum)

	possibilities := ticketPossibilities(myTicket, rules)
	fmt.Println(possibilities)
	for _, t := range validTickets {
		newPoss := ticketPossibilities(t, rules)
		for i, p := range newPoss {
			possibilities[i] = utilities.Intersect(possibilities[i], p)
		}
	}

	for i, p := range possibilities {
		fmt.Printf("%s : %v\n", ruleNames[i], p)
	}

	return -1
}

func checkTicket(ticket []int, rules [][]int) (bool, int) {
	for _, v := range ticket {
		anyRule := false
		for _, r := range rules {
			thisRule := ((v >= r[0]) && (v <= r[1])) || ((v >= r[2]) && (v <= r[3]))
			if thisRule {
				anyRule = true
				break
			}
		}

		if !anyRule {
			return false, v
		}
	}
	return true, 0
}

func ticketPossibilities(ticket []int, rules [][]int) [][]int {
	possibilities := make([][]int, 0)
	for i, r := range rules {
		possibilities = append(possibilities, make([]int, 0))
		for ii, v := range ticket {
			thisRule := ((v >= r[0]) && (v <= r[1])) || ((v >= r[2]) && (v <= r[3]))
			if thisRule {
				possibilities[i] = append(possibilities[i], ii)
			}
		}
	}
	return possibilities
}
