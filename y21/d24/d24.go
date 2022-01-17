package d24

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func RunHandCoded() int {
	for i := int64(1); i <= 9; i++ {
		fmt.Println("First digit", i)
		result, answer := lit1(i)
		if result {
			fmt.Println("found answer!", i, answer)
			realNum := fmt.Sprintf("%d%s", i, answer)
			model, err := strconv.ParseInt(realNum, 10, 64)
			if err != nil {
				fmt.Println("error")
				return -1
			}
			prog := parseProgram()

			r2 := runProgram(model, prog)
			fmt.Println("Did it work?", model, ":", r2)

			return 1
		}
	}

	return -1
}

func parseProgram() []instruction {
	lines := utilities.ReadFileIntoLines("input/y21/d24.txt")
	prog := make([]instruction, 0)
	for _, s := range lines {
		pts := strings.Split(s, " ")
		c := parseCommand(pts[0])
		aReg, _ := parseRegister(pts[1])

		if c != inp {
			bReg, bIsReg := parseRegister(pts[2])
			bInt, _ := strconv.Atoi(pts[2])
			bOp := operand{value: int64(bInt), reg: regInvalid}
			if bIsReg {
				bOp.reg = bReg
			}
			i := instruction{C: c, a: aReg, b: bOp}
			prog = append(prog, i)

		} else {
			i := instruction{C: c, a: aReg, b: operand{0, regInvalid}}
			prog = append(prog, i)
		}

	}

	return prog
}

func Run() int {
	prog := parseProgram()

	result := runProgram(34996296989999, prog)
	fmt.Println("Did 34996296989999 work?", result)

	for n := int64(9); 9 >= 1; n-- {
		comp := alu{0, 0, 0, 0, make([]int64, 1), false}
		runSubProgram(n, comp, prog, 1)
	}

	/* 99998779000000
	var n int64 = 99999999999999
	count := 0
	for n >= 11111111111111 {

		if runProgram(n, prog) {
			fmt.Println("highest model:", n)
			return 1
		}
		count++
		if count%1000000 == 0 {
			fmt.Println("tried down to", n)
		}
		n--
	}*/

	return 0
}

func runProgram(model int64, program []instruction) bool {
	comp := alu{0, 0, 0, 0, make([]int64, 14), false}
	strInput := fmt.Sprintf("%d", model)
	skip := false
	for i, c := range strInput {
		if c == '0' {
			skip = true
		}
		comp.input[i] = int64(byte(c) - '0')
	}
	if skip {
		return false
	}

	for _, p := range program {
		comp.process(p)
		if comp.fault {
			return false
		}
	}

	if comp.z == 0 {
		return true
	}
	return false
}

func runSubProgram(nextModel int64, comp alu, program []instruction, level int) (bool, string) {
	//fmt.Println("Running sub program", nextModel, program)
	comp.input = make([]int64, 1)
	i := 0
	comp.input[i] = nextModel
	modelString := fmt.Sprintf("%d", nextModel)
	comp.process(program[i]) //input
	if comp.fault {
		return false, modelString
	}

	for i = 1; i < len(program) && program[i].C != inp; i++ {
		comp.process(program[i])
		if comp.fault {
			return false, modelString
		}
	}

	if i == len(program) {
		return comp.z == 0, modelString
	} else {
		for n := int64(9); n >= 1; n-- {
			subOk, result := runSubProgram(n, comp, program[i:], level+1)
			if subOk {
				modelString = fmt.Sprintf("%s%s", modelString, result)
				fmt.Println("Success!", modelString)
				return true, modelString
			}
		}
	}
	if level < 7 {
		fmt.Println("N", modelString, "L", level, "exhausted")
	}
	return false, modelString
}

func parseRegister(s string) (register, bool) {
	switch s {
	case "x":
		return regX, true
	case "y":
		return regY, true
	case "z":
		return regZ, true
	case "w":
		return regW, true
	}
	return regInvalid, false
}

type register rune

const (
	regX       register = 'x'
	regY                = 'y'
	regZ                = 'z'
	regW                = 'w'
	regInvalid          = 'a'
)

func parseCommand(s string) command {
	switch s {
	case "inp":
		return inp
	case "add":
		return add
	case "mul":
		return mul
	case "div":
		return div
	case "mod":
		return mod
	case "eql":
		return eql
	}
	panic("got a bad command" + s)
}

type command string

const (
	inp command = "inp"
	add         = "add"
	mul         = "mul"
	div         = "div"
	mod         = "mod"
	eql         = "eql"
)

type operand struct {
	value int64
	reg   register
}
type instruction struct {
	C command
	a register
	b operand
}

type alu struct {
	x     int64
	y     int64
	z     int64
	w     int64
	input []int64
	fault bool
}

func (a *alu) process(i instruction) int64 {
	v1 := a.x
	switch i.a {
	case regX:
		v1 = a.x
	case regY:
		v1 = a.y
	case regZ:
		v1 = a.z
	case regW:
		v1 = a.w
	}
	v2 := i.b.value
	if !(i.b.reg == regInvalid) {
		switch i.b.reg {
		case regX:
			v2 = a.x
		case regY:
			v2 = a.y
		case regZ:
			v2 = a.z
		case regW:
			v2 = a.w
		}
	}

	result := a.processCmd(v1, v2, i.C)
	switch i.a {
	case regX:
		a.x = result
	case regY:
		a.y = result
	case regZ:
		a.z = result
	case regW:
		a.w = result
	}
	return 0
}

