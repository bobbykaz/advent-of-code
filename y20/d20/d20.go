package d20

import (
	"fmt"
	"strconv"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d20.txt"

var Print = true

type tile struct {
	ID    int
	G     utilities.Grid
	Up    *tile
	Down  *tile
	Left  *tile
	Right *tile
}

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	blocks := utilities.GroupLinesByLineSeparator(input, "")
	tileMap := make(map[int]*tile)
	for _, b := range blocks {
		t := parseTile(b)
		tileMap[t.ID] = &t
	}

	iter := 1
	anySolve := true
	fmt.Println("Basic swapping")
	//========================
	for !mapSolved(tileMap) && anySolve {
		anySolve = false
		for _, v := range tileMap {
			for _, v2 := range tileMap {
				if v.ID != v2.ID {
					b := compareAndMatchTiles(v, v2)
					if b {
						anySolve = b
					}
				}
			}
		}
		fmt.Println("flipping")
		for _, v := range tileMap {
			if !v.isSolved() {
				if v.unSolvedEdges() > 1 {
					anyflip := v.basicFlipIfNeeded()
					if anyflip {
						break
					}
				}
			}
		}

		fmt.Println("Re-comparing", iter)
		iter++
	}

	fmt.Println("Swap pairs")

	anySolve = true
	for !mapSolved(tileMap) && anySolve {
		fmt.Println("Flip Special!")
		for _, v := range tileMap {
			if !v.isSolved() {
				if v.unSolvedEdges() > 1 {
					anyflip := v.complexFlipIfNeeded()
					if anyflip {
						break
					}
				}
			}
		}

		anySolve = false
		for _, v := range tileMap {
			for _, v2 := range tileMap {
				if v.ID != v2.ID {
					b := compareAndMatchTiles(v, v2)
					if b {
						anySolve = b
					}
				}
			}
		}

		fmt.Println("Re-comparing", iter)
		iter++
	}

	//
	p := int64(1)
	for _, v := range tileMap {

		if v.unSolvedEdges() >= 2 {
			p *= int64(v.ID)
			v.print()
			fmt.Printf("Tile %d: open edges: %d\n", v.ID, v.unSolvedEdges())
		}
	}

	fmt.Println("Total:", p)
	return -1
}

func mapSolved(tileMap map[int]*tile) bool {
	twoPlus := 0
	for _, v := range tileMap {
		if v.unSolvedEdges() >= 2 {
			twoPlus++
		}
	}

	return twoPlus == 4
}

func compareAndMatchTiles(t1 *tile, t2 *tile) bool {
	anyChange := false
	if t1.Up == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeUp)

		c1 := t2.G.GetEdge(utilities.GridEdgeUp)
		c2 := t2.G.GetEdge(utilities.GridEdgeDown)
		c3 := t2.G.GetEdge(utilities.GridEdgeLeft)
		c4 := t2.G.GetEdge(utilities.GridEdgeRight)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Up = t2
			t2.Up = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c2) {
			t1.Up = t2
			t2.Down = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c3) {
			t1.Up = t2
			t2.Left = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c4) {
			t1.Up = t2
			t2.Right = t1
			anyChange = true
		}
	}

	if t1.Down == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeDown)
		c1 := t2.G.GetEdge(utilities.GridEdgeUp)
		c2 := t2.G.GetEdge(utilities.GridEdgeDown)
		c3 := t2.G.GetEdge(utilities.GridEdgeLeft)
		c4 := t2.G.GetEdge(utilities.GridEdgeRight)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Down = t2
			t2.Up = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c2) {
			t1.Down = t2
			t2.Down = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c3) {
			t1.Down = t2
			t2.Left = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c4) {
			t1.Down = t2
			t2.Right = t1
			anyChange = true
		}
	}

	if t1.Left == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeLeft)
		c1 := t2.G.GetEdge(utilities.GridEdgeUp)
		c2 := t2.G.GetEdge(utilities.GridEdgeDown)
		c3 := t2.G.GetEdge(utilities.GridEdgeLeft)
		c4 := t2.G.GetEdge(utilities.GridEdgeRight)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Left = t2
			t2.Up = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c2) {
			t1.Left = t2
			t2.Down = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c3) {
			t1.Left = t2
			t2.Left = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c4) {
			t1.Left = t2
			t2.Right = t1
			anyChange = true
		}
	}

	if t1.Right == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeRight)
		c1 := t2.G.GetEdge(utilities.GridEdgeUp)
		c2 := t2.G.GetEdge(utilities.GridEdgeDown)
		c3 := t2.G.GetEdge(utilities.GridEdgeLeft)
		c4 := t2.G.GetEdge(utilities.GridEdgeRight)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Right = t2
			t2.Up = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c2) {
			t1.Right = t2
			t2.Down = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c3) {
			t1.Right = t2
			t2.Left = t1
			anyChange = true
		} else if utilities.RuneSliceEqual(e1, c4) {
			t1.Right = t2
			t2.Right = t1
			anyChange = true
		}
	}
	return anyChange
}

