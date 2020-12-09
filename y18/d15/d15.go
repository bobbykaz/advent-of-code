package d15

import (
	"fmt"
	"math"

	"github.com/bobbykaz/aoc2018/utilities"
)

type gameboard struct {
	grid            [][]int
	actors          []actor
	rounds          int
	incompleteRound bool
}

type actor struct {
	HP   int
	Team string
	Atp  int
	ID   int
	Col  int
	Row  int
	Dead bool
}

type actorMove struct {
	actorID  int
	Position pos
}

type pos struct {
	Row int
	Col int
}

func Part1() {
	fmt.Println("Day 15")
	playGame("input/input15.txt", true, 19)
}

func playGame(filename string, printBoard bool, elfPower int) int {
	input := utilities.ReadFileIntoLines(filename)
	game := gameboard{grid: make([][]int, len(input)), actors: make([]actor, 0), rounds: 0, incompleteRound: false}
	initGame(&game, input, elfPower)
	for !game.gameDone() {
		if printBoard {
			game.print()
		}
		game.round()
	}

	fmt.Println("==========================")
	if game.incompleteRound {
		game.rounds--
	}
	if printBoard {
		game.print()
	}
	return game.gameStats()
}

const _Wall = -100
const _OpenSpace = -1

func initGame(game *gameboard, input []string, elfpower int) {
	nextID := 0
	elves := 0
	goblins := 0
	for i := 0; i < len(input); i++ {
		game.grid[i] = make([]int, len(input[i]))
		for j := 0; j < len(input[i]); j++ {
			current := input[i][j]
			switch current {
			case '#':
				game.grid[i][j] = _Wall
				break
			case '.':
				game.grid[i][j] = _OpenSpace
				break
			case 'E':
				newActor := actor{HP: 200, Team: "E", Atp: elfpower, ID: nextID, Col: j, Row: i, Dead: false}
				elves++
				game.actors = append(game.actors, newActor)
				game.grid[i][j] = nextID
				nextID++
				break
			case 'G':
				newActor := actor{HP: 200, Team: "G", Atp: 3, ID: nextID, Col: j, Row: i, Dead: false}
				goblins++
				game.actors = append(game.actors, newActor)
				game.grid[i][j] = nextID
				nextID++
				break
			default:
				game.grid[i][j] = _Wall
			}
		}
	}

	fmt.Println("init game: Rows: ", len(input), ", Cols:", len(input[0]), ", TotalPlayers:", len(game.actors), ", Elves:", elves, ", Goblins:", goblins)
}

func (g *gameboard) print() {
	for i := 0; i < len(g.grid); i++ {
		for j := 0; j < len(g.grid[i]); j++ {
			current := g.grid[i][j]
			switch current {
			case _Wall:
				fmt.Print("#")
				break
			case _OpenSpace:
				fmt.Print(".")
				break
			default:
				fmt.Print(g.actors[current].Team)
			}
		}
		fmt.Println()
	}
}

func (g *gameboard) round() {
	//ID turn order
	order := g.determineTurnOrder()
	//for each actor
	for x := 0; x < len(order); x++ {
		if !g.actors[order[x].actorID].Dead {
			////identify targets
			targets := g.targets(order[x])
			////if not in range, move to closest, reading-order space
			inRange := false
			for _, loc := range targets {
				if loc.Col == order[x].Position.Col && loc.Row == order[x].Position.Row {
					inRange = true
					println("...In range, no move needed")
					break
				}
			}
			if !inRange {
				//move
				pathToBeInRange := g.pathToClosestSpots(order[x].Position, targets)
				//fmt.Printf("...Actor %d has path to target %v \n", order[x].actorID, pathToBeInRange)

				//pathToBeInRange always starts with current position
				if pathToBeInRange != nil {
					order[x].Position = pathToBeInRange[1]
					g.move(order[x].actorID, pathToBeInRange[1])
					if len(pathToBeInRange) == 2 {
						inRange = true
						println("...Moved In range")
					}
				}
			}
			//combat
			if inRange {
				g.attack(order[x].actorID)
			}
		}
	}

	//done
	g.rounds++
	println("Round ", g.rounds, "done")
}

