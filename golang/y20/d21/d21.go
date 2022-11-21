package d21

import (
	"fmt"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

var inputFile = "input/y20/d21.txt"

var Print = true

func Run() int {
	input := utilities.ReadFileIntoLines(inputFile)
	foods := make([]food, 0)
	for _, s := range input {
		foods = append(foods, parseFood(s))
	}

	// ingrediantes -> possible allergens
	ingMap := make(map[string][]string)
	//allergens -> possible ingrediants
	allgMap := make(map[string][]string)

	for _, f := range foods {
		//ingrediants
		for _, i := range f.Ingrediants {
			_, exists := ingMap[i]
			if exists {
				ingMap[i] = utilities.IntersectString(ingMap[i], f.Allergens)
			} else {
				ingMap[i] = make([]string, len(f.Allergens))
				copy(ingMap[i], f.Allergens)
			}
		}
		//allergens
		for _, i := range f.Allergens {
			_, exists := allgMap[i]
			if exists {
				allgMap[i] = utilities.IntersectString(allgMap[i], f.Ingrediants)
			} else {
				allgMap[i] = make([]string, len(f.Ingrediants))
				copy(allgMap[i], f.Ingrediants)
			}
		}
	}

	log("Ingrediants -> Possible Allergens\n")
	for k, v := range ingMap {
		log("%s: %v\n", k, v)
	}

	possibleAI := make(map[string]bool)

	log("Allergens -> Ingrediants\n")
	for k, v := range allgMap {
		for _, s := range v {
			possibleAI[s] = true
		}
		log("%s: %v\n", k, v)
	}

	fmt.Println(possibleAI)

	count := 0
	for _, f := range foods {
		for _, s := range f.Ingrediants {
			_, exists := possibleAI[s]
			if !exists {
				count++
			}
		}

	}

	log("%d\n", count)
	return count
}

type food struct {
	Ingrediants []string
	Allergens   []string
}

func parseFood(str string) food {
	pts := strings.Split(str, " (contains ")
	ing := strings.Split(pts[0], " ")
	allStr := pts[1][:len(pts[1])-1]
	allerg := strings.Split(allStr, ", ")

	f := food{Ingrediants: ing, Allergens: allerg}
	return f
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
