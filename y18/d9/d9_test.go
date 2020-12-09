package d9

import "testing"

//9 players; last marble is worth 32 points: high score is 32
func TestBasic(t *testing.T) {
	var result = playGame(9, 32)
	var expected = 32
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//10 players; last marble is worth 1618 points: high score is 8317
func Test10(t *testing.T) {
	var result = playGame(10, 1618)
	var expected = 8317
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//13 players; last marble is worth 7999 points: high score is 146373
func Test13(t *testing.T) {
	var result = playGame(13, 7999)
	var expected = 146373
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//17 players; last marble is worth 1104 points: high score is 2764
func Test17(t *testing.T) {
	var result = playGame(17, 1104)
	var expected = 2764
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//21 players; last marble is worth 6111 points: high score is 54718
func Test21(t *testing.T) {
	var result = playGame(21, 6111)
	var expected = 54718
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//30 players; last marble is worth 5807 points: high score is 37305
func Test30(t *testing.T) {
	var result = playGame(30, 5807)
	var expected = 37305
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

//428 players; last marble is worth 72061 points: high score?
func Test428(t *testing.T) {
	var result = playGame(428, 72061)
	var expected = 409832
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}

func Test428x100(t *testing.T) {
	var result = playGame(428, 72061*100)
	var expected = 3469562780
	if result != expected {
		t.Fatalf("Expected %d, got %d", expected, result)
	}
}
