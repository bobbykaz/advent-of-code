package d15

import "testing"

func TestBasic(t *testing.T) {
	var result = playGame("../input/sample15.txt", false, 3)
	var expected = 27730
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test1(t *testing.T) {
	var result = playGame("../input/sample15-1.txt", false, 3)
	var expected = 36334
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test2(t *testing.T) {
	var result = playGame("../input/sample15-2.txt", false, 3)
	var expected = 39514
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test3(t *testing.T) {
	var result = playGame("../input/sample15-3.txt", false, 3)
	var expected = 27755
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test4(t *testing.T) {
	var result = playGame("../input/sample15-4.txt", false, 3)
	var expected = 28944
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test5(t *testing.T) {
	var result = playGame("../input/sample15-5.txt", false, 3)
	var expected = 18740
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}