func (g *gameboard) targets(mv actorMove) []pos {
	player := g.actors[mv.actorID]
	enemies := make([]*actor, 0)
	for i := 0; i < len(g.actors); i++ {
		if g.actors[i].Team != player.Team && !g.actors[i].Dead {
			enemies = append(enemies, &(g.actors[i]))
		}
	}

	if len(enemies) == 0 {
		println("Game over, round incomplete")
		g.incompleteRound = true
	}

	println(player.Team, player.ID, "at [", player.Row, player.Col, "] has enemies: ", enemies)

	result := make([]pos, 0)

	for _, a := range enemies {
		if g.grid[a.Row][a.Col-1] == _OpenSpace || g.grid[a.Row][a.Col-1] == mv.actorID {
			result = append(result, pos{Col: a.Col - 1, Row: a.Row})
		}
		if g.grid[a.Row][a.Col+1] == _OpenSpace || g.grid[a.Row][a.Col+1] == mv.actorID {
			result = append(result, pos{Col: a.Col + 1, Row: a.Row})
		}
		if g.grid[a.Row-1][a.Col] == _OpenSpace || g.grid[a.Row-1][a.Col] == mv.actorID {
			result = append(result, pos{Col: a.Col, Row: a.Row - 1})
		}
		if g.grid[a.Row+1][a.Col] == _OpenSpace || g.grid[a.Row+1][a.Col] == mv.actorID {
			result = append(result, pos{Col: a.Col, Row: a.Row + 1})
		}
	}
	println("...Target positions: ", result)
	return result
}

func (g *gameboard) move(mover int, destination pos) {
	actor := g.actors[mover]
	println("...", actor.Team, actor.ID, "moving to", destination)
	g.grid[actor.Row][actor.Col] = _OpenSpace
	g.actors[mover].Row = destination.Row
	g.actors[mover].Col = destination.Col
	g.grid[destination.Row][destination.Col] = mover
}

func (g *gameboard) attack(attacker int) {
	//Identify in-range target with lowest hp (reading order)
	attackerTeam := g.actors[attacker].Team
	row := g.actors[attacker].Row
	col := g.actors[attacker].Col
	lowestHP := math.MaxInt32
	target := _OpenSpace

	//top
	nextSpace := g.grid[row-1][col]
	target, lowestHP = g.attackCheckTarget(attackerTeam, nextSpace, target, lowestHP)

	//left
	nextSpace = g.grid[row][col-1]
	target, lowestHP = g.attackCheckTarget(attackerTeam, nextSpace, target, lowestHP)

	//right
	nextSpace = g.grid[row][col+1]
	target, lowestHP = g.attackCheckTarget(attackerTeam, nextSpace, target, lowestHP)

	//bottom
	nextSpace = g.grid[row+1][col]
	target, lowestHP = g.attackCheckTarget(attackerTeam, nextSpace, target, lowestHP)

	//attack
	g.attackDamage(attacker, target)
}

//Returns Target, HP
func (g *gameboard) attackCheckTarget(attackerTeam string, nextSpace int, currentTarget int, currentHpThreshold int) (int, int) {
	if nextSpace != _Wall && nextSpace != _OpenSpace {
		possibleTarget := g.actors[nextSpace]
		if !possibleTarget.Dead && possibleTarget.Team != attackerTeam && possibleTarget.HP < currentHpThreshold {
			return possibleTarget.ID, possibleTarget.HP
		} else {
			return currentTarget, currentHpThreshold
		}
	} else {
		return currentTarget, currentHpThreshold
	}
}

func (g *gameboard) attackDamage(attacker int, target int) {
	if target != _OpenSpace && target != _Wall {
		g.actors[target].HP -= g.actors[attacker].Atp
		println("...", attacker, "hit", target, "for", g.actors[attacker].Atp, "; remaining:", g.actors[target].HP)
		if g.actors[target].HP <= 0 {
			println("...", attacker, "killed", target)
			g.actors[target].Dead = true
			g.grid[g.actors[target].Row][g.actors[target].Col] = _OpenSpace
			g.actors[target].Row = -1
			g.actors[target].Col = -1
		}
	}
}

func (g *gameboard) determineTurnOrder() []actorMove {
	order := make([]actorMove, 0)
	for i := 0; i < len(g.grid); i++ {
		for j := 0; j < len(g.grid[i]); j++ {
			current := g.grid[i][j]
			switch current {
			case _Wall:
			case _OpenSpace:
				break
			default:
				order = append(order, actorMove{actorID: current, Position: pos{Row: i, Col: j}})
			}
		}
	}
	println("order", order)
	return order
}

