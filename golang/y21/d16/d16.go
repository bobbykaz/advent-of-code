package d16

import (
	"fmt"
	"strconv"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d16.txt")
	b := hexstringToBinString(lines[0])
	fmt.Println(b)
	p, _ := parsePacket(b)
	totalV := sumPacketVersions(&p)
	fmt.Println("Total versions", totalV)
	fmt.Println("==================================")
	val := eval(&p)
	fmt.Println("Evaluated:", val)
	return 0
}

func hexstringToBinString(hex string) string {
	binStr := ""
	for _, c := range hex {
		bInt, _ := strconv.ParseInt(fmt.Sprintf("%c", c), 16, 64)
		bStr := fmt.Sprintf("%b", bInt)
		for len(bStr) < 4 {
			bStr = "0" + bStr
		}
		fmt.Printf("%c -> %s \n", c, bStr)
		binStr += bStr
	}
	return binStr
}

// parses a packet and returns the remaining string
func parsePacket(pkt string) (packet, string) {
	ver := btoi(pkt[0:3])
	id := btoi(pkt[3:6])
	if id == 4 { //literal
		index := 6
		lastGroup := false
		literalStr := ""
		for !lastGroup {
			lastGroup = rune(pkt[index]) == '0'
			index++
			literalStr += pkt[index : index+4]
			index += 4
		}
		lp := packet{V: ver, T: id, Literal: btoiPrint(literalStr), Subpkts: nil}
		remaining := pkt[index:]
		printf("Found Literal packet V: %d, TID: %d, Value: %d, remaining len %d", lp.V, lp.T, lp.Literal, len(remaining))
		return lp, remaining
	} else {
		ltid := btoi(pkt[6:7])
		index := 7
		if ltid == 0 {
			length := btoi(pkt[index : index+15])
			index += 15
			remaining := pkt[index:]
			op := packet{V: ver, T: id}
			op.Subpkts = make([]*packet, 0)
			printf("Making operator packet %d, %d with %d length sub", ver, id, length)
			for i := int64(0); i < length; i++ {
				newPacket, r := parsePacket(remaining)
				i += int64(len(remaining) - len(r))
				remaining = r
				op.Subpkts = append(op.Subpkts, &newPacket)
			}
			printf("Done Making operator packet %d, %d with %d lenght sub", ver, id, length)
			return op, remaining
		} else { //
			num := btoi(pkt[index : index+11])
			index += 11
			remaining := pkt[index:]
			op := packet{V: ver, T: id}
			op.Subpkts = make([]*packet, 0)
			printf("Making operator packet %d, %d with %d subpackets", ver, id, num)
			for n := int64(0); n < num; n++ {
				newPacket, r := parsePacket(remaining)
				remaining = r
				op.Subpkts = append(op.Subpkts, &newPacket)
			}
			printf("Done making operator packet %d, %d with %d subpackets", ver, id, num)
			return op, remaining
		}
	}
}

func btoi(b string) int64 {
	//printf(b)
	bInt, _ := strconv.ParseInt(b, 2, 64)
	return bInt
}

func btoiPrint(b string) int64 {
	bInt := btoi(b)
	fmt.Println(b, "->", bInt)
	return bInt
}

func printf(a string, b ...interface{}) {
	s := fmt.Sprintf("%s\n", a)
	fmt.Printf(s, b...)
}

func sumPacketVersions(pkt *packet) int64 {
	if pkt.Subpkts == nil {
		return pkt.V
	}
	total := pkt.V
	for _, p := range pkt.Subpkts {
		total += sumPacketVersions(p)
	}
	return total
}

func eval(p *packet) int64 {
	switch p.T {
	case 4:
		return p.Literal
	case 0:
		ns := evalSlice(p.Subpkts)
		sum := ns[0]
		for i := 1; i < len(ns); i++ {
			sum += ns[i]
		}
		fmt.Println("sum of ", ns, "is", sum)
		return sum
	case 1:
		ns := evalSlice(p.Subpkts)
		product := ns[0]
		for i := 1; i < len(ns); i++ {
			product *= ns[i]
		}
		fmt.Println("product of ", ns, "is", product)
		return product
	case 2:
		ns := evalSlice(p.Subpkts)
		min, _ := utilities.FindMinMax64(ns)
		fmt.Println("min of ", ns, "is", min)
		return min
	case 3:
		ns := evalSlice(p.Subpkts)
		_, max := utilities.FindMinMax64(ns)
		fmt.Println("max of ", ns, "is", max)
		return max
	case 5:
		ns := evalSlice(p.Subpkts)
		if ns[0] > ns[1] {
			fmt.Println(ns[0], "is greater than", ns[1])
			return 1
		} else {
			fmt.Println(ns[0], "is NOT greater than", ns[1])
			return 0
		}
	case 6:
		ns := evalSlice(p.Subpkts)
		if ns[0] < ns[1] {
			fmt.Println(ns[0], "is less than", ns[1])
			return 1
		} else {
			fmt.Println(ns[0], "is NOT less than", ns[1])
			return 0
		}
	case 7:
		ns := evalSlice(p.Subpkts)
		if ns[0] == ns[1] {
			fmt.Println(ns[0], "==", ns[1])
			return 1
		} else {
			fmt.Println(ns[0], "!=", ns[1])
			return 0
		}
	}
	fmt.Println("uhoh, encountered an error with packet", p.V, p.T)
	return 0
}

func evalSlice(ps []*packet) []int64 {
	ns := make([]int64, 0)
	for _, p := range ps {
		ns = append(ns, eval(p))
	}
	return ns
}

type packet struct {
	V       int64
	T       int64
	Literal int64
	Subpkts []*packet
}
