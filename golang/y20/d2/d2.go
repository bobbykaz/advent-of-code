package d2

import (
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20/d2.txt")
	good := 0
	for i := 0; i < len(input); i++ {
		parts := utilities.Split(input[i], "-", " ", ":")
		min, _ := strconv.Atoi(parts[0])
		max, _ := strconv.Atoi(parts[1])
		target := rune(parts[2][0])
		if isValidPwd(min, max, target, strings.TrimSpace(parts[3])) {
			good++
		}
	}
	return good
}

func isValidPwd(min int, max int, letter rune, pwd string) bool {
	count := 0

	for i := 0; i < len(pwd); i++ {
		if letter == rune(pwd[i]) {
			count++
		}
	}

	//fmt.Println("Checkin", pwd, "Found", letter, count, "Min/Max", min, max)

	return count <= max && count >= min
}

func isValidPwd2(min int, max int, letter rune, pwd string) bool {

	p1, p2 := false, false

	p1 = letter == rune(pwd[min-1])
	p2 = letter == rune(pwd[max-1])

	return (p1 && !p2) || (p2 && !p1)

}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20/d2.txt")
	good := 0
	for i := 0; i < len(input); i++ {
		parts := utilities.Split(input[i], "-", " ", ":")
		min, _ := strconv.Atoi(parts[0])
		max, _ := strconv.Atoi(parts[1])
		target := rune(parts[2][0])
		if isValidPwd2(min, max, target, strings.TrimSpace(parts[3])) {
			good++
		}
	}
	return good
}