func (g *gameboard) pathToClosestSpots(from pos, to []pos) []pos {
	var shortestLen = math.MaxInt32
	var currentTarget = pos{}
	var currentPath []pos
	for _, p := range to {
		nextPath := g.pathToClosestSpot(from, p)
		if nextPath != nil {
			if len(nextPath) < shortestLen {
				shortestLen = len(nextPath)
				currentTarget = p
				currentPath = nextPath
			} else if len(nextPath) == shortestLen {
				if p.Row < currentTarget.Row || (p.Row == currentTarget.Row && p.Col < currentTarget.Col) {
					shortestLen = len(nextPath)
					currentTarget = p
					currentPath = nextPath
				}
			}
		}
	}

	return currentPath
}

func (g *gameboard) pathToClosestSpot(from pos, to pos) []pos {

	takenPath := make([][]bool, len(g.grid))
	for i, v := range g.grid {
		takenPath[i] = make([]bool, len(v))
	}

	bfsq := make([]bfsPos, 0)
	bfsq = append(bfsq, bfsPos{Next: from, Path: make([]pos, 0)})
	takenPath[from.Row][from.Col] = true
	for len(bfsq) > 0 {
		current := bfsq[0]
		//fmt.Println("......Evaluating space", current.Next)
		bfsq = bfsq[1:]
		if current.Next.Row == to.Row && current.Next.Col == to.Col {
			//fmt.Println(".........at target!", current.Next)
			return append(current.Path, current.Next)
		}
		//reading order
		//up
		up := pos{Col: current.Next.Col, Row: current.Next.Row - 1}
		if !takenPath[up.Row][up.Col] {
			//fmt.Println("......adding up", up)
			bfsq = g.pathToClosestHelper(up, current, bfsq, &takenPath)
		}
		//left
		left := pos{Col: current.Next.Col - 1, Row: current.Next.Row}
		if !takenPath[left.Row][left.Col] {
			//fmt.Println("......adding left", left)
			bfsq = g.pathToClosestHelper(left, current, bfsq, &takenPath)
		}
		//right
		right := pos{Col: current.Next.Col + 1, Row: current.Next.Row}
		if !takenPath[right.Row][right.Col] {
			//fmt.Println("......adding right", right)
			bfsq = g.pathToClosestHelper(right, current, bfsq, &takenPath)
		}
		//down
		down := pos{Col: current.Next.Col, Row: current.Next.Row + 1}
		if !takenPath[down.Row][down.Col] {
			//fmt.Println("......adding down", down)
			bfsq = g.pathToClosestHelper(down, current, bfsq, &takenPath)
		}
	}

	return nil
}

func (g *gameboard) pathToClosestHelper(next pos, current bfsPos, bfsq []bfsPos, takenPath *[][]bool) []bfsPos {
	(*takenPath)[next.Row][next.Col] = true
	if g.grid[next.Row][next.Col] == _OpenSpace {
		newNode := newBfsNode(next, current)
		bfsq = append(bfsq, newNode)
	}
	return bfsq
}

func posInList(target pos, to []pos) bool {
	for _, p := range to {
		if target.Col == p.Col && target.Row == p.Row {
			return true
		}
	}

	return false
}

func newBfsNode(current pos, prev bfsPos) bfsPos {
	newBfs := bfsPos{Next: current, Path: append(prev.Path, prev.Next)}
	return newBfs
}

type bfsPos struct {
	Next pos
	Path []pos
}

func (g *gameboard) gameDone() bool {
	elf := false
	gob := false
	for _, a := range g.actors {
		if !a.Dead && a.Team == "G" {
			gob = true
		}
		if !a.Dead && a.Team == "E" {
			elf = true
		}
	}

	return !(gob && elf)
}

func (g *gameboard) gameStats() int {
	fmt.Println("Game done, total rounds", g.rounds)
	hpTotal := 0
	for _, a := range g.actors {
		if !a.Dead {
			hpTotal += a.HP
			println(a.Team, a.ID, "; HP:", a.HP)
		}
	}
	fmt.Println("Total HP", hpTotal)
	fmt.Println("Score", g.rounds, "*", hpTotal, "=", g.rounds*hpTotal)
	return g.rounds * hpTotal
}

func println(a ...interface{}) {
	//fmt.Println(a)
}
