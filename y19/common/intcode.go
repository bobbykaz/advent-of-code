package common

import "fmt"

type IntcodeProgram struct {
	Input      []int
	Output     []int
	NewOutput  bool
	Program    []int
	CurrentPtr int
	PrintMsgs  bool
	JumpFlag   bool
}
type OpCode struct {
	Code   int
	P1Mode int
	P2Mode int
	P3Mode int
}

func (p *IntcodeProgram) Run(print bool) int {
	p.PrintMsgs = print
	if p.Output == nil {
		p.Output = make([]int, 0)
	}
	for p.CurrentPtr = 0; p.CurrentPtr < len(p.Program); {
		oc := p.GetOpCode()
		i1, i2, i3 := p.GetInputs(oc)
		opCode := oc.Code

		if opCode != 99 {
			p.NewOutput = false
		}

		switch opCode {
		case 1:
			p.Add(oc, i1, i2, i3)
			break
		case 2:
			p.Mul(oc, i1, i2, i3)
			break
		case 3:
			p.StoreInput(i1)
			break
		case 4:
			p.StoreOutput(oc, i1)
			break
		case 5:
			p.JumpIf(true, oc, i1, i2)
			break
		case 6:
			p.JumpIf(false, oc, i1, i2)
			break
		case 7:
			p.LessThan(oc, i1, i2, i3)
			break
		case 8:
			p.Equal(oc, i1, i2, i3)
			break
		case 99:
			if print {
				fmt.Println("HALT:")
				if p.NewOutput {
					fmt.Println("Output", p.Output)
				}
				p.Print()
			}
			return p.Program[0]
		default:
			fmt.Println("Error at pos", p.CurrentPtr, "opcode", opCode)
		}
		if !p.JumpFlag {
			p.CurrentPtr += p.GetNumInputs(oc)
		}
		p.JumpFlag = false
		//p.Print()
	}
	return -1
}

//GetOpCode breaks input into opcode and reads parameter modes
func (p *IntcodeProgram) GetOpCode() OpCode {
	index := p.CurrentPtr
	opCode := p.Program[index]
	code := opCode % 100
	opCode = opCode / 100
	p1 := opCode % 10
	opCode = opCode / 10
	p2 := opCode % 10
	opCode = opCode / 10
	p3 := opCode % 10
	return OpCode{Code: code, P1Mode: p1, P2Mode: p2, P3Mode: p3}
}

//GetNumInputs returns the number of inputs plus the opcode, or how far to advance instruction ptr
func (p *IntcodeProgram) GetNumInputs(oc OpCode) int {
	switch oc.Code {
	case 1:
		return 4
	case 2:
		return 4
	case 3:
		return 2
	case 4:
		return 2
	case 5:
		return 3
	case 6:
		return 3
	case 7:
		return 4
	case 8:
		return 4
	case 99:
		return 0
	default:
		fmt.Println("Error reading input count of ", oc)
		break
	}
	return -1
}

//GetNumInputs returns the number of inputs plus the opcode, or how far to advance instruction ptr
func (p *IntcodeProgram) GetInputs(oc OpCode) (int, int, int) {
	i1, i2, i3 := -1, -1, -1
	switch oc.Code {
	case 1:
		fallthrough
	case 2:
		fallthrough
	case 7:
		fallthrough
	case 8:
		i1 = p.Program[p.CurrentPtr+1]
		i2 = p.Program[p.CurrentPtr+2]
		i3 = p.Program[p.CurrentPtr+3]
	case 3:
		fallthrough
	case 4:
		i1 = p.Program[p.CurrentPtr+1]
	case 5:
		fallthrough
	case 6:
		i1 = p.Program[p.CurrentPtr+1]
		i2 = p.Program[p.CurrentPtr+2]
	case 99:
		if p.Output == nil || len(p.Output) == 0 {
			return 0, -1, -1
		}
		return p.Output[0], -1, -1
	default:
		fmt.Println("Error reading inputs of ", oc)
		panic("bad input")
	}

	return i1, i2, i3
}

//operations

func (p *IntcodeProgram) Add(oc OpCode, i1, i2, i3 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}
	if oc.P2Mode == 0 && i2 >= 0 {
		i2 = p.Program[i2]
	}

	if p.PrintMsgs {
		fmt.Println("storing ", i1, "+", i2, "(", i1+i2, ") at position ", i3)
	}
	p.Program[i3] = i1 + i2
}

func (p *IntcodeProgram) Mul(oc OpCode, i1, i2, i3 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}
	if oc.P2Mode == 0 && i2 >= 0 {
		i2 = p.Program[i2]
	}

	if p.PrintMsgs {
		fmt.Println("storeing ", i1, "X", i2, "(", i1*i2, ") at position ", i3)
	}
	p.Program[i3] = i1 * i2
}

func (p *IntcodeProgram) StoreInput(i1 int) {
	if p.PrintMsgs {
		fmt.Println("saving input ", p.Input, "to position", i1)
	}
	p.Program[i1] = p.Input[0]
	if len(p.Input) > 1 {
		p.Input = p.Input[1:]
	} else {
		p.Input = nil
	}
}

func (p *IntcodeProgram) StoreOutput(oc OpCode, i1 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}
	if p.PrintMsgs {
		fmt.Println("saving output ", i1)
	}
	if p.Output == nil {
		p.Output = make([]int, 0)
	}
	p.Output = append(p.Output, i1)
	p.NewOutput = true
}

func (p *IntcodeProgram) JumpIf(condition bool, oc OpCode, i1, i2 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}

	if oc.P2Mode == 0 {
		i2 = p.Program[i2]
	}

	if p.PrintMsgs {
		if condition {
			fmt.Println("jump to: ", i2, "if", i1, " != 0")
		} else {
			fmt.Println("jump to: ", i2, "if", i1, " == 0")
		}
	}

	if (i1 != 0) == condition {
		p.CurrentPtr = i2
		//fmt.Println("jumping....ptr at", i2)
		p.JumpFlag = true
	}
}

func (p *IntcodeProgram) LessThan(oc OpCode, i1, i2, i3 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}
	if oc.P2Mode == 0 && i2 >= 0 {
		i2 = p.Program[i2]
	}

	if p.PrintMsgs {
		fmt.Println("Compareing: ", i1, "<", i2, "?@position ", i3)
	}

	if i1 < i2 {
		p.Program[i3] = 1
	} else {
		p.Program[i3] = 0
	}
}

func (p *IntcodeProgram) Equal(oc OpCode, i1, i2, i3 int) {
	if oc.P1Mode == 0 {
		i1 = p.Program[i1]
	}
	if oc.P2Mode == 0 && i2 >= 0 {
		i2 = p.Program[i2]
	}

	if p.PrintMsgs {
		fmt.Println("Compareing: ", i1, "==", i2, "?@position ", i3)
	}

	if i1 == i2 {
		p.Program[i3] = 1
	} else {
		p.Program[i3] = 0
	}
}

//Print prints out program
func (p *IntcodeProgram) Print() {
	fmt.Println("Ptr: ", p.CurrentPtr)
	i := 0
	for i = 0; i < len(p.Program)-4; i += 4 {
		fmt.Println(i, ":::", p.Program[i], ",", p.Program[i+1], ",", p.Program[i+2], ",", p.Program[i+3])
	}
	if i < len(p.Program) {
		fmt.Print(i, " ::: ", p.Program[i])
		i++
		for i < len(p.Program) {
			fmt.Print(" , ", p.Program[i])
			i++
		}
		fmt.Print("\n")
	}
	fmt.Println("==============================")
}
