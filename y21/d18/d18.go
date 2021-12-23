package d18

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d18.txt")
	max := 0
	for _, s1 := range lines {
		for _, s2 := range lines {
			if s1 != s2 {
				mag1 := addPair(s1, s2)
				if mag1 > max {
					max = mag1
				}
				mag2 := addPair(s2, s1)
				if mag2 > max {
					max = mag2
				}
			}
		}
	}
	return max
}

func addPair(a, b string) int {
	pa, _ := parsePair(a, nil)
	pb, _ := parsePair(b, nil)
	s := add(pa, pb)
	return s.magnitude()
}

func sumAll(lines []string) int {
	a, _ := parsePair(lines[0], nil)
	fmt.Println("Start:", a.print())
	for i := 1; i < len(lines); i++ {
		b, _ := parsePair(lines[i], nil)
		fmt.Println()
		fmt.Println(a.print())
		fmt.Println(" +", b.print())
		p := add(a, b)
		a = p
	}

	return a.magnitude()
}

type pair struct {
	Parent *pair
	N      int
	L      *pair
	R      *pair
}

func (p *pair) findPairToLeft() *pair {
	if p.Parent == nil {
		return nil
	}

	for p.IsLeftChildOf(p.Parent) {
		//fmt.Printf("{%s} is left child of {%s}\n", p.print(), p.Parent.print())
		p = p.Parent
		if p == nil {
			return nil
		}
	}

	if p.Parent == nil {
		return nil
	}

	p = p.Parent.L

	for p.R != nil {
		//fmt.Printf("searching down right path of %s\n", p.print())
		p = p.R
	}

	return p
}

func (p *pair) findPairToRight() *pair {
	if p.Parent == nil {
		return nil
	}

	for p.IsRightChildOf(p.Parent) {
		//fmt.Printf("{%s} is right child of {%s}\n", p.print(), p.Parent.print())
		p = p.Parent
		if p == nil {
			return nil
		}
	}

	if p.Parent == nil {
		return nil
	}

	p = p.Parent.R

	for p.L != nil {
		//fmt.Printf("searching down left path of %s\n", p.print())
		p = p.L
	}

	return p
}

func (p *pair) IsLeftChildOf(other *pair) bool {
	if other == nil {
		return false
	}
	if other.L == p {
		return true
	}
	return false
}

func (p *pair) IsRightChildOf(other *pair) bool {
	if other == nil {
		return false
	}
	if other.R == p {
		return true
	}
	return false
}

func parsePair(s string, parent *pair) (*pair, string) {
	p := pair{Parent: parent}

	if s[0] >= '0' && s[0] <= '9' {
		p.N = int(s[0] - '0')
		p.L, p.R = nil, nil
		return &p, s[1:]
	} else if rune(s[0]) == '[' {
		p.L, s = parsePair(s[1:], &p)
		if rune(s[0]) != ',' {
			fmt.Println("1)Error parsing", s)
		}
		s = s[1:]
		p.R, s = parsePair(s, &p)
		if rune(s[0]) != ']' {
			fmt.Println("2)Error parsing", s)
		}
		s = s[1:]
	}

	return &p, s
}

func add(a, b *pair) *pair {
	p := pair{Parent: nil, L: a, R: b}
	a.Parent = &p
	b.Parent = &p
	either := true
	for either {
		either = reduceEx(&p)
		if either {
			//fmt.Println("...exploded to", p.print())
		} else {
			either = reduceSp(&p)
			if either {
				//fmt.Println("...split to", p.print())
			}
		}
	}
	//fmt.Println(" Result", p.print())
	return &p
}

func reduceEx(p *pair) bool {
	if p == nil {
		return false
	}

	if reduceEx(p.L) {
		return true
	}

	if p.shouldExplode() {
		//fmt.Println(p.print(), "should explode")
		p.explode()
		return true
	}

	if reduceEx(p.R) {
		return true
	}

	return false
}

func reduceSp(p *pair) bool {
	if p == nil {
		return false
	}

	if reduceSp(p.L) {
		return true
	}

	if p.shouldSplit() {
		//fmt.Println(p.print(), "should split")
		p.split()
		return true
	}

	if reduceSp(p.R) {
		return true
	}

	return false
}

func (p *pair) shouldExplode() bool {
	if !p.isRegular() {
		return false
	}
	count := 0
	n := p
	for n != nil {
		n = n.Parent
		count++
	}
	return count > 4
}

func (p *pair) explode() {
	left := p.findPairToLeft()
	if left != nil {
		//fmt.Printf("Left of %s is %s\n", p.print(), left.print())
		left.N += p.L.N
	}
	right := p.findPairToRight()
	if right != nil {
		//fmt.Printf("right of %s is %s\n", p.print(), right.print())
		right.N += p.R.N
	}
	p.L = nil
	p.R = nil
	p.N = 0
}

func (p *pair) shouldSplit() bool {
	return p.hasNoChildren() && p.N >= 10
}

func (p *pair) split() {
	v := p.N / 2
	extra := p.N % 2
	left := pair{Parent: p, N: v}
	right := pair{Parent: p, N: v + extra}
	p.L = &left
	p.R = &right
}

func (p *pair) isRegular() bool {
	return p.L != nil && p.R != nil && p.L.hasNoChildren() && p.R.hasNoChildren()
}

func (p *pair) hasNoChildren() bool {
	return p.L == nil && p.R == nil
}

func (p *pair) magnitude() int {
	if p.hasNoChildren() {
		return p.N
	}
	return 3*p.L.magnitude() + 2*p.R.magnitude()
}

func (p *pair) print() string {
	if p.hasNoChildren() {
		return fmt.Sprintf("%d", p.N)
	} else {
		return fmt.Sprintf("[%s,%s]", p.L.print(), p.R.print())
	}
}
