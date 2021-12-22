package d22

import (
	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d22.txt")
	world := make([][][]bool, 102)
	for x := range world {
		world[x] = make([][]bool, 102)
		for y := range world[x] {
			world[x][y] = make([]bool, 102)
		}
	}

	for _, s := range lines {
		//on x=-26..26,y=-40..10,z=-12..42
		pts := utilities.Split(s, " x=", "..", ",y=", "..", ",z=", "..")
		light := false
		if pts[0] == "on" {
			light = true
		}
		nums := utilities.StringsToInts(pts[1:])
		x, y, z, xx, yy, zz, skip := bounds(nums, -50, 50)

		if skip {
			break
		}

		for i := x; i <= xx; i++ {
			for j := y; j <= yy; j++ {
				for k := z; k <= zz; k++ {
					world[i][j][k] = light
				}
			}
		}
	}

	count := 0

	for x := range world {
		for y := range world[x] {
			for _, on := range world[x][y] {
				if on {
					count++
				}
			}
		}
	}

	return count
}

func bounds(nums []int, low, high int) (int, int, int, int, int, int, bool) {
	if nums[1] < low || nums[3] < low || nums[5] < low {
		return 0, 0, 0, 0, 0, 0, true
	}

	if nums[0] > high || nums[2] > high || nums[4] > high {
		return 0, 0, 0, 0, 0, 0, true
	}

	for i, n := range nums {
		if n < low {
			nums[i] = low
		}

		if n > high {
			nums[i] = high
		}
	}

	x := nums[0] + high + 1
	xx := nums[1] + high + 1
	y := nums[2] + high + 1
	yy := nums[3] + high + 1
	z := nums[4] + high + 1
	zz := nums[5] + high + 1

	return x, y, z, xx, yy, zz, false
}
