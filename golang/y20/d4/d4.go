package d4

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

type passport struct {
	byr string
	iyr string
	eyr string
	ecl string
	hcl string
	hgt string
	pid string
	cid string
}

func Part1() int {
	input := utilities.ReadFileIntoLines("input/y20/d4.txt")
	passports := make([]passport, 0)
	pp := passport{}
	for _, s := range input {
		if s == "" {
			passports = append(passports, pp)
			pp = passport{}
		} else {
			pp.fillIn(s)
		}
	}

	passports = append(passports, pp)

	validpp := 0
	for _, p := range passports {
		if fieldsPresent(p) {
			validpp++
		}
	}

	return validpp
}

func (p *passport) fillIn(s string) {
	parts := strings.Split(s, " ")
	for _, sp := range parts {
		kv := strings.Split(sp, ":")
		p.fillInEntry(kv[0], kv[1])
	}
}

func (p *passport) fillInEntry(k, v string) {
	switch k {
	case "byr":
		p.byr = v
	case "iyr":
		p.iyr = v
	case "eyr":
		p.eyr = v
	case "hcl":
		p.hcl = v
	case "ecl":
		p.ecl = v
	case "hgt":
		p.hgt = v
	case "pid":
		p.pid = v
	case "cid":
		p.cid = v
	}
}

func fieldsPresent(pp passport) bool {
	return len(pp.byr) > 0 &&
		len(pp.iyr) > 0 &&
		len(pp.eyr) > 0 &&
		len(pp.ecl) > 0 &&
		len(pp.hcl) > 0 &&
		len(pp.hgt) > 0 &&
		len(pp.pid) > 0
}

func isValid(pp passport) bool {
	byr, err := strconv.Atoi(pp.byr)
	if err != nil || byr < 1920 || byr > 2002 {
		fmt.Println(pp, "invalid byr:", pp.byr)
		return false
	}

	iyr, err := strconv.Atoi(pp.iyr)
	if err != nil || iyr < 2010 || iyr > 2020 {
		fmt.Println(pp, "invalid iyr:", pp.iyr)
		return false
	}

	eyr, err := strconv.Atoi(pp.eyr)
	if err != nil || eyr < 2020 || eyr > 2030 {
		fmt.Println(pp, "invalid eyr:", pp.eyr)
		return false
	}

	_, err = strconv.Atoi(pp.pid)
	if err != nil || len(pp.pid) != 9 {
		fmt.Println(pp, ";invalid pid:", pp.pid)
		return false
	}

	if !eyeCheck(pp.ecl) {
		fmt.Println(pp, "invalid ecl:", pp.ecl)
		return false
	}

	if !hairCheck(pp.hcl) {
		fmt.Println(pp, "invalid hcl:", pp.hcl)
		return false
	}

	if !heightCheck(pp.hgt) {
		fmt.Println(pp, "invalid hgt:", pp.hgt)
		return false
	}

	return true
}

func heightCheck(s string) bool {
	if strings.Contains(s, "cm") {
		substr := s[:(len(s) - 2)]
		cm, err := strconv.Atoi(substr)
		if err != nil || cm < 150 || cm > 193 {
			return false
		}
		return true
	} else if strings.Contains(s, "in") {
		substr := s[:(len(s) - 2)]
		in, err := strconv.Atoi(substr)
		if err != nil || in < 59 || in > 76 {
			return false
		}
		return true
	}

	return false
}

func hairCheck(s string) bool {
	if len(s) != 7 {
		return false
	}

	if rune(s[0]) != '#' {
		return false
	}

	for _, c := range s[1:] {
		b := byte(c)
		if !(b >= byte('0') && b <= byte('9')) && !(b >= byte('a') && b <= byte('f')) {
			return false
		}
	}
	return true
}

func eyeCheck(s string) bool {
	switch s {
	case "amb":
		return true
	case "blu":
		return true
	case "brn":
		return true
	case "gry":
		return true
	case "grn":
		return true
	case "hzl":
		return true
	case "oth":
		return true
	}
	return false
}

func Part2() int {
	input := utilities.ReadFileIntoLines("input/y20/d4.txt")
	passports := make([]passport, 0)
	pp := passport{}
	for _, s := range input {
		if s == "" {
			passports = append(passports, pp)
			pp = passport{}
		} else {
			pp.fillIn(s)
		}
	}

	passports = append(passports, pp)

	firstPass := make([]passport, 0)

	for _, p := range passports {
		if fieldsPresent(p) {
			firstPass = append(firstPass, p)
		}
	}

	validpp := 0
	for _, p := range firstPass {
		if isValid(p) {
			validpp++
		}
	}

	return validpp
}
