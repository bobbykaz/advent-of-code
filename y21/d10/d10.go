package d10

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d10.txt")
	score := 0
	autoScore := make([]int, 0)
	for i := 0; i < len(lines); i++ {
		output, autocomplete := checkLine(lines[i])
		switch output {
		case ')':
			score += 3
		case '>':
			score += 25137
		case ']':
			score += 57
		case '}':
			score += 1197
		default:
			autoScore = append(autoScore, autocomplete)
		}
	}
	sort.Ints(autoScore)

	fmt.Println("Syntax score", score)
	fmt.Println("autocomplete scores", autoScore)
	fmt.Println("middle autocomplete score", autoScore[len(autoScore)/2])
	return score
}

func checkLine(input string) (byte, int) {
	stack := make([]byte, 0)
	for i := 0; i < len(input); i++ {
		switch input[i] {
		case '(':
			fallthrough
		case '<':
			fallthrough
		case '[':
			fallthrough
		case '{':
			stack = append([]byte{input[i]}, stack...)
		case ')':
			if stack[0] == '(' {
				stack = stack[1:]
			} else {
				return ')', 0
			}
		case '>':
			if stack[0] == '<' {
				stack = stack[1:]
			} else {
				return '>', 0
			}
		case ']':
			if stack[0] == '[' {
				stack = stack[1:]
			} else {
				return ']', 0
			}
		case '}':
			if stack[0] == '{' {
				stack = stack[1:]
			} else {
				return '}', 0
			}
		default:
			fmt.Println("Error??? - ", input[i])
		}
	}

	autocompleteScore := 0
	for i := 0; i < len(stack); i++ {
		switch stack[i] {
		case '(':
			autocompleteScore *= 5
			autocompleteScore += 1
		case '[':
			autocompleteScore *= 5
			autocompleteScore += 2
		case '{':
			autocompleteScore *= 5
			autocompleteScore += 3
		case '<':
			autocompleteScore *= 5
			autocompleteScore += 4
		default:
			fmt.Println("Error2 ?? -", input[i])
		}
	}

	return '0', autocompleteScore
}
