package d4

import (
	"fmt"
	"sort"
	"strings"

	"github.com/bobbykaz/aoc2018/utilities"
)

func Part1() {
	input := utilities.ReadFileIntoLines("input/input4.txt")
	sort.Strings(input)
	gws := make([]guardWatch, len(input))
	fmt.Println("making schedules.....")
	for i := 0; i < len(input); i++ {
		gw := processLine(input[i])
		gws = append(gws, gw)
	}
	guardMap := make(map[string]guard)
	lastGuard := ""
	lastMin := 0
	lastState := Sleep
	fmt.Println("reading schedules...")
	for i := 0; i < len(gws); i++ {
		gID, state := processAction(gws[i].Action)
		if gID != "" {
			lastGuard = gID
			//fmt.Println("Guard starting shift: ", gID)
			_, prs := guardMap[gID]
			if !prs {
				guardMap[gID] = guard{ID: gID, Sleeptime: 0, Schedule: make([]int, 60)}
			}
		}

		if lastState != state {
			if state == StartWatch {
				lastMin = gws[i].TM
				if gws[i].TH == 23 {
					lastMin = 0
				}
			}

			if state == Sleep {
				//fmt.Println(" sleeping...")
				lastMin = gws[i].TM
			}

			if state == Wake && lastState == Sleep {
				diff := gws[i].TM - lastMin
				//fmt.Println(" woke ", diff, " min later")
				g := guardMap[lastGuard]
				g.Sleeptime += diff
				for s := lastMin; s < gws[i].TM; s++ {
					g.Schedule[s]++
				}
				guardMap[lastGuard] = g

				lastMin = gws[i].TM
			}
		}

		lastState = state
	}

	lastGuard = ""
	maxSleep := 0
	maxSleepPerMin := 0
	maxSleepMin := -1
	guardIDPerMin := ""
	fmt.Println("finding sleepiest guard...")
	for _, v := range guardMap {
		//fmt.Println("  guard: ", v.ID)
		if v.Sleeptime > maxSleep {
			maxSleep = v.Sleeptime
			lastGuard = v.ID
		}
		for i, s := range v.Schedule {
			if s > maxSleepPerMin {
				maxSleepPerMin = s
				maxSleepMin = i
				guardIDPerMin = v.ID
			}
		}
	}

	fmt.Println("Guard with freq sleep / min: ", guardIDPerMin, "with ", maxSleepPerMin, " in min ", maxSleepMin)

	fmt.Println("target guard: ", lastGuard)
	g := guardMap[lastGuard]
	maxSleep = -1
	min := -1
	for s := 0; s < 60; s++ {
		//fmt.Println("min: ", s, ", ", g.Schedule[s])
		if g.Schedule[s] > maxSleep {
			maxSleep = g.Schedule[s]
			min = s
		}
	}
	fmt.Println("sleepiestMinute: ", min)
}

type guard struct {
	ID        string
	Sleeptime int
	Schedule  []int
}

type guardWatch struct {
	Y      int
	M      int
	D      int
	TH     int
	TM     int
	Action string
}

//[1518-10-28 00:01] Guard #0000 begins shift
func processLine(line string) guardWatch {
	first := strings.Split(line, "] ")
	dateTime := strings.Split(first[0], " ")
	y, m, d, _ := utilities.ParseDateStyleString(dateTime[0][1:]) //skip opening '['
	th, tm, _ := utilities.ParseTimeStyleString(dateTime[1])

	return guardWatch{Y: y, M: m, D: d, TH: th, TM: tm, Action: first[1]}
}

var StartWatch = 0
var Sleep = 1
var Wake = 2

func processAction(action string) (string, int) {
	prts := strings.Split(action, " ")
	if prts[0] == "Guard" {
		return prts[1], StartWatch
	} else if prts[0] == "wakes" {
		return "", Wake
	} else {
		return "", Sleep
	}

}
