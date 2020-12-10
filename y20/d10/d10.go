package d10

import (
	"fmt"
	"sort"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d10.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	adapters := utilities.StringsToInts(input)
	sort.Ints(adapters)
	fmt.Println(p1(adapters))
	fmt.Println(p2(adapters))
	return -1
}

func p1(input []int) int {
	d1, d2, d3 := 0, 0, 0
	prev := 0
	for _, v := range input {
		if v-prev == 1 {
			d1++
		} else if v-prev == 2 {
			d2++
		} else if v-prev == 3 {
			d3++
		}
		prev = v
	}
	d3++ //(device)
	fmt.Printf("D1 %d, D2 %d, D3 %d, final %d, final+3 %d\n", d1, d2, d3, prev, prev+3)

	return d1 * d3
}

func isValid(input []int, prev int) bool {
	for _, v := range input {
		if (v - prev) > 3 {
			return false
		}
		prev = v
	}
	return true
}

func trib(n int) int64 {
	switch n {
	case 0:
		return int64(0)
	case 1:
		return int64(0)
	case 2:
		return int64(1)
	default:
		break
	}

	return trib(n-1) + trib(n-2) + trib(n-3)
}

func p2(input []int) int64 {
	/*a run of 4 (ex 5678) permutes 4x
	a run of 3 (ex 567) permutes 2x
	a run of 5 does NOT do 8x...56789 - 7x
		56789,5789,589,5689,5679,569,579
	a run of n permutes trib(n-1)
	*/
	p := int64(1)
	run := 0
	prev := 0
	input = append([]int{0}, input...)
	input = append(input, input[len(input)-1]+3)
	for i := 0; i < len(input); i++ {
		v := input[i]
		d := v - prev
		if d == 1 {
			run++
		} else {
			if run > 1 {
				t := trib(run + 2)
				fmt.Println("Last run", input[i-(run+1):i], "mult factor ", t)

				if t > 1 {
					p *= t
				}
			}
			run = 0
		}
		prev = v
		fmt.Println("i:", i, "v:", v, "r:", run, "p:", p)
	}

	return p
}
