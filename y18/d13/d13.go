package d13

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	fmt.Println("test")
	input := utilities.ReadFileIntoLines("input/input13.txt")
	grid := make([][]rune, len(input))

	fmt.Println("parsing grid")
	for i := 0; i < len(input); i++ {
		grid[i] = make([]rune, len(input[i]))
		for j := 0; j < len(input[i]); j++ {
			grid[i][j] = rune(input[i][j])
		}
	}

	printGrid(grid)

	fmt.Println("parsing players")
	playerID := 0
	players := make([]player, 0)
	for i := 0; i < len(grid); i++ {
		for j := 0; j < len(grid[i]); j++ {
			current := grid[i][j]
			switch current {
			case '<':
				players = append(players, player{ID: playerID, Row: i, Col: j, Dir: dirLeft, Xing: 0})
				grid[i][j] = '-'
				playerID++
				break
			case '>':
				players = append(players, player{ID: playerID, Row: i, Col: j, Dir: dirRight, Xing: 0})
				grid[i][j] = '-'
				playerID++
				break
			case '^':
				players = append(players, player{ID: playerID, Row: i, Col: j, Dir: dirUp, Xing: 0})
				grid[i][j] = '|'
				playerID++
				break
			case 'v':
				players = append(players, player{ID: playerID, Row: i, Col: j, Dir: dirDown, Xing: 0})
				grid[i][j] = '|'
				playerID++
				break
			}
		}
	}

	printGrid(grid)
	fmt.Println("Testing Grid and players")
	printGridAndPlayers(grid, players)
	gameLoop(grid, players)
}

func gameLoop(grid [][]rune, players []player) {
	nocrash := true
	for i := 0; nocrash; i++ {
		fmt.Printf("\x0c")
		message := ""
		sort.Slice(players, func(i int, j int) bool { return playerLess(players[i], players[j]) })
		for p := 0; p < len(players); p++ {
			players[p].move(grid)
			crashed, Y, X := checkCrash(players, players[p], p)
			if crashed {
				message += fmt.Sprintf("Crash at X: %d Y %d, ", X, Y)
				//nocrash = false
				players = removePlayersAt(players, Y, X)
				p = p - 2
				if p < -1 {
					p = -1
				}
			}

			if len(players) <= 1 {
				nocrash = false
			}
			//printGridAndPlayers(grid, players)
			//time.Sleep(5 * time.Second)
		}
		//printGridAndPlayers(grid, players)
		fmt.Printf("Tick %d complete, %s\n", i, message)
		if len(players) == 1 {
			fmt.Printf("Final player at %d, %d\n", players[0].Col, players[0].Row)
		}
		//time.Sleep(5 * time.Second)
	}
}

func removePlayersAt(players []player, row int, col int) []player {
	outputPlayers := make([]player, 0)
	for i := 0; i < len(players); i++ {
		p := players[i]
		if p.Row != row || p.Col != col {
			outputPlayers = append(outputPlayers, p)
		} else {
			fmt.Printf("Removing player %d at %d, %d\n", p.ID, col, row)
		}
	}
	return outputPlayers
}

func checkCrash(players []player, moved player, moveIndex int) (bool, int, int) {
	for i := 0; i < len(players); i++ {
		p1 := players[i]
		if i != moveIndex {
			if moved.Row == p1.Row && moved.Col == p1.Col {
				return true, moved.Row, moved.Col
			}
		}
	}
	return false, 0, 0
}
func playerLess(p1 player, p2 player) bool {
	if p1.Row < p2.Row {
		return true
	} else if p1.Row == p2.Row {
		return p1.Col < p2.Col
	} else {
		return false
	}
}
func printGrid(grid [][]rune) {
	for i := 0; i < len(grid); i++ {
		for j := 0; j < len(grid[i]); j++ {
			fmt.Printf("%c", grid[i][j])
		}
		fmt.Println()
	}
}
func printPlayer(p player) rune {
	c := '?'
	switch p.Dir {
	case dirDown:
		c = 'v'
		break
	case dirLeft:
		c = '<'
		break
	case dirRight:
		c = '>'
		break
	case dirUp:
		c = '^'
		break
	}
	return c
}
func printGridAndPlayers(grid [][]rune, players []player) {
	px := 0
	pi, pj := players[px].Row, players[px].Col
	for i := 0; i < len(grid); i++ {
		line := ""
		for j := 0; j < len(grid[i]); j++ {
			if i == pi && j == pj {
				line += string(printPlayer(players[px]))
				px++
				if px < len(players) {
					pi, pj = players[px].Row, players[px].Col
				} else {
					pi, pj = 0, 0
				}
			} else {
				line += string(grid[i][j])
			}
		}
		fmt.Println(line)
	}
}

//Xing: 0 -> 1 -> 2 -> 0
type player struct {
	ID   int
	Row  int
	Col  int
	Dir  int
	Xing int
}

//Moving: move to new square, then turn
func (p *player) move(grid [][]rune) {
	dir := "?"
	switch p.Dir {
	case dirLeft:
		dir = "LEFT"
		p.Col = p.Col - 1
		nextSpace := grid[p.Row][p.Col]
		if nextSpace == '/' {
			p.Dir = dirDown
		} else if nextSpace == '\\' {
			p.Dir = dirUp
		} else if nextSpace == '+' {
			switch p.Xing {
			case 0:
				p.Dir = dirDown
				break
			case 2:
				p.Dir = dirUp
				break
			}
			p.Xing = ((p.Xing + 1) % 3)
		}
		break
	case dirRight:
		dir = "RIGHT"
		p.Col = p.Col + 1
		nextSpace := grid[p.Row][p.Col]
		if nextSpace == '/' {
			p.Dir = dirUp
		} else if nextSpace == '\\' {
			p.Dir = dirDown
		} else if nextSpace == '+' {
			switch p.Xing {
			case 0:
				p.Dir = dirUp
				break
			case 2:
				p.Dir = dirDown
				break
			}
			p.Xing = ((p.Xing + 1) % 3)
		}
		break
	case dirUp:
		dir = "UP"
		p.Row = p.Row - 1
		nextSpace := grid[p.Row][p.Col]
		if nextSpace == '/' {
			p.Dir = dirRight
		} else if nextSpace == '\\' {
			p.Dir = dirLeft
		} else if nextSpace == '+' {
			switch p.Xing {
			case 0:
				p.Dir = dirLeft
				break
			case 2:
				p.Dir = dirRight
				break
			}
			p.Xing = ((p.Xing + 1) % 3)
		}
		break
	case dirDown:
		dir = "DOWN"
		p.Row = p.Row + 1
		nextSpace := grid[p.Row][p.Col]
		if nextSpace == '/' {
			p.Dir = dirLeft
		} else if nextSpace == '\\' {
			p.Dir = dirRight
		} else if nextSpace == '+' {
			switch p.Xing {
			case 0:
				p.Dir = dirRight
				break
			case 2:
				p.Dir = dirLeft
				break
			}
			p.Xing = ((p.Xing + 1) % 3)
		}
		break
	}
	fmt.Printf("Player %d moved %s to %d, %d\n", p.ID, dir, p.Col, p.Row)
}

var dirLeft = 0
var dirUp = 1
var dirRight = 2
var dirDown = 3
