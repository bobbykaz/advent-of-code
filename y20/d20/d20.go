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
	tileQueue := make([]*tile, 0)
	//for _, v := range tileMap {
	//	tileQueue = append(tileQueue, v)
	//	break
	//}

	tileQueue = append(tileQueue, tileMap[3557])

	fmt.Println("Basic swapping")
	//========================
	for !mapSolved(tileMap) && len(tileQueue) > 0 {
		next := tileQueue[0]
		tileQueue = tileQueue[1:]
		newTiles := compareAndTransformOthertiles(next, &tileMap)
		tileQueue = append(tileQueue, newTiles...)

		fmt.Println("Next loop", iter)
		iter++
	}

	//end result
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

func compareAndTransformOthertiles(t *tile, tileMap *map[int]*tile) []*tile {
	log("Checking tile %d \n", t.ID)
	next := make([]*tile, 0)
	for _, v := range *tileMap {
		log(".. comparing to %d\n", v.ID)
		Print = false
		mf, md := checkAllRotations(t, v)

		if !mf && v.unSolvedEdges() == 4 {
			//log(".. flipping Horizontal\n")
			v.FlipHoriz()
			mf, md = checkAllRotations(t, v)
		}

		if !mf && v.unSolvedEdges() == 4 {
			//log(".. flipping Vertical\n")
			v.FlipVert()
			mf, md = checkAllRotations(t, v)
		}

		if !mf && v.unSolvedEdges() == 4 {
			//log(".. unflipping Horizontal\n")
			v.FlipHoriz()
			mf, md = checkAllRotations(t, v)
		}
		Print = true
		if mf {
			next = append(next, v)
			log(". match found with edge %d\n", md)
		}
	}
	return next
}

func checkAllRotations(t1, t2 *tile) (bool, utilities.Edge) {
	count := 0
	mf, md := compareAndMatchTile(t1, t2)
	for !mf && count != 3 && t2.unSolvedEdges() == 4 {
		t2.G.RotateGrid(1)
		mf, md = compareAndMatchTile(t1, t2)
		count++
	}
	return mf, md
}

func compareAndMatchTile(t1, t2 *tile) (bool, utilities.Edge) {
	if t1.Up == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeUp)
		c1 := t2.G.GetEdge(utilities.GridEdgeDown)
		log("Comparing Up: %d -> %d\n", t1.ID, t2.ID)
		if utilities.RuneSliceEqual(e1, c1) {
			log("Match\n")
			t1.Up = t2
			t2.Down = t1
			return true, utilities.GridEdgeUp
		}
	}

	if t1.Down == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeDown)
		c1 := t2.G.GetEdge(utilities.GridEdgeUp)
		log("Comparing Down: %d -> %d\n", t1.ID, t2.ID)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Down = t2
			t2.Up = t1
			return true, utilities.GridEdgeDown
		}
	}

	if t1.Left == nil {
		e1 := t1.G.GetEdge(utilities.GridEdgeLeft)
		c1 := t2.G.GetEdge(utilities.GridEdgeRight)
		log("Comparing Left: %d -> %d\n", t1.ID, t2.ID)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Left = t2
			t2.Right = t1
			return true, utilities.GridEdgeLeft
		}
	}

	if t1.Right == nil {
		log("Comparing Right: %d -> %d\n", t1.ID, t2.ID)
		e1 := t1.G.GetEdge(utilities.GridEdgeRight)
		c1 := t2.G.GetEdge(utilities.GridEdgeLeft)
		if utilities.RuneSliceEqual(e1, c1) {
			t1.Right = t2
			t2.Left = t1
			return true, utilities.GridEdgeRight
		}
	}
	return false, utilities.GridEdgeRight
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

func (t *tile) FlipHoriz() {
	t.G.FlipGridHorizontal()
	//log("...Flipping H\n")
	tmp := t.Left
	t.Left = t.Right
	t.Right = tmp
}

func (t *tile) FlipVert() {
	t.G.FlipGridVertical()
	//fmt.Println("...Flipping V")
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
