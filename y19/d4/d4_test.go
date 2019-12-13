package d4

import "testing"

func TestP1S1(t *testing.T) {
	var input = 111111
	var actual = true
	expected := isElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP1S2(t *testing.T) {
	var input = 223450
	var actual = false
	expected := isElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP1S3(t *testing.T) {
	var input = 123789
	var actual = false
	expected := isElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP2S1(t *testing.T) {
	var input = 111111
	var actual = false
	expected := isMoreElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP2S2(t *testing.T) {
	var input = 112233
	var actual = true
	expected := isMoreElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP2S3(t *testing.T) {
	var input = 123444
	var actual = false
	expected := isMoreElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}

func TestP2S4(t *testing.T) {
	var input = 111122
	var actual = true
	expected := isMoreElligable(input)
	if actual != expected {
		t.Fatalf("failed")
	}
}
