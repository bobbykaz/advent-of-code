package d17

import "fmt"

func Run() int {
	target := bounds{153, 199, -114, -75}
	//target := bounds{20, 30, -5, 10}
	fmt.Println("Starting")
	maxH := 0
	count := 0
	for x := 0; x < 200; x++ {
		for y := -115; y < 10000; y++ {
			hit, height := test(x, y, target)
			if hit && height > maxH {
				maxH = height
			}
			if hit {
				count++
			}
		}
	}
	fmt.Println("Hits:", count, "Max height", maxH)
	return maxH
}

//returns whether the target is hit, and max height
func test(x, y int, b bounds) (bool, int) {
	style := 0
	hit := false
	s := state{X: 0, Y: 0, Vx: x, Vy: y}

	for s.Y >= b.Ymin {
		s = step(s)
		if s.Y > style {
			style = s.Y
		}
		if checkTarget(s, b) {
			hit = true
		}
	}

	if hit {
		fmt.Println("Initial V", x, y, "hits target with max height", style)
	}

	return hit, style

}

func step(i state) state {
	newVx := i.Vx - 1
	if newVx < 0 {
		newVx = 0
	}
	return state{X: i.X + i.Vx, Y: i.Y + i.Vy, Vx: newVx, Vy: i.Vy - 1}
}

func checkTarget(s state, b bounds) bool {
	if s.X >= b.Xmin && s.X <= b.Xmax {
		if s.Y >= b.Ymin && s.Y <= b.Ymax {
			return true
		}
	}
	return false
}

type state struct {
	X  int
	Y  int
	Vx int
	Vy int
}

type bounds struct {
	Xmin int
	Xmax int
	Ymin int
	Ymax int
}