func (a *alu) processCmd(v1, v2 int64, c command) int64 {
	//fmt.Println("processing command", c, "on", v1, v2)
	switch c {
	case inp:
		tmp := a.input[0]
		//fmt.Println("input:", tmp, "current z", a.z)
		a.input = a.input[1:]
		return tmp
	case add:
		return v1 + v2
	case mul:
		return v1 * v2
	case div:
		if v2 == 0 {
			a.fault = true
			return 0
		} else {
			return v1 / v2
		}
	case mod:
		if v1 < 0 || v2 <= 0 {
			a.fault = true
			return 0
		} else {
			return v1 % v2
		}
	case eql:
		if v1 == v2 {
			return 1
		} else {
			return 0
		}
	}
	panic("bad command!" + c)
}

func lit1(w int64) (bool, string) {
	x, y, z := int64(0), int64(0), int64(0)
	// 	mul x 0
	// add x z
	// mod x 26
	// div z 1
	// add x 13
	x = 13
	// eql x w
	x = eQL(x, w) //w is never 13, this ends up 0
	// eql x 0
	x = eQL(x, 0) // x ends up 1
	// mul y 0
	// add y 25
	// mul y x
	// add y 1
	y = 26
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 14
	y = w + 14
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		//fmt.Println(".second digit", i)
		found, s := lit2(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", 0, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit2(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 1
	// add x 12
	x += 12
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 8
	y = w + 8
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		//fmt.Println("..3rd digit", i)
		found, s := lit3(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit3(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	//mul x 0
	//add x z
	//mod x 26
	x = z % 26
	//div z 1
	//add x 11
	x += 11
	//eql x w
	x = eQL(x, w) // always 0, no input is 11
	//eql x 0
	x = eQL(x, 0) // always 1
	//mul y 0
	//add y 25
	y = 25
	//mul y x
	y *= x
	//add y 1
	y += 1
	//mul z y
	z *= y // z *= 26
	//mul y 0
	//add y w
	//add y 5
	y = w + 5
	//mul y x
	y *= x
	//add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		//fmt.Println("...4th digit", i)
		found, s := lit4(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit4(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x 0
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 4
	y = w + 4
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit5(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit5(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 1
	// add x 15
	x += 15
	// eql x w
	x = eQL(x, w) //x always >15, so 0
	// eql x 0
	x = eQL(x, 0) // always 1
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y //z *= 26
	// mul y 0
	// add y w
	// add y 10
	y = w + 10
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit6(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit6(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x -13
	x -= 13
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 13
	y = w + 13
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit7(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit7(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 1
	// add x 10
	x += 10
	// eql x w
	x = eQL(x, w) // x always >10, ends up 0
	// eql x 0
	x = eQL(x, 0) // always 1
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y // z *= 26
	// mul y 0
	// add y w
	// add y 16
	y = w + 16
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit8(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit8(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x -9
	x -= 9
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 5
	y = w + 5
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit9(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit9(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 1
	// add x 11
	x += 11
	// eql x w
	x = eQL(x, w) // x always greater than w, answer == 0
	// eql x 0
	x = eQL(x, 0) // always 1
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 6
	y = w + 6
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit10(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit10(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 1
	// add x 13
	x += 13
	// eql x w
	x = eQL(x, w) // x always > 13, always 0
	// eql x 0
	x = eQL(x, 0) // always 1
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 13
	y = w + 13
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit11(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit11(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x -14
	x -= 14
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 6
	y = w + 6
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit12(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit12(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x -3
	x -= 3
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 7
	y = w + 7
	// mul y x
	y *= x
	// add z y
	z += y
	for i := int64(1); i <= 9; i++ {
		found, s := lit13(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit13(w, z int64) (bool, string) {
	x, y, initZ := int64(0), int64(0), z
	// 	mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26
	// add x -2
	x -= 2
	// eql x w
	x = eQL(x, w)
	// eql x 0
	x = eQL(x, 0)
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x
	// add y 1
	y += 1
	// mul z y
	z *= y
	// mul y 0
	// add y w
	// add y 13
	y = w + 13
	// mul y x
	y *= x
	// add z y
	z += y // z + y must be less than 26!
	for i := int64(1); i <= 9; i++ {
		found, s := lit14(i, z)
		if found {
			fmt.Println("Success", i, s, "initZ:", initZ, "z:", z)
			s = fmt.Sprintf("%d%s", i, s)
			return true, s
		}
	}
	return false, ""
}

func lit14(w, z int64) (bool, string) {
	x, y := int64(0), int64(0)
	// mul x 0
	// add x z
	// mod x 26
	x = z % 26
	// div z 26
	z /= 26 //z must be less than 26
	// add x -14
	x -= 14
	// eql x w
	x = eQL(x, w) // w must be (z%26) - 14
	// eql x 0
	x = eQL(x, 0) // x must be 1
	if x == 1 {
		return false, ""
	}
	// mul y 0
	// add y 25
	// mul y x
	y = 25 * x // x must be 0
	// add y 1
	y += 1
	// mul z y
	z *= y // z must already b 0
	// mul y 0
	// add y w
	// add y 3
	y = w + 3
	// mul y x
	y *= x //x must be 0
	// add z y
	z += y //y must be 0
	return z == 0, ""
}

func eQL(a, b int64) int64 {
	if a == b {
		return 1
	} else {
		return 0
	}
}
