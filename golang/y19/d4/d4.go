package d4

import (
	"fmt"
	"strconv"
)

func Part1() int {
	start, end := 273025, 767253
	possiblePwd := 0
	for i := start; i < end; i++ {
		if isMoreElligable(i) {
			possiblePwd++
		}
	}

	fmt.Println("Total:", possiblePwd)
	return possiblePwd
}

func isElligable(num int) bool {
	str := strconv.Itoa(num)
	nums := make([]int, len(str))
	for i, c := range str {
		nums[i], _ = strconv.Atoi(string(c))
	}

	neverDecreasing := true
	hasRepeat := false
	for i := 0; i < len(nums)-1; i++ {
		if nums[i] > nums[i+1] {
			neverDecreasing = false
		}

		if nums[i] == nums[i+1] {
			hasRepeat = true
		}
	}

	return neverDecreasing && hasRepeat
}

func isMoreElligable(num int) bool {
	str := strconv.Itoa(num)
	nums := make([]int, len(str))
	for i, c := range str {
		nums[i], _ = strconv.Atoi(string(c))
	}

	neverDecreasing := true
	for i := 0; i < len(nums)-1; i++ {
		if nums[i] > nums[i+1] {
			neverDecreasing = false
		}
	}

	hasExactDouble := false
	streakCount := 0
	for i := 0; i < len(nums)-1; i++ {
		if nums[i] == nums[i+1] {
			streakCount++
		} else {
			if streakCount == 1 {
				hasExactDouble = true
			}
			streakCount = 0
		}
	}
	if streakCount == 1 {
		hasExactDouble = true
	}

	return neverDecreasing && hasExactDouble
}
