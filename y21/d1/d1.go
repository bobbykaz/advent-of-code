package d1

import (
	"fmt"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Part1() int {
	lines := utilities.ReadFileIntoLines("input/y21/d1.txt")
	numbers := utilities.StringsToInts(lines)

	prev := numbers[0]
	next := 0
	incs := 0
	for i := 1; i < len(numbers); i++ {
		next = numbers[i]
		if next > prev {
			incs++
		}
		prev = next
	}

	Part2(numbers)
	return incs
}

func Part2(nums []int) int {

	p1 := nums[0]
	p2 := nums[1]
	p3 := nums[2]
	psum := p1 + p2 + p3
	incs := 0
	p1 = p2
	p2 = p3
	for i := 3; i < len(nums); i++ {
		p3 = nums[i]
		sum := p1 + p2 + p3
		if sum > psum {
			incs++
		}
		p1 = p2
		p2 = p3
		psum = sum
	}
	fmt.Println("part 2 output:", incs)
	return incs
}
