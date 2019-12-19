package common

import (
	"testing"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func TestP1S1(t *testing.T) {
	input := "1,9,10,3,2,3,11,0,99,30,40,50"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 3500 != prog.Program[0] {
		t.Fatalf("failed")
	}
}

func TestImmediateModeMult1(t *testing.T) {
	input := "1002,4,3,4,33"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 99 != prog.Program[4] {
		t.Fatalf("failed")
	}
}

func TestImmediateModeMult2(t *testing.T) {
	input := "1102,11,9,4,33"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 99 != prog.Program[4] {
		t.Fatalf("failed")
	}
}

func TestImmediateModeAdd1(t *testing.T) {
	input := "101,11,4,4,88"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 99 != prog.Program[4] {
		t.Fatalf("failed")
	}
}

func TestImmediateModeAdd2(t *testing.T) {
	input := "1101,11,88,4,7"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 99 != prog.Program[4] {
		t.Fatalf("failed")
	}
}

func TestInput(t *testing.T) {
	input := "3,2,100"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 99}
	prog.Run(true)
	if 99 != prog.Program[2] {
		t.Fatalf("failed")
	}
}

func TestOutput(t *testing.T) {
	input := "4,2,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 99 != prog.Output {
		t.Fatalf("failed")
	}

	if !prog.NewOutput {
		t.Fatalf("failed")
	}
}

func TestP1S2(t *testing.T) {
	input := "1,0,0,0,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 2 != prog.Program[0] {
		t.Fatalf("failed")
	}
}

func TestP1S3(t *testing.T) {
	input := "2,3,0,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 6 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

//2,4,4,5,99,0 becomes 2,4,4,5,99,9801 (99 * 99 = 9801).
//1,1,1,4,99,5,6,0,99 becomes 30,1,1,4,2,5,6,0,99.
func TestP1S4(t *testing.T) {
	input := "2,4,4,5,99,0"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 9801 != prog.Program[5] {
		t.Fatalf("failed")
	}
}
func TestP1S5(t *testing.T) {
	input := "1,1,1,4,99,5,6,0,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 30 != prog.Program[0] {
		t.Fatalf("failed")
	}
	if 2 != prog.Program[4] {
		t.Fatalf("failed")
	}
}

func TestLessThan(t *testing.T) {
	input := "1107,1,2,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 1 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestLessThan2(t *testing.T) {
	input := "1007,0,1008,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 1 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestLessThan3(t *testing.T) {
	input := "1007,0,1007,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 0 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestLessThan4(t *testing.T) {
	input := "1107,3,2,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 0 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestEquals_Position(t *testing.T) {
	input := "8,3,3,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 1 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestEquals_Immediate(t *testing.T) {
	input := "1108,107,108,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 0 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestEquals_Mix(t *testing.T) {
	input := "108,99,4,3,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 1 != prog.Program[3] {
		t.Fatalf("failed")
	}
}

func TestJumpTrue_Jumps(t *testing.T) {
	input := "1105,1,5,99,-1,104,77,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 77 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJumpTrue_NoJump(t *testing.T) {
	input := "105,0,5,104,77,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 77 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJumpFalse_Jumps(t *testing.T) {
	input := "1106,0,5,99,-1,104,77,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 77 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJumpFalse_NoJump(t *testing.T) {
	input := "1106,33,5,104,77,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0}
	prog.Run(true)
	if 77 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestComps1(t *testing.T) {
	input := "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 7}
	prog.Run(true)
	if 999 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestComps2(t *testing.T) {
	input := "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 8}
	prog.Run(true)
	if 1000 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestComps3(t *testing.T) {
	input := "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 9}
	prog.Run(true)
	if 1001 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJump1(t *testing.T) {
	input := "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 0}
	prog.Run(true)
	if 0 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJump2(t *testing.T) {
	input := "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 7}
	prog.Run(true)
	if 1 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJump3(t *testing.T) {
	input := "3,3,1105,-1,9,1101,0,0,12,4,12,99,1"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 0}
	prog.Run(true)
	if 0 != prog.Output {
		t.Fatalf("failed")
	}
}

func TestJump4(t *testing.T) {
	input := "3,3,1105,-1,9,1101,0,0,12,4,12,99,1"
	intcodes := utilities.StringToInts(input)
	prog := IntcodeProgram{Program: intcodes, CurrentPtr: 0, Input: 7}
	prog.Run(true)
	if 1 != prog.Output {
		t.Fatalf("failed")
	}
}
