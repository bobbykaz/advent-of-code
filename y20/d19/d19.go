package d19

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d19-2.txt"

var Print = false

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	groups := utilities.GroupLinesByLineSeparator(input, "")
	rules := parseRules(groups[0])
	valid := 0
	for li, s := range groups[1] {
		rule := rules[0]
		i := 0
		b := false
		b, i = rule.Validate(s, i)
		tooShort := i != len(s)

		if b && !tooShort {
			valid++
		}
		fmt.Printf("%d: %s -> Valid? %v  Too short? %v ; Final: %v\n", li, s, b, tooShort, b && !tooShort)
	}

	return valid
}

type rule interface {
	Validate(str string, pos int) (bool, int)
}

type baseRule struct {
	C rune
}

func (r baseRule) Validate(str string, pos int) (bool, int) {
	if pos >= len(str) {
		return false, len(str)
	}
	if Print {
		spacer := "       "
		for i := 0; i < pos; i++ {
			spacer = spacer + " "
		}
		fmt.Printf("%s[%c]", spacer, str[pos])
		if pos+1 < len(str) {
			fmt.Printf("%s", str[pos+1:])
		}
		fmt.Printf(" == %c\n", r.C)
	}
	if rune(str[pos]) == r.C {
		return true, pos + 1
	}

	return false, -1
}

type orRule struct {
	IR1   []int
	IR2   []int
	Rules *map[int]rule
}

func (r orRule) Validate(str string, pos int) (bool, int) {
	if pos >= len(str) {
		return false, len(str)
	}
	if Print {
		spacer := "   "
		for i := 0; i < pos; i++ {
			spacer = spacer + " "
		}
		fmt.Printf("%sOR: [%c]", spacer, str[pos])
		if pos+1 < len(str) {
			fmt.Printf("%s", str[pos+1:])
		}
		fmt.Printf(" => %v\n", r)
	}

	b1, p1 := true, pos
	for _, i := range r.IR1 {
		rN, _ := (*(r.Rules))[i]
		b1, p1 = rN.Validate(str, p1)
		if !b1 {
			break
		}
	}

	if b1 {
		return b1, p1
	}

	b2, p2 := true, pos
	for _, i := range r.IR2 {
		rN, _ := (*(r.Rules))[i]
		b2, p2 = rN.Validate(str, p2)
		if !b2 {
			break
		}
	}

	if b2 {
		return b2, p2
	}

	return false, pos
}

type comboRule struct {
	Is    []int
	Rules *map[int]rule
}

func (r comboRule) Validate(str string, pos int) (bool, int) {
	if pos >= len(str) {
		return false, len(str)
	}

	if Print {
		spacer := ""
		for i := 0; i < pos; i++ {
			spacer = spacer + " "
		}
		fmt.Printf("%sCombo: [%c]", spacer, str[pos])
		if pos+1 < len(str) {
			fmt.Printf("%s", str[pos+1:])
		}
		fmt.Printf(" => %v\n", r)
	}
	b, p := true, pos
	for _, i := range r.Is {
		rN, _ := (*(r.Rules))[i]
		b, p = rN.Validate(str, p)
		if !b {
			return false, -1
		}
	}

	return b, p
}

func parseRules(ruleInput []string) map[int]rule {
	rules := make(map[int]rule)
	for i, s := range ruleInput {
		pts := strings.Split(s, ": ")
		ri, _ := strconv.Atoi(pts[0])
		r := parseRule(pts[1], &rules)
		rules[ri] = r
		if Print {
			fmt.Printf("%d: R%d => %v\n", i, ri, r)
		}
	}

	return rules
}

func parseRule(str string, ruleMap *map[int]rule) rule {
	if strings.Contains(str, "|") {
		r := orRule{}
		rs := strings.Split(str, " | ")
		r1Pts := strings.Split(rs[0], " ")
		r2Pts := strings.Split(rs[1], " ")
		r.IR1 = make([]int, 0)
		for _, s := range r1Pts {
			i, _ := strconv.Atoi(s)
			r.IR1 = append(r.IR1, i)
		}

		r.IR2 = make([]int, 0)
		for _, s := range r2Pts {
			i, _ := strconv.Atoi(s)
			r.IR2 = append(r.IR2, i)
		}

		r.Rules = ruleMap
		return r
	}
	if strings.Contains(str, "\"") {
		r := baseRule{C: rune(str[1])}

		return r
	}
	rPts := strings.Split(str, " ")
	r := comboRule{}
	r.Is = make([]int, 0)
	r.Rules = ruleMap
	for _, s := range rPts {
		i, _ := strconv.Atoi(s)
		r.Is = append(r.Is, i)
	}
	return r
}

func p1(input []string) {

}

func p2(input []string) {

}
