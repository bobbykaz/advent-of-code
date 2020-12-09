package d14

import (
	"fmt"
)

func Part1() {
	fmt.Println("Day 14")
	input := 598701
	loopBounds := 100000000
	recipes := make([]int, 2)
	recipes[0] = 3
	recipes[1] = 7
	elf1 := 0
	elf2 := 1
	l := len(recipes)
	for l < loopBounds+10 {
		newRecipe := recipes[elf1] + recipes[elf2]
		twoRecAdded := false
		if newRecipe >= 10 {
			recipes = append(recipes, 1)
			newRecipe -= 10
			twoRecAdded = true
		}
		recipes = append(recipes, newRecipe)

		l = len(recipes)
		elf1 = (elf1 + 1 + recipes[elf1]) % l
		elf2 = (elf2 + 1 + recipes[elf2]) % l
		//printRecipes(recipes)
		// if l%1000 == 0 {
		// 	fmt.Printf("Elf1: %d, Elf2: %d, total: %d\n", elf1, elf2, l)
		// }
		if l > 6 {
			if checkMostRecentRecipes(input, recipes, twoRecAdded) {
				//printRecipes(recipes)
				fmt.Println("Target found")
				l = loopBounds + 11
			}
		}
	}

	message := ""
	for i := len(recipes) - 10; i < len(recipes); i++ {
		message += fmt.Sprintf("%d ", recipes[i])
	}
	fmt.Printf("Final number: %s \n", message)
	fmt.Printf("Elf1: %d, Elf2: %d, total: %d\n", elf1, elf2, len(recipes))
}

func printRecipes(r []int) {
	message := ""
	for i := 0; i < len(r); i++ {
		message += fmt.Sprintf("%d", r[i])
	}
	fmt.Println(message)
}

func checkMostRecentRecipes(target int, r []int, doubleCheck bool) bool {
	//if two recipes were added, we need to check numbers 7-1 from end in addition to 6-0
	targetStr := fmt.Sprintf("%d", target)
	recipeStr := ""
	for i := len(r) - len(targetStr); i < len(r); i++ {
		recipeStr += fmt.Sprintf("%d", r[i])
	}
	//fmt.Printf("checking %s against %s\n", targetStr, recipeStr)
	if targetStr == recipeStr {
		return true
	}
	if doubleCheck {
		recipeStr = ""
		for i := len(r) - len(targetStr) - 1; i < (len(r) - 1); i++ {
			recipeStr += fmt.Sprintf("%d", r[i])
		}

		//fmt.Printf("checking %s against %s\n", targetStr, recipeStr)
		if targetStr == recipeStr {
			return true
		}
	}
	return false
}
