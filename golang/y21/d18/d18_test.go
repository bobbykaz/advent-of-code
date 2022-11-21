package d18

import (
	"fmt"
	"testing"
)

func TestP1S1(t *testing.T) {
	input := "[1,2]"
	//"[[1,2],3]"
	p, s := parsePair(input, nil)
	result := p.print()
	fmt.Println("remaining:", s)
	if result != input {
		t.Fatalf("failed: %s", result)
	}
}

func TestP1S2(t *testing.T) {
	input := "[[1,2],3]"
	p, s := parsePair(input, nil)
	result := p.print()
	fmt.Println("remaining:", s)
	if result != input {
		t.Fatalf("failed: %s", result)
	}
}

func TestP1S3(t *testing.T) {
	input := "[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]"
	p, s := parsePair(input, nil)
	result := p.print()
	fmt.Println("remaining:", s)
	if result != input {
		t.Fatalf("failed: %s", result)
	}
}

func TestExplode(t *testing.T) {
	explodeTest("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]", t)
	explodeTest("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]", t)
	explodeTest("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]", t)
	explodeTest("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", t)
	explodeTest("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]", t)
	/*
			[[[[[9,8],1],2],3],4] becomes [[[[0,9],2],3],4] (the 9 has no regular number to its left, so it is not added to any regular number).
		[7,[6,[5,[4,[3,2]]]]] becomes [7,[6,[5,[7,0]]]] (the 2 has no regular number to its right, and so it is not added to any regular number).
		[[6,[5,[4,[3,2]]]],1] becomes [[6,[5,[7,0]]],3].
		[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]] becomes [[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]] (the pair [3,2] is unaffected because the pair [7,3] is further to the left; [3,2] would explode on the next action).
		[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]] becomes .

	*/
}

func explodeTest(input, output string, t *testing.T) {
	p, s := parsePair(input, nil)
	fmt.Println("remaining:", s)
	fmt.Println("init", p.print())
	ok := reduceEx(p)
	if !ok {
		t.Fatalf("reduce failed")
	}
	result := p.print()
	if result != output {
		t.Fatalf("failed: %s", result)
	}
}

func TestSplit(t *testing.T) {
	input := "[[[[[9,8],1],2],3],4]"
	p, s := parsePair(input, nil)
	fmt.Println("remaining:", s)
	fmt.Println("init", p.print())
	ok := reduceSp(p)
	if !ok {
		t.Fatalf("reduce failed")
	}
	result := p.print()
	if result != "[[[[0,9],2],3],4]" {
		t.Fatalf("failed: %s", result)
	}
}

func TestMag(t *testing.T) {
	input := "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]"
	p, s := parsePair(input, nil)
	fmt.Println("remaining:", s)
	fmt.Println("init", p.print())

	result := p.magnitude()
	if result != 4140 {
		t.Fatalf("failed: %d", result)
	}
}

func TestAdd(t *testing.T) {
	a := "[[[[4,3],4],4],[7,[[8,4],9]]]"
	b := "[1,1]"
	aa, _ := parsePair(a, nil)
	bb, _ := parsePair(b, nil)
	p := add(aa, bb)

	result := p.print()
	if result != "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]" {
		t.Fatalf("failed: %s", result)
	}
}
