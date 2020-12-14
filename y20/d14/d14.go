package d14

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d14.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	//p1(input)
	p2(input)
	return -1
}

type bitmasker struct {
	Mask string
	Mem  map[int64]int64
}

func (b *bitmasker) MemSet(n int64, v int64) {
	b.Mem[n] = mask(b.Mask, v)
}

func mask(mask string, v int64) int64 {
	bstr := strconv.FormatInt(v, 2)
	for len(bstr) < len(mask) {
		bstr = fmt.Sprintf("0%s", bstr)
	}
	result := ""
	for i, c := range mask {
		if c != 'X' {
			result += string(c)
		} else {
			result += string(bstr[i])
		}
	}

	r, err := strconv.ParseInt(result, 2, 64)
	if err != nil {
		panic(err)
	}

	return r
}

func (b *bitmasker) MemSetV2(n int64, v int64) {
	addrs := getMaskedAddrs(b.Mask, n)
	for _, i := range addrs {
		b.Mem[i] = v
	}
}

func getMaskedAddrs(mask string, n int64) []int64 {
	bstr := strconv.FormatInt(n, 2)
	for len(bstr) < len(mask) {
		bstr = fmt.Sprintf("0%s", bstr)
	}
	memAddrs := make([]string, 1)
	memAddrs[0] = ""
	for i, c := range mask {
		if c == 'X' {
			memAddrsTemp := make([]string, 0, len(memAddrs)*2)
			for _, v := range memAddrs {
				memAddrsTemp = append(memAddrsTemp, v+"0")
				memAddrsTemp = append(memAddrsTemp, v+"1")
			}
			memAddrs = memAddrsTemp
		} else if c == '1' {
			for mi := 0; mi < len(memAddrs); mi++ {
				memAddrs[mi] += "1"
			}
		} else {
			for mi := 0; mi < len(memAddrs); mi++ {
				memAddrs[mi] += string(bstr[i])
			}
		}
	}

	results := make([]int64, 0, len(memAddrs))
	for _, v := range memAddrs {
		r, err := strconv.ParseInt(v, 2, 64)
		if err != nil {
			panic(err)
		}
		results = append(results, r)
	}

	return results
}

func parseCmd(str string) (string, string, string) {
	pts := strings.Split(str, " = ")
	if pts[0] == "mask" {
		return pts[0], pts[1], ""
	}
	//whats left:
	//mem[1234]
	return "mem", pts[0][4 : len(pts[0])-1], pts[1]
}

func p1(input []string) {
	b := bitmasker{Mask: "", Mem: make(map[int64]int64)}
	for i, s := range input {
		cmd, v1, v2 := parseCmd(s)
		if cmd == "mask" {
			b.Mask = v1
		} else {
			i1, _ := strconv.ParseInt(v1, 10, 64)
			i2, _ := strconv.ParseInt(v2, 10, 64)
			b.MemSet(i1, i2)
		}
		fmt.Printf("%d: %s: %s, %s \n", i, cmd, v1, v2)
	}
	sum := int64(0)
	for _, v := range b.Mem {
		sum += v
	}
	fmt.Println("Total mem sum", sum)
}

func p2(input []string) {
	b := bitmasker{Mask: "", Mem: make(map[int64]int64)}
	for i, s := range input {
		cmd, v1, v2 := parseCmd(s)
		if cmd == "mask" {
			b.Mask = v1
		} else {
			i1, _ := strconv.ParseInt(v1, 10, 64)
			i2, _ := strconv.ParseInt(v2, 10, 64)
			b.MemSetV2(i1, i2)
		}
		fmt.Printf("%d: %s: %s, %s \n", i, cmd, v1, v2)
	}
	sum := int64(0)
	for _, v := range b.Mem {
		sum += v
	}
	fmt.Println("Total mem sum", sum)
}
