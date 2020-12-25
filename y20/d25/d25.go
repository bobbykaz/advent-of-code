package d25

import (
	"fmt"
)

var Print = true

func Run() int {
	//doorId := 17807724
	//cardId := 5764801
	doorId := 12090988
	cardId := 240583

	sn := 7
	value := 1
	doorloop := 0
	for value != doorId {
		value *= sn
		value = value % 20201227
		doorloop++
	}

	log("Door loopsize: %d\n", doorloop)

	cardloop := 0
	value = 1
	for value != cardId {
		value *= sn
		value = value % 20201227
		cardloop++
	}
	log("card loopsize: %d\n", cardloop)

	value = 1
	for i := 0; i < cardloop; i++ {
		value *= doorId
		value = value % 20201227
	}
	log("Encryption Key path 1: %d\n", value)
	value = 1
	for i := 0; i < doorloop; i++ {
		value *= cardId
		value = value % 20201227
	}

	log("Encryption Key path 2: %d\n", value)
	return -1
}

func log(str string, objs ...interface{}) {
	if Print {
		fmt.Printf(str, objs...)
	}
}
