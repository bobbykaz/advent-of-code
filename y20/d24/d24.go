package d24

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d24.txt"

var Print = true

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	hexGrid := make([][]int, 1000)
	for i := range hexGrid {
		hexGrid[i] = make([]int, 1000)
	}

	cmds := make([][]string, len(input))
	for i, s := range input {
		newCmds := parseCommands(s)
		cmds[i] = newCmds
	}
	log("Total commands: %d \n", len(cmds))
	for _, c := range cmds {
		hexGrid = placeTiles(hexGrid, c)
	}
	for i := 0; i < 100; i++ {
		ng := copyHexGrid(hexGrid)
		for r := 1; r < len(hexGrid)-1; r++ {
			for q := 1; q < len(hexGrid[r])-1; q++ {
				sum := sumAdjacent(q, r, hexGrid)
				current := hexGrid[r][q]
				if current == 0 {
					if sum == 2 {
						ng[r][q] = 1
					}
				} else {
					if sum == 0 || sum > 2 {
						ng[r][q] = 0
					}
				}
			}
		}
		hexGrid = ng
	}

	//printHex(hexGrid)

	bt := 0
	for _, r := range hexGrid {
		for _, c := range r {
			bt += c
		}
	}

	log("%d\n", bt)
	return bt
}

func placeTiles(hg [][]int, cmds []string) [][]int {
	q, r := len(hg)/2, len(hg)/2
	for i, c := range cmds {
		q, r = nextLoc(q, r, c)
		log("...%d: %s to %d,%d\n", i, c, r, q)
	}

	hg[r][q] = (hg[r][q] + 1) % 2
	log("...flipping %d,%d -> %d\n", r, q, hg[r][q])

	return hg
}

func sumAdjacent(q, r int, hg [][]int) int {
	sum := 0
	if q == 0 || r == 0 || q == len(hg)-1 || r == len(hg)-1 {
		panic("hitting boundries of grid")
	}
	if r%2 == 0 {
		sum += hg[r][q+1]   //E
		sum += hg[r][q-1]   //W
		sum += hg[r-1][q]   //NE
		sum += hg[r-1][q-1] //NW
		sum += hg[r+1][q]   //SE
		sum += hg[r+1][q-1] //SW
	} else {
		sum += hg[r][q+1]   //E
		sum += hg[r][q-1]   //W
		sum += hg[r-1][q+1] //NE
		sum += hg[r-1][q]   //NW
		sum += hg[r+1][q+1] //SE
		sum += hg[r+1][q]   //SW
	}

	return sum
}

func copyHexGrid(hg [][]int) [][]int {
	nhg := make([][]int, len(hg))
	for i := range nhg {
		nhg[i] = make([]int, len(hg[i]))
		copy(nhg[i], hg[i])
	}
	return nhg
}

//returns Column,Row - NOT R,C
func nextLoc(q, r int, cmd string) (int, int) {
	switch cmd {
	case "e":
		return q + 1, r
	case "w":
		return q - 1, r
	case "ne":
		if r%2 == 0 {
			return q, r - 1
		}
		return q + 1, r - 1
	case "nw":
		if r%2 == 0 {
			return q - 1, r - 1
		}
		return q, r - 1
	case "se":
		if r%2 == 0 {
			return q, r + 1
		}
		return q + 1, r + 1
	case "sw":
		if r%2 == 0 {
			return q - 1, r + 1
		}
		return q, r + 1
	}
	panic(fmt.Sprintf("invalid command %s", cmd))
}

func parseCommands(str string) []string {
	cmds := make([]string, 0)
	i := 0
	for i < len(str) {
		r := rune(str[i])
		switch r {
		case 'e':
			cmds = append(cmds, "e")
			i++
		case 'w':
			cmds = append(cmds, "w")
			i++
		case 's':
			cmds = append(cmds, str[i:i+2])
			i += 2
		case 'n':
			cmds = append(cmds, str[i:i+2])
			i += 2
		}
	}
	//fmt.Println(cmds)
	return cmds
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}

func printHex(hg [][]int) {
	for i := range hg {
		for j := range hg[i] {
			if i%2 == 0 {
				if hg[i][j] == 1 {
					fmt.Printf(" o")
				} else {
					fmt.Printf(" .")
				}
			} else {
				if hg[i][j] == 1 {
					fmt.Printf("o ")
				} else {
					fmt.Printf(". ")
				}
			}
		}
		fmt.Printf("\n")
	}
}
