package d5

import "testing"

func TestSeatID_Min(t *testing.T) {
	var input = "FFFFFFFLLL"
	var actual = 0
	expected := SeatID(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

func TestSeatID_Max(t *testing.T) {
	var input = "BBBBBBBRRR"
	var actual = 8*128 - 1
	expected := SeatID(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

func TestSeatID_1(t *testing.T) {
	var input = "BFFFBBFRRR"
	var actual = 567
	expected := SeatID(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

func TestSeatID_2(t *testing.T) {
	var input = "FFFBBBFRRR"
	var actual = 119
	expected := SeatID(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

func TestSeatID_3(t *testing.T) {
	var input = "BBFFBBFRLL"
	var actual = 820
	expected := SeatID(input)
	if !(actual == expected) {
		t.Fatalf("failed")
	}
}

/*
BFFFBBFRRR: row 70, column 7, seat ID 567.
FFFBBBFRRR: row 14, column 7, seat ID 119.
BBFFBBFRLL: row 102, column 4, seat ID 820.
*/