func (t *tile) unSolvedEdges() int {
	i := 0
	if t.Up == nil {
		i++
	}

	if t.Down == nil {
		i++
	}

	if t.Left == nil {
		i++
	}

	if t.Right == nil {
		i++
	}
	return i
}

func (t *tile) isSolved() bool {
	return (t.Up != nil && t.Down != nil && t.Left != nil && t.Right != nil)
}

func (t *tile) basicFlipIfNeeded() bool {
	anyflip := false
	t.print()
	if t.Up == nil && t.Down == nil {
		t.FlipHoriz()
		anyflip = true
	}

	if t.Left == nil && t.Right == nil {
		t.FlipVert()
		anyflip = true
	}

	return anyflip
}

func (t *tile) complexFlipIfNeeded() bool {
	anyflip := false
	t.print()
	other := (*tile)(nil)

	if t.Up != nil && t.Up.unSolvedEdges() > 1 {
		other = t.Up
		t.FlipVert()
		anyflip = true
	} else if t.Down != nil && t.Down.unSolvedEdges() > 1 {
		other = t.Down
		t.FlipVert()
		anyflip = true
	} else if t.Left != nil && t.Left.unSolvedEdges() > 1 {
		other = t.Left
		t.FlipHoriz()
		anyflip = true
	} else if t.Right != nil && t.Right.unSolvedEdges() > 1 {
		other = t.Right
		t.FlipHoriz()
		anyflip = true
	}

	if anyflip {
		log("Other: ")
		other.print()
		if other.Up == t || other.Down == t {
			other.FlipVert()
		} else {
			other.FlipHoriz()
		}
	}

	return anyflip
}

func (t *tile) FlipHoriz() {
	t.G.FlipGridHorizontal()
	log("...Flipping H\n")
	tmp := t.Left
	t.Left = t.Right
	t.Right = tmp
}

func (t *tile) FlipVert() {
	t.G.FlipGridVertical()
	fmt.Println("...Flipping V")
	tmp := t.Down
	t.Down = t.Up
	t.Up = tmp
}

func (t *tile) print() {
	u := "(...)"
	if t.Up != nil {
		u = fmt.Sprintf("%d", t.Up.ID)
	}
	d := "(...)"
	if t.Down != nil {
		d = fmt.Sprintf("%d", t.Down.ID)
	}
	l := "(...)"
	if t.Left != nil {
		l = fmt.Sprintf("%d", t.Left.ID)
	}
	r := "(...)"
	if t.Right != nil {
		r = fmt.Sprintf("%d", t.Right.ID)
	}
	log("ID %d: ^^%s^^   vv%svv   <<%s<<   >>%s>>\n", t.ID, u, d, l, r)
}

func parseTile(strs []string) tile {
	//Tile 1234:
	idStr := strs[0][5 : len(strs[0])-1]
	id, _ := strconv.Atoi(idStr)
	g := utilities.StringsToGrid(strs[1:])
	t := tile{ID: id, G: g}
	return t
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
