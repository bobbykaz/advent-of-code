package d8

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y19/d8.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	p1(input[0], 25, 6)
	p2(input)
	return -1
}

func p1(input string, w, h int) {
	layers := make([][][]int, len(input)/w/h)
	for i := range layers {
		layers[i] = make([][]int, h)
		for j := 0; j < h; j++ {
			layers[i][j] = make([]int, w)
		}
	}

	zpl := make([]int, len(layers))
	opl := make([]int, len(layers))
	tpl := make([]int, len(layers))

	for i, b := range input {
		v := int(byte(b - '0'))
		l := i / (w * h)
		r := (i % (w * h)) / w
		c := i % 25

		layers[l][r][c] = v
		switch v {
		case 0:
			zpl[l]++
		case 1:
			opl[l]++
		case 2:
			tpl[l]++
		}
	}

	ml := 0
	mlZeros := 151
	for i, v := range zpl {
		if v < mlZeros {
			ml = i
			mlZeros = v
		}
	}
	fmt.Println("layer", ml, "has fewest 0s:", mlZeros)
	fmt.Println("layer", ml, "has 1s:", opl[ml], "2s:", tpl[ml], "product:", opl[ml]*tpl[ml])
	fmt.Println("totals:", len(zpl), len(opl), len(tpl), len(layers))

	final := make([][]int, h)
	for i := range final {
		final[i] = make([]int, w)
	}

	for r := 0; r < h; r++ {
		for c := 0; c < w; c++ {
			for i := 0; i < len(layers); i++ {
				if layers[i][r][c] != 2 {
					final[r][c] = layers[i][r][c]
					i = len(layers)
				}
			}
		}
	}
	for r := 0; r < h; r++ {
		for c := 0; c < w; c++ {
			if final[r][c] == 0 {
				fmt.Printf(" ")
			} else {
				fmt.Printf("X")
			}
		}
		fmt.Println()
	}
}

func p2(input []string) {

}
