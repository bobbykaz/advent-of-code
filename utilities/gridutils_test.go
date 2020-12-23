package utilities

import (
	"fmt"
	"testing"
)

func Test_Grid_Adjacent(t *testing.T) {
	var input = []string{"123", "456", "789"}
	g := StringsToGrid(input)
	expected := g.Adjacent(1, 1)
	var actual = []rune{'1', '2', '3', '4', '6', '7', '8', '9'}

	if !(fmt.Sprintf("%v", actual) == fmt.Sprintf("%v", expected)) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_Grid_Adjacent_UL_Edge(t *testing.T) {
	var input = []string{"123", "456", "789"}
	g := StringsToGrid(input)
	expected := g.Adjacent(0, 0)
	var actual = []rune{'2', '4', '5'}

	if !(fmt.Sprintf("%v", actual) == fmt.Sprintf("%v", expected)) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_Grid_Adjacent_UR_Edge(t *testing.T) {
	var input = []string{"123", "456", "789"}
	g := StringsToGrid(input)
	expected := g.Adjacent(0, 2)
	var actual = []rune{'2', '5', '6'}

	if !(fmt.Sprintf("%v", actual) == fmt.Sprintf("%v", expected)) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_FlipHorizontal_Odd(t *testing.T) {
	var input = []string{"123", "456", "789"}
	g := StringsToGrid(input)
	g.FlipGridHorizontal()

	var ai = []string{"321", "654", "987"}
	ag := StringsToGrid(ai)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed, %v, %v", ag, g)
	}
}

func Test_FlipHorizontal_Even(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	g.FlipGridHorizontal()

	var ai = []string{"21", "43"}
	ag := StringsToGrid(ai)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed, %v, %v", ag, g)
	}
}

func Test_FlipVertical(t *testing.T) {
	var input = []string{"123", "456", "789"}
	g := StringsToGrid(input)
	g.FlipGridVertical()

	var ai = []string{"789", "456", "123"}
	ag := StringsToGrid(ai)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed, %v, %v", ag, g)
	}
}

func Test_FlipVertical_Even(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	g.FlipGridVertical()

	var ai = []string{"34", "12"}
	ag := StringsToGrid(ai)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed, %v, %v", ag, g)
	}
}

func Test_GetEdge_Up(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	expected := "12"
	actual := string(g.GetEdge(GridEdgeUp))

	if !(expected == actual) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_GetEdge_Down(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	expected := "34"
	actual := string(g.GetEdge(GridEdgeDown))

	if !(expected == actual) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_GetEdge_Left(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	expected := "13"
	actual := string(g.GetEdge(GridEdgeLeft))

	if !(expected == actual) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_GetEdge_Right(t *testing.T) {
	var input = []string{"12", "34"}
	g := StringsToGrid(input)
	expected := "24"
	actual := string(g.GetEdge(GridEdgeRight))

	if !(expected == actual) {
		t.Fatalf("failed, %v, %v", expected, actual)
	}
}

func Test_Rotate_1(t *testing.T) {
	var input = []string{"1234", "5678", "90AB", "CDEF"}
	g := StringsToGrid(input)
	var ei = []string{"C951", "D062", "EA73", "FB84"}
	ag := StringsToGrid(ei)

	g.RotateGrid(1)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed, %v, %v", g, ag)
	}
}

func Test_Rotate_2(t *testing.T) {
	var input = []string{"1234", "5678", "90AB", "CDEF"}
	g := StringsToGrid(input)
	var ei = []string{"FEDC", "BA09", "8765", "4321"}
	ag := StringsToGrid(ei)

	g.RotateGrid(2)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed\n Actual:%v\n, Expected: %v\n", g, ag)
	}
}

func Test_Rotate_3(t *testing.T) {
	var input = []string{"1234", "5678", "90AB", "CDEF"}
	g := StringsToGrid(input)
	var ei = []string{"48BF", "37AE", "260D", "159C"}
	ag := StringsToGrid(ei)

	g.RotateGrid(3)

	if !(g.Equals(&ag, false)) {
		t.Fatalf("failed\n Actual:%v\n, Expected: %v\n", g, ag)
	}
}

func Test_GetCompositeGrid_1(t *testing.T) {
	var input = []string{"1234", "5678", "90AB", "CDEF"}
	ag := StringsToGrid(input)

	ng := GetCompositeGrid([][]Grid{[]Grid{ag}}, 1, false)

	var ei = []string{"67", "0A"}
	eg := StringsToGrid(ei)

	if !(ng.Equals(&eg, false)) {
		t.Fatalf("failed\n Actual:%v\n, Expected: %v\n", ng, eg)
	}
}
func Test_GetCompositeGrid_2(t *testing.T) {
	var input = []string{"123", "456", "789"}
	ag := StringsToGrid(input)

	gs := [][]Grid{[]Grid{ag, ag}, []Grid{ag, ag}}
	ng := GetCompositeGrid(gs, 1, false)

	var ei = []string{"55", "55"}
	eg := StringsToGrid(ei)

	if !(ng.Equals(&eg, false)) {
		t.Fatalf("failed\n Actual:%v\n, Expected: %v\n", ng, eg)
	}
}

func Test_GetCompositeGrid_3(t *testing.T) {
	var input = []string{"123", "456", "789"}
	ag := StringsToGrid(input)

	gs := [][]Grid{[]Grid{ag, ag}, []Grid{ag, ag}}
	ng := GetCompositeGrid(gs, 0, false)

	var ei = []string{"123123", "456456", "789789", "123123", "456456", "789789"}
	eg := StringsToGrid(ei)

	if !(ng.Equals(&eg, false)) {
		t.Fatalf("failed\n Actual:%v\n, Expected: %v\n", ng, eg)
	}
}
