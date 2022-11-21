package d4

import (
	"crypto/md5"
	"fmt"
	"math"
)

func Part1() int {
	fmt.Println()
	sample := "abcdef609043"
	sh := hash(sample)
	fmt.Printf("%x\n", sh)
	fmt.Println(sh[0], sh[1], sh[2], sh[3], sh[4])
	return p1("iwrupvqb")
}

func p1(key string) int {
	for i := 0; i < math.MaxInt32; i++ {
		if i%10000000 == 0 {
			fmt.Println(i, "...")
		}
		p := fmt.Sprintf("%s%d", key, i)
		h := hash(p)

		if h[0] == 0 &&
			h[1] == 0 &&
			h[2] == 0 { // use h[2] < 16 for 00000....
			fmt.Printf("%s -> %x\n", p, h)
			return i
		}
	}
	return -1
}

func hash(str string) [16]byte {
	return md5.Sum([]byte(str))
}
