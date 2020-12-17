package d17

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d17.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	return P2(input)
}

func P1(input []string) int {
	h := len(input)
	w := len(input[0])

	//add 7 in either direction as a buffer for 6 cycles
	h += 14
	w += 14
	z := 15

	//z , row, col
	space := getNewSpace(z, h, w)

	//fill center layer, z==7
	for r := 0; r < len(input); r++ {
		for c := 0; c < len(input[0]); c++ {
			if rune(input[r][c]) == '#' {
				space[7][r+7][c+7] = 1
			}
		}
	}
	fmt.Println("Init Sum", sumSpace(space))

	for i := 0; i < 6; i++ {
		next := cycle(space)
		space = next
		fmt.Println("Sum", i, ":", sumSpace(space))
	}

	return sumSpace(space)
}

func P2(input []string) int {
	h := len(input)
	w := len(input[0])

	//add 7 in either direction as a buffer for 6 cycles
	h += 14
	w += 14
	z := 15
	hw := 15

	//z , row, col
	space := getNewHyperSpace(hw, z, h, w)

	//fill center layer, z==7
	for r := 0; r < len(input); r++ {
		for c := 0; c < len(input[0]); c++ {
			if rune(input[r][c]) == '#' {
				space[7][7][r+7][c+7] = 1
			}
		}
	}
	fmt.Println("Init Sum", sumHyperSpace(space))

	for i := 0; i < 6; i++ {
		next := hyperCycle(space)
		space = next
		fmt.Println("Sum", i, ":", sumHyperSpace(space))
	}

	return sumHyperSpace(space)
}

func cycle(current [][][]int) [][][]int {
	z, h, w := len(current), len(current[0]), len(current[0][0])
	next := getNewSpace(z, h, w)
	for i := range current {
		for j := range current[i] {
			for k := range current[i][j] {
				adj := adjacent(i, j, k, current)
				sum := 0
				for _, v := range adj {
					sum += v
				}

				if sum == 3 {
					next[i][j][k] = 1
				} else if sum == 2 && current[i][j][k] == 1 {
					next[i][j][k] = 1
				} else {
					next[i][j][k] = 0
				}

				//fmt.Printf("(%d,%d,%d): %d ; %v -> %d\n", i, j, k, current[i][j][k], adj, next[i][j][k])
			}
		}
	}
	return next
}

func hyperCycle(current [][][][]int) [][][][]int {
	hw, z, h, w := len(current), len(current[0]), len(current[0][0]), len(current[0][0][0])
	next := getNewHyperSpace(hw, z, h, w)
	for i := range current {
		for j := range current[i] {
			for k := range current[i][j] {
				for l := range current[i][j][k] {
					adj := hyperAdjacent(i, j, k, l, current)
					sum := 0
					for _, v := range adj {
						sum += v
					}

					if sum == 3 {
						next[i][j][k][l] = 1
					} else if sum == 2 && current[i][j][k][l] == 1 {
						next[i][j][k][l] = 1
					} else {
						next[i][j][k][l] = 0
					}

					//fmt.Printf("(%d,%d,%d): %d ; adj: %d -> %d\n", i, j, k, current[i][j][k][l], len(adj), next[i][j][k][l])
				}
			}
		}
	}
	return next
}

func adjacent(z, r, c int, space [][][]int) []int {
	adj := make([]int, 0)
	for k := z - 1; k < z+2; k++ {
		if k >= 0 && k < len(space) {
			for i := r - 1; i < r+2; i++ {
				if i >= 0 && i < len(space[k]) {
					for j := c - 1; j < c+2; j++ {
						if !(j < 0 || j >= len(space[k][i])) {
							if !(i == r && j == c && k == z) {
								r := space[k][i][j]
								adj = append(adj, r)
							}
						}
					}
				}
			}
		}
	}
	return adj
}

func hyperAdjacent(w, z, r, c int, space [][][][]int) []int {
	adj := make([]int, 0)
	for l := w - 1; l < w+2; l++ {
		if l >= 0 && l < len(space) {
			for k := z - 1; k < z+2; k++ {
				if k >= 0 && k < len(space[l]) {
					for i := r - 1; i < r+2; i++ {
						if i >= 0 && i < len(space[l][k]) {
							for j := c - 1; j < c+2; j++ {
								if !(j < 0 || j >= len(space[l][k][i])) {
									if !(i == r && j == c && k == z && l == w) {
										r := space[l][k][i][j]
										adj = append(adj, r)
									}
								}
							}
						}
					}
				}
			}
		}
	}
	return adj
}

func sumSpace(space [][][]int) int {
	sum := 0
	for i := range space {
		for j := range space[i] {
			for k := range space[i][j] {
				sum += space[i][j][k]
			}
		}
	}
	return sum
}

func sumHyperSpace(space [][][][]int) int {
	sum := 0
	for i := range space {
		for j := range space[i] {
			for k := range space[i][j] {
				for l := range space[i][j][k] {
					sum += space[i][j][k][l]
				}
			}
		}
	}
	return sum
}

func getNewSpace(z, r, c int) [][][]int {
	//z , row, col
	space := make([][][]int, z)
	for i := range space {
		space[i] = make([][]int, r)
		for j := range space[i] {
			space[i][j] = make([]int, c)
		}
	}
	return space
}

func getNewHyperSpace(w, z, r, c int) [][][][]int {
	//z , row, col
	space := make([][][][]int, w)
	for i := range space {
		space[i] = make([][][]int, z)
		for j := range space[i] {
			space[i][j] = make([][]int, r)
			for k := range space[i][j] {
				space[i][j][k] = make([]int, c)
			}
		}
	}
	return space
}
