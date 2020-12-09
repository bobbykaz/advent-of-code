package d5

import (
	"fmt"
	"strings"
	"unicode"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Solve() {
	fmt.Println("starting d6.")
	input := utilities.ReadFileIntoLines("input/input5.txt")
	runes := []rune(input[0])
	runes = annihilate(runes)
	final := string(runes)
	fmt.Println(final)
	fmt.Println("len: ", len(final))
	removeAndSolve(runes)
}

func removeAndSolve(finalRunes []rune) {
	for i := 0; i < len(alphabet); i++ {
		temp := strings.Replace(string(finalRunes), string(alphabet[i]), "", -1)
		temp = strings.Replace(temp, string(unicode.ToUpper(alphabet[i])), "", -1)
		tempRunes := []rune(temp)
		tempRunes = annihilate(tempRunes)
		fmt.Printf("removing %c: len %d\n", alphabet[i], len(tempRunes))
	}
}

var alphabet = []rune{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'}

func annihilate(runes []rune) []rune {
	for i := 0; i < len(runes)-1; i++ {
		if runesAnnihilate(runes[i], runes[i+1]) {
			//fmt.Println("clearing ", i, i+1)
			runes = append(runes[:i], runes[i+2:]...)
			i -= 2
			if i < 0 {
				i = -1
			}
		}
	}
	return runes
}

func runesAnnihilate(a, b rune) bool {
	//fmt.Printf("Checking: (%c,%c); lowers: %c, %c\n", a, b, unicode.ToLower(a), unicode.ToLower(b))
	if unicode.ToLower(a) == unicode.ToLower(b) {
		if unicode.IsLower(a) {
			return unicode.IsUpper(b)
		}

		if unicode.IsUpper(a) {
			return unicode.IsLower(b)
		}
	}
	return false
}
