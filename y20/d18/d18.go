package d18

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d18.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	sum := 0
	for _, s := range input {
		s = strings.Replace(s, "(", "( ", -1)
		s = strings.Replace(s, ")", " )", -1)
		terms := strings.Split(s, " ")
		t2 := addPrecedenceToAddition(terms)
		v := eval(t2)
		fmt.Printf("%s = %d\n", s, v)
		sum += v
	}

	return sum
}

func eval(terms []string) int {
	if len(terms) == 0 {
		return 0
	}

	total := 0
	action := 1 // 1 is add, 2 is multiply
	i := 0
	for i < len(terms) {
		if terms[i] == "(" {
			subTerms, next := extractSubstatement(terms[i:])
			value := eval(subTerms)

			switch action {
			case 1:
				total += value
			case 2:
				total *= value
			}
			i += next
		} else if terms[i] == "+" {
			action = 1
			i++
		} else if terms[i] == "*" {
			action = 2
			i++
		} else {
			v, err := strconv.Atoi(terms[i])
			if err != nil {
				panic(fmt.Sprintf("Some other term: %s : %d\n", terms[i], i))
			}
			switch action {
			case 1:
				total += v
			case 2:
				total *= v
			}
			i++
		}
	}

	return total
}

func addPrecedenceToAddition(terms []string) []string {
	newTerms := make([]string, 0)

	if len(terms) == 0 {
		return newTerms
	}

	mulPtr := -1
	escaping := false
	i := 0
	for i < len(terms) {
		if terms[i] == "(" {
			subTerms, next := extractSubstatement(terms[i:])
			newSubTerms := addPrecedenceToAddition(subTerms)
			newTerms = append(newTerms, "(")
			newTerms = append(newTerms, newSubTerms...)
			newTerms = append(newTerms, ")")
			i += next
		} else if terms[i] == "+" {
			if mulPtr >= 0 {
				escaping = true
			}

			if escaping && mulPtr >= 0 {
				firstHalf := newTerms[:mulPtr]
				backHalf := newTerms[mulPtr:]
				backCopy := make([]string, len(backHalf))
				copy(backCopy, backHalf)
				newTerms = append(firstHalf, "(")
				newTerms = append(newTerms, backCopy...)
				mulPtr = -1
			}
			newTerms = append(newTerms, "+")
			i++
		} else if terms[i] == "*" {
			if escaping {
				escaping = false
				newTerms = append(newTerms, ")")
			}
			newTerms = append(newTerms, "*")
			mulPtr = len(newTerms)
			i++
		} else {
			newTerms = append(newTerms, terms[i])
			i++
		}
	}

	if escaping {
		escaping = false
		newTerms = append(newTerms, ")")
	}

	return newTerms
}

//returns substatement, continue after
func extractSubstatement(terms []string) ([]string, int) {
	if terms[0] != "(" {
		panic("not a substatement!")
	}
	open := 1
	i := 1
	for open > 0 {
		if terms[i] == "(" {
			open++
		} else if terms[i] == ")" {
			open--
		}
		i++
	}

	return terms[1 : i-1], i
}
