package d13

import "testing"

func TestEX1(t *testing.T) {
	var input = "17,x,13,19"
	var actual = int64(3417)
	expected := p2(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

func TestEX2(t *testing.T) {
	var input = "3,x,x,4,5"
	var actual = int64(21)
	expected := p2(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}
