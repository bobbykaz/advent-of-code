package utilities

import "testing"

func TestStringsToInts(t *testing.T) {
	var input = []string{"0", "3", "0", "1", "-3"}
	var actual = []int{0, 3, 0, 1, -3}
	expected := StringsToInts(input)
	if !testEq(actual, expected) {
		t.Fatalf("failed")
	}
}

func TestIntsToString(t *testing.T) {
	var expected = "0,3,0,1,-3"
	var input = []int{0, 3, 0, 1, -3}
	actual := IntsToString(input)
	if actual != expected {
		t.Fatalf("failed: actual: %s, expected %s", actual, expected)
	}
}

func TestParseDateStyleString(t *testing.T) {
	e1, e2, e3 := 1999, 11, 10
	a1, a2, a3, aerr := ParseDateStyleString("    1999-11-10    ")
	if e1 != a1 {
		t.Fatalf("failed: actual: %d, expected %d", e1, a1)
	}

	if e2 != a2 {
		t.Fatalf("failed: actual: %d, expected %d", e2, a2)
	}

	if e3 != a3 {
		t.Fatalf("failed: actual: %d, expected %d", e3, a3)
	}

	if aerr != nil {
		t.Fatalf("failed: should have nil err")
	}
}

func TestParseTimeStyleString(t *testing.T) {
	e1, e2 := 0, 1000
	a1, a2, aerr := ParseTimeStyleString("    000:1000    ")
	if e1 != a1 {
		t.Fatalf("failed: actual: %d, expected %d", e1, a1)
	}

	if e2 != a2 {
		t.Fatalf("failed: actual: %d, expected %d", e2, a2)
	}

	if aerr != nil {
		t.Fatalf("failed: should have nil err")
	}
}

func testEq(a, b []int) bool {

	if a == nil && b == nil {
		return true
	}

	if a == nil || b == nil {
		return false
	}

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
