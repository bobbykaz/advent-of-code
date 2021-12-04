package d4

import (
	"fmt"
	"strconv"
	"strings"

	"github.com/bobbykaz/advent-of-code/utilities"
)

func Run() int {
	lines := utilities.ReadFileIntoLines("input/y21/d4.txt")
	//numbers := utilities.StringsToInts(lines)

	boards := make([]*BingoBoard, 0)

	bingoNums := utilities.StringToInts(lines[0])

	for i := 2; i < len(lines); i += 6 {
		fmt.Println("new board at", i)
		board := makeBoard()
		for j := 0; j < 5; j++ {
			pts := strings.Split(lines[i+j], " ")
			//fmt.Println(board)
			fmt.Println("i,", i, "j", j, "split", pts)
			pts = removeEmpties(pts)
			for k := 0; k < len(pts); k++ {
				//fmt.Println("k", k, "split", pts)
				curr, _ := strconv.Atoi(pts[k])
				board.Board[j][k] = curr
			}
		}
		boards = append(boards, board)
	}
	fmt.Println("Done setting up")
	//boards set up
	for i := 0; i < len(bingoNums); i++ {
		currentNum := bingoNums[i]
		for j := 0; j < len(boards); j++ {
			boards[j].MarkNumber(currentNum)
		}
		if i >= 4 {
			/*// First to win
			for j := 0; j < len(boards); j++ {
				if isWinningBoard(*boards[j]) {
					unm := boards[j].SumUnmarked()
					fmt.Println("Board wins after number ", currentNum, boards[j], "unmarked", unm)
					return currentNum * unm
				}
			}
			////
			*/
			// last to win
			numLosers := 0
			remainingBoards := make([]*BingoBoard, 0)
			for j := 0; j < len(boards); j++ {
				if !isWinningBoard(*boards[j]) {
					numLosers++
					remainingBoards = append(remainingBoards, boards[j])
				} else if len(boards) == 1 {
					unm := boards[j].SumUnmarked()
					fmt.Println("Last Board wins after number ", currentNum, boards[j], "unmarked", unm)
					return currentNum * unm
				}
			}
			boards = remainingBoards
			////
		}
	}

	return -1
}

func removeEmpties(strs []string) []string {
	result := make([]string, 0)
	for i := 0; i < len(strs); i++ {
		if strs[i] != "" {
			result = append(result, strs[i])
		}
	}
	return result
}

type BingoBoard struct {
	Board  [][]int
	Marked [][]bool
}

func makeBoard() *BingoBoard {
	b := make([][]int, 5)
	m := make([][]bool, 5)
	for i := 0; i < 5; i++ {
		b[i] = make([]int, 5)
		m[i] = make([]bool, 5)
	}
	return &BingoBoard{Board: b, Marked: m}
}

func (b *BingoBoard) MarkNumber(num int) {
	for i := 0; i < len(b.Board); i++ {
		for j := 0; j < len(b.Board[i]); j++ {
			if b.Board[i][j] == num {
				b.Marked[i][j] = true
				break
			}
		}
	}
}

func (b *BingoBoard) SumUnmarked() int {
	r := 0
	for i := 0; i < len(b.Board); i++ {
		for j := 0; j < len(b.Board[i]); j++ {
			if !b.Marked[i][j] {
				r += b.Board[i][j]
			}
		}
	}
	return r
}

func isWinningBoard(b BingoBoard) bool {
	//rows
	for i := 0; i < len(b.Board); i++ {
		win := true
		for j := 0; j < len(b.Board[i]); j++ {
			if !b.Marked[i][j] {
				win = false
				break
			}
		}
		if win {
			return true
		}
	}
	//columns
	for i := 0; i < len(b.Board); i++ {
		win := true
		for j := 0; j < len(b.Board[i]); j++ {
			if !b.Marked[j][i] {
				win = false
				break
			}
		}
		if win {
			return true
		}
	}
	/*
		//diags
		win := true
		for x := 0; x < len(b.Board); x++ {
			if !b.Marked[x][x] {
				win = false
				break
			}
		}
		if win {
			return true
		}
		win = true
		for x := 0; x < len(b.Board); x++ {
			if !b.Marked[x][len(b.Board)-x-1] {
				win = false
				break
			}
		}
		if win {
			return true
		}
	*/
	return false
}
