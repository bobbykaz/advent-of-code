package d12

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	fmt.Println("test")
	init := "##.##..#.#....#.##...###.##.#.#..###.#....##.###.#..###...#.##.#...#.#####.###.##..#######.####..#"
	plants := make([]int, 0)
	buffer := 1000
	sbuff := 5
	for i := 0; i < sbuff; i++ {
		plants = append(plants, 0)
	}
	for i := 0; i < len(init); i++ {
		if init[i] == '#' {
			plants = append(plants, 1)
		} else {
			plants = append(plants, 0)
		}
	}
	for i := 0; i < buffer; i++ {
		plants = append(plants, 0)
	}

	input := utilities.ReadFileIntoLines("input/input12.txt")
	laws := generateLaws(input)

	for gen := 1; gen < 500; gen++ {
		fmt.Printf("Gen %d: ", gen)
		plants = stepGeneration(plants, laws)
		printPlants(plants)
		plantsum := 0
		for i := 0; i < len(plants); i++ {
			if plants[i] == 1 {
				plantsum += i
			}
		}
		//fmt.Println("plantsum", plantsum)
	}

	plantsum := 0
	for i := 0; i < len(plants); i++ {
		if plants[i] == 1 {
			plantsum += i - sbuff
		}
	}
	fmt.Println("plantsum", plantsum)
}

func printPlants(plants []int) {
	for i := 0; i < len(plants); i++ {
		if plants[i] == 1 {
			fmt.Printf("#")
		} else {
			fmt.Printf(".")
		}
	}
	fmt.Printf("\n")
}

func stepGeneration(plants []int, laws [][][][][]int) []int {
	newPlants := make([]int, len(plants))
	for i := 0; i < len(plants); i++ {
		L2, L1, R1, R2 := 0, 0, 0, 0
		if i-2 >= 0 {
			L2 = plants[i-2]
		}
		if i-1 >= 0 {
			L1 = plants[i-1]
		}
		if i+1 < len(plants) {
			R1 = plants[i+1]
		}
		if i+2 < len(plants) {
			R2 = plants[i+2]
		}
		newPlants[i] = laws[L2][L1][plants[i]][R1][R2]
	}

	return newPlants
}

func generateLaws(input []string) [][][][][]int {
	rules := make([][][][][]int, 2)
	for i := 0; i < 2; i++ {
		rules[i] = make([][][][]int, 2)
		for j := 0; j < 2; j++ {
			rules[i][j] = make([][][]int, 2)
			for k := 0; k < 2; k++ {
				rules[i][j][k] = make([][]int, 2)
				for l := 0; l < 2; l++ {
					rules[i][j][k][l] = make([]int, 2)
				}
			}
		}
	}

	for i := 0; i < len(input); i++ {
		parts := strings.Split(input[i], " => ")
		law := make([]int, 5)
		for j := 0; j < 5; j++ {
			if parts[0][j] == '#' {
				law[j] = 1
			} else {
				law[j] = 0
			}
		}
		if parts[1][0] == '#' {
			rules[law[0]][law[1]][law[2]][law[3]][law[4]] = 1
		} else {
			rules[law[0]][law[1]][law[2]][law[3]][law[4]] = 0
		}
	}

	return rules
}
