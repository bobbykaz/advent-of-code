package d25

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d25.txt"

var Print = true

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	log("%d\n", len(input))
	return -1
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
