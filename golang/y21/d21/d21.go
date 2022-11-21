package d21

import "fmt"

func Run() int {
	players := make([]int, 2)
	scores := make([]int, 2)
	players[0] = 7
	players[1] = 2
	die := 0
	rolls := 0
	p1Turn := true
	for scores[0] < 1000 && scores[1] < 1000 {
		p := 0
		if !p1Turn {
			p = 1
		}

		d1 := rollDet(die)
		d2 := rollDet(d1)
		d3 := rollDet(d2)
		die = d3
		rolls += 3
		move := (d1 + d2 + d3) % 10
		players[p] += move
		if players[p] > 10 {
			players[p] = players[p] % 10
		}
		scores[p] += players[p]
		fmt.Println("player", (p + 1), "rolled", d1, d2, d3, "moves to", players[p], "for total score", scores[p])
		p1Turn = !p1Turn
	}
	fmt.Println("Player 1", scores[0])
	fmt.Println("Player 2", scores[1])
	fmt.Println("Rolls", rolls, "Ending die", die)
	fmt.Println("p1", scores[0]*rolls, "p2", scores[1]*rolls)
	fmt.Println("Starting multiverse")
	multiverse()
	return 0
}

func rollDet(d int) int {
	if d > 100 {
		return (d % 100) + 1
	}
	return d + 1
}

func multiverse() {
	history := make([]map[string]*gstate, 1)
	history[0] = make(map[string]*gstate)
	init := &gstate{p1: 7, p2: 2, s1: 0, s2: 0, hits: 1}
	diceRolls := diceRolls()
	history[0][init.key()] = init
	p1w, p2w := int64(0), int64(0)
	index := 0
	for len(history[index]) > 0 {
		nextRound := make(map[string]*gstate)
		fmt.Println("Starting round", index, "with", len(history[index]), "initial states")
		for _, gPtr := range history[index] {
			//for each possible gamestate, calculate all possible new states
			g := *gPtr
			//whose turn?
			if index%2 == 0 { // P1
				for _, r := range diceRolls {
					move := r % 10
					nP1 := g.p1 + move
					if nP1 > 10 {
						nP1 = nP1 % 10
					}
					nS1 := g.s1 + nP1
					if nS1 >= 21 {
						p1w += g.hits
					} else {
						nG := &gstate{p1: nP1, p2: g.p2, s1: nS1, s2: g.s2, hits: g.hits}
						_, exists := nextRound[nG.key()]
						if exists {
							nextRound[nG.key()].hits += g.hits
						} else {
							nextRound[nG.key()] = nG
						}
					}
				}
			} else { //P2
				for _, r := range diceRolls {
					move := r % 10
					nP2 := g.p2 + move
					if nP2 > 10 {
						nP2 = nP2 % 10
					}
					nS2 := g.s2 + nP2
					if nS2 >= 21 {
						p2w += g.hits
					} else {
						nG := &gstate{p1: g.p1, p2: nP2, s1: g.s1, s2: nS2, hits: g.hits}
						_, exists := nextRound[nG.key()]
						if exists {
							nextRound[nG.key()].hits += g.hits
						} else {
							nextRound[nG.key()] = nG
						}
					}
				}
			}
		}
		history = append(history, nextRound)
		index++
	}
	fmt.Println("Player 1 wins", p1w)
	fmt.Println("Player 2 wins", p2w)
}

func diceRolls() []int {
	result := make([]int, 0)
	for d1 := 1; d1 <= 3; d1++ {
		for d2 := 1; d2 <= 3; d2++ {
			for d3 := 1; d3 <= 3; d3++ {
				result = append(result, d1+d2+d3)
			}
		}
	}
	return result
}

func (g *gstate) key() string {
	return fmt.Sprintf("%d-%d-%d-%d", g.p1, g.p2, g.s1, g.s2)
}

type gstate struct {
	p1   int
	p2   int
	s1   int
	s2   int
	hits int64
}
