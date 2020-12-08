package d8

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20d8.txt"

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	pc := bootCode{Accumulator: 0, Program: input, Ptr: 0, ExecMap: make(map[int]bool)}
	fmt.Println(pc.Run())
	for i, s := range input {
		inputCopy := make([]string, len(input))
		copy(inputCopy, input)
		result := -1
		if strings.Contains(s, "nop") {
			fmt.Printf("Run %d nop ->jmp: ", i)
			pc = bootCode{Accumulator: 0, Program: inputCopy, Ptr: 0, ExecMap: make(map[int]bool)}
			pc.Program[i] = strings.Replace(pc.Program[i], "nop", "jmp", 1)
			result = pc.Run()
		} else if strings.Contains(s, "jmp") {
			fmt.Printf("Run %d jmp-> nop: ", i)
			pc = bootCode{Accumulator: 0, Program: inputCopy, Ptr: 0, ExecMap: make(map[int]bool)}
			pc.Program[i] = strings.Replace(pc.Program[i], "jmp", "nop", 1)
			result = pc.Run()
		}
		if result == 0 {
			return pc.Accumulator
		}
	}

	return len(input)
}

type bootCode struct {
	Accumulator int
	Program     []string
	Ptr         int
	ExecMap     map[int]bool
}

func (b *bootCode) Run() int {
	for true {
		_, exists := b.ExecMap[b.Ptr]
		if exists {
			fmt.Println("Loop detected at ", b.Ptr, "Acc:", b.Accumulator)
			return -1
		}

		if b.Ptr == len(b.Program) {
			fmt.Println("Program Exit. Acc:", b.Accumulator)
			return 0
		}

		if b.Ptr < 0 || b.Ptr > len(b.Program) {
			fmt.Println("Fault found. Ptr:", b.Ptr, "Acc:", b.Accumulator)
			return b.Ptr
		}

		curr := b.Ptr
		b.Execute(b.Program[b.Ptr])
		b.ExecMap[curr] = true
	}
	return -1
}

func (b *bootCode) Execute(ins string) {
	pts := strings.Split(ins, " ")
	switch pts[0] {
	case "acc":
		b.Accumulate(pts[1])
	case "jmp":
		b.Jump(pts[1])
	case "nop":
		b.Ptr++
	}
}

func (b *bootCode) Accumulate(input string) {
	i, _ := strconv.Atoi(input)
	b.Accumulator += i
	b.Ptr++
}

func (b *bootCode) Jump(input string) {
	i, _ := strconv.Atoi(input)
	b.Ptr += i
}
