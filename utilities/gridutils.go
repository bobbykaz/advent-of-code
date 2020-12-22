package utilities

import "fmt"

type Grid struct {
	G      [][]rune
	Width  int
	Height int
}

type GridCell struct {
	V   rune
	Row int
	Col int
}

func StringsToGrid(input []string) Grid {
	g := Grid{}
	g.G = make([][]rune, len(input))
	for i := 0; i < len(input); i++ {
		g.G[i] = make([]rune, len(input[i]))
		for j := 0; j < len(input[i]); j++ {
			g.G[i][j] = rune(input[i][j])
		}
	}

	g.Height = len(input)
	g.Width = len(input[0])

	return g
}

//RotateGrid rotates the grid by 90 degrees * 'rotations' clockwise
func (g *Grid) RotateGrid(rotations int) {
	if g.Height != g.Width {
		panic("Didnt implement for non-square grids")
	}

	for x := 0; x < rotations; x++ {
		ng := make([][]rune, g.Width)
		for i := range ng {
			ng[i] = make([]rune, g.Width)
			copy(ng[i], g.G[i])
		}

		for i := 0; i < g.Height; i++ {
			for j := 0; j < g.Width; j++ {
				ng[j][g.Height-i-1] = g.G[i][j]
			}
		}

		g.G = ng
	}
}

func (g *Grid) FlipGridHorizontal() {
	ng := make([][]rune, g.Width)
	for i := range ng {
		ng[i] = make([]rune, g.Width)
		copy(ng[i], g.G[i])
	}

	for i := 0; i < g.Height; i++ {
		for j := 0; j < g.Width; j++ {
			ng[i][g.Width-j-1] = g.G[i][j]
		}
	}

	g.G = ng
}

func (g *Grid) FlipGridVertical() {
	ng := make([][]rune, g.Width)
	for i := range ng {
		ng[i] = make([]rune, g.Width)
		copy(ng[i], g.G[i])
	}

	for i := 0; i < g.Height; i++ {
		for j := 0; j < g.Width; j++ {
			ng[g.Height-1-i][j] = g.G[i][j]
		}
	}

	g.G = ng
}

func (g *Grid) Adjacent(r, c int) []rune {
	adj := make([]rune, 0)
	for i := r - 1; i < r+2; i++ {
		if i >= 0 && i < len(g.G) {
			for j := c - 1; j < c+2; j++ {
				if !(j < 0 || j >= len(g.G[i])) {
					if !(i == r && j == c) {
						r := g.G[i][j]
						adj = append(adj, r)
					}
				}
			}
		}
	}
	return adj
}

func (g *Grid) LineOfSight(r, c int, ignore []rune) []GridCell {
	los := make([]GridCell, 0)
	//up
	for i := r - 1; i >= 0; i-- {
		if !RuneInSlice(g.G[i][c], ignore) {
			gc := GridCell{V: g.G[i][c], Row: i, Col: c}
			los = append(los, gc)
			break
		}
	}
	//up-right
	for x := 1; r-x >= 0 && c+x < g.Width; x++ {
		dr, dc := r-x, c+x
		if !RuneInSlice(g.G[dr][dc], ignore) {
			gc := GridCell{V: g.G[dr][dc], Row: dr, Col: dc}
			los = append(los, gc)
			break
		}
	}
	//right
	for j := c + 1; j < g.Width; j++ {
		if !RuneInSlice(g.G[r][j], ignore) {
			gc := GridCell{V: g.G[r][j], Row: r, Col: j}
			los = append(los, gc)
			break
		}
	}
	//down-right
	for x := 1; r+x < g.Height && c+x < g.Width; x++ {
		dr, dc := r+x, c+x
		if !RuneInSlice(g.G[dr][dc], ignore) {
			gc := GridCell{V: g.G[dr][dc], Row: dr, Col: dc}
			los = append(los, gc)
			break
		}
	}
	//down
	for i := r + 1; i < g.Height; i++ {
		if !RuneInSlice(g.G[i][c], ignore) {
			gc := GridCell{V: g.G[i][c], Row: i, Col: c}
			los = append(los, gc)
			break
		}
	}
	//down-left
	for x := 1; r+x < g.Height && c-x >= 0; x++ {
		dr, dc := r+x, c-x
		if !RuneInSlice(g.G[dr][dc], ignore) {
			gc := GridCell{V: g.G[dr][dc], Row: dr, Col: dc}
			los = append(los, gc)
			break
		}
	}

	//left
	for j := c - 1; j >= 0; j-- {
		if !RuneInSlice(g.G[r][j], ignore) {
			gc := GridCell{V: g.G[r][j], Row: r, Col: j}
			los = append(los, gc)
			break
		}
	}
	//up-left
	for x := 1; r-x >= 0 && c-x >= 0; x++ {
		dr, dc := r-x, c-x
		if !RuneInSlice(g.G[dr][dc], ignore) {
			gc := GridCell{V: g.G[dr][dc], Row: dr, Col: dc}
			los = append(los, gc)
			break
		}
	}

	return los
}

type Edge uint8

const (
	GridEdgeUp Edge = iota
	GridEdgeDown
	GridEdgeLeft
	GridEdgeRight
)

func (g *Grid) GetEdge(e Edge) []rune {
	switch e {
	case GridEdgeUp:
		return g.G[0]
	case GridEdgeDown:
		return g.G[g.Height-1]
	case GridEdgeLeft:
		l := make([]rune, g.Height)
		for i := 0; i < len(l); i++ {
			l[i] = g.G[i][0]
		}
		return l
	case GridEdgeRight:
		r := make([]rune, g.Height)
		for i := 0; i < len(r); i++ {
			r[i] = g.G[i][g.Width-1]
		}
		return r
	}

	panic("specified invalid edge for Grid")
}

func RuneInSlice(r rune, rs []rune) bool {
	for _, v := range rs {
		if v == r {
			return true
		}
	}
	return false
}

func RuneSliceEqual(a, b []rune) bool {
	if len(a) != len(b) {
		return false
	}

	for i := range a {
		if a[i] != b[i] {
			return false
		}
	}

	return true
}

//Equals returns true if two Grids are identical (same values in same cells)
func (g *Grid) Equals(other *Grid, print bool) bool {
	if g.Width != other.Width || g.Height != other.Height {
		return false
	}

	for i := 0; i < len(g.G); i++ {
		for j := 0; j < len(g.G[i]); j++ {
			if g.G[i][j] != other.G[i][j] {
				if print {
					fmt.Printf("Difference at (%d,%d) [%c / %c] ", i, i, g.G[i][j], other.G[i][j])
				}
				return false
			}
		}
	}

	if print {
		fmt.Printf("Same! ")
	}

	return true
}

//Print prints the Grid to the console
func (g *Grid) Print() {
	for _, s := range g.G {
		fmt.Printf("%s\n", string(s))
	}
}
