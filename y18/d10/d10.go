package d10

import (
	"fmt"
	"sort"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input10.txt")

	particles := make([]*particle, 0)
	for i := 0; i < len(input); i++ {
		prt := processLine(input[i])
		fmt.Println("x", prt.Pos.X, "y", prt.Pos.Y, "vX", prt.Vel.X, "vY", prt.Vel.Y)
		particles = append(particles, prt)
	}
	findBounds(particles)

	stop := false
	for step := 0; step < 20000; step++ {
		for i := 0; i < len(particles); i++ {
			particles[i].Pos.X += particles[i].Vel.X
			particles[i].Pos.Y += particles[i].Vel.Y
		}

		minx, _, maxx, _ := findBounds(particles)
		if (maxx - minx) < 150 {
			stop = true
			fmt.Println("Step: ", step)
			printParticles(particles, minx)
		}
		if stop && (maxx-minx) > 150 {
			step = 20000
		}
	}
}

type vec2 struct {
	X int
	Y int
}

type particle struct {
	Pos vec2
	Vel vec2
}

func printParticles(particles []*particle, minx int) {
	sort.Slice(particles, func(i int, j int) bool { return particleLess(particles[i], particles[j]) })
	fmt.Println("......................................................")
	lastX, lastY := minx-1, particles[0].Pos.Y
	//Y will always decrease
	//x will always increase
	for i := 0; i < len(particles); i++ {
		x, y := particles[i].Pos.X, particles[i].Pos.Y
		//dont print overlapping particles
		if !(x == lastX && y == lastY) {
			for lastY < y {
				fmt.Printf("\n")
				lastX = minx - 1
				lastY++
			}
			for lastX < x-1 {
				fmt.Printf(" ")
				lastX++
			}
			fmt.Printf("#")
			lastX = x
		}
	}
	fmt.Printf("\n")
}

func particleLess(a, b *particle) bool {
	//Higher Y values are upper lines. Lower X values are to the Left
	if a.Pos.Y < b.Pos.Y {
		return true
	}

	if a.Pos.Y == b.Pos.Y {
		return a.Pos.X < b.Pos.X
	}
	return false
}

func findBounds(particles []*particle) (int, int, int, int) {
	minx, miny := particles[0].Pos.X, particles[0].Pos.Y
	maxx, maxy := minx, miny

	for i := 1; i < len(particles); i++ {
		pos := particles[i].Pos
		if pos.X < minx {
			minx = pos.X
		}
		if pos.Y < miny {
			miny = pos.Y
		}
		if pos.X > maxx {
			maxx = pos.X
		}
		if pos.Y > maxy {
			maxy = pos.Y
		}
	}

	dx, dy := (maxx - minx), (maxy - miny)
	if dx < 150 {
		fmt.Println("Bounds", minx, miny, maxx, maxy, "diff", dx, dy)
	}
	return minx, miny, maxx, maxy
}

func processLine(line string) *particle {
	//position=<-31065, -31102> velocity=< 3,  3>
	parts := strings.Split(line, "velocity=")
	//position
	px, py, err := utilities.ParseCoord(parts[0], "position=<", ", ", ">")
	if err != nil {
		fmt.Println("position", px, py, err)
	}
	pos := vec2{X: px, Y: py}

	//velocity
	vx, vy, err := utilities.ParseCoord(parts[1], "<", ", ", ">")
	if err != nil {
		fmt.Println("velocity", px, py, err)
	}
	vel := vec2{X: vx, Y: vy}

	return &particle{Pos: pos, Vel: vel}
}
