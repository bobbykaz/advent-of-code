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
