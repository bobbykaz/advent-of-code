package d3

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1_2() {
	fmt.Println("Parsing...")
	input := utilities.ReadFileIntoLines("input/input3.txt")
	fabric := make([][]int, 1000)
	fmt.Println("Setup...")
	for i := 0; i < 1000; i++ {
		fabric[i] = make([]int, 1000)
		for j := 0; j < 1000; j++ {
			fabric[i][j] = 0
		}
	}
	fmt.Println("Processing...")
	for i := 0; i < len(input); i++ {
		order := processLine(input[i])
		for w := 0; w < order.Width; w++ {
			for h := 0; h < order.Height; h++ {
				fabric[order.Left+w][order.Top+h]++
			}
		}
	}

	overlap := 0
	for i := 0; i < 1000; i++ {
		line := ""
		for j := 0; j < 1000; j++ {
			if fabric[i][j] == 0 {
				line += " "
			} else if fabric[i][j] == 1 {
				line += "."
			} else {
				line += "X"
				overlap++
			}
		}
		//fmt.Println(line)
	}
	fmt.Println("Total Overlap: ", overlap)

	fmt.Println("Finding correct order...")
	for i := 0; i < len(input); i++ {
		order := processLine(input[i])
		cleanOrder := true
		for w := 0; w < order.Width; w++ {
			for h := 0; h < order.Height; h++ {
				if fabric[order.Left+w][order.Top+h] > 1 {
					cleanOrder = false
					w = order.Width
					h = order.Height
				}
			}
		}

		if cleanOrder {
			fmt.Printf("Order %s at (%d,%d) with %d x %d is clean!\n", order.ID, order.Left, order.Top, order.Width, order.Height)
		}
	}
}

type fabricOrder struct {
	ID     string
	Left   int
	Top    int
	Width  int
	Height int
}

func processLine(line string) fabricOrder {
	first := strings.Split(line, " @ ")
	strID := first[0]
	second := strings.Split(first[1], ": ")
	strCoord := second[0]
	strDim := second[1]
	coords := strings.Split(strCoord, ",")
	dims := strings.Split(strDim, "x")
	//
	left, _ := strconv.Atoi(coords[0])
	top, _ := strconv.Atoi(coords[1])
	width, _ := strconv.Atoi(dims[0])
	height, _ := strconv.Atoi(dims[1])
	result := fabricOrder{ID: strID, Left: left, Top: top, Width: width, Height: height}
	return result
}
