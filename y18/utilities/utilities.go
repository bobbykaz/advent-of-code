package utilities

import "strings"
import "strconv"
import "io/ioutil"
import "fmt"

func ReadFileIntoLines(filename string) []string {
	b, err := ioutil.ReadFile(filename)
	if err != nil {
		panic(err)
	}
	file := string(b)
	lines := strings.Split(file, "\n")
	if lines[len(lines)-1] == "" {
		lines = lines[:(len(lines) - 1)]
	}
	return lines
}

func StringsToInts(strs []string) []int {
	var output = make([]int, len(strs))
	for i, s := range strs {
		temp, err := strconv.Atoi(s)
		if err != nil {
			panic(err)
		}
		output[i] = temp
	}

	return output
}

func IntsToString(input []int) string {
	var s string = fmt.Sprintf("%d", input[0])
	for _, item := range input[1:] {
		s = fmt.Sprintf("%s,%d", s, item)
	}

	return s
}

//ParseCoord parses an X,Y style coordinate and also removes the beginning and end strings provided, and trims spaces everywhere
func ParseCoord(coordStr, beginning, separator, end string) (int, int, error) {
	trimmed := strings.TrimSpace(coordStr)
	trimmed = strings.TrimPrefix(trimmed, beginning)
	trimmed = strings.TrimSpace(trimmed)
	trimmed = strings.TrimSuffix(trimmed, end)
	trimmed = strings.TrimSpace(trimmed)
	parts := strings.Split(trimmed, separator)
	x, err := strconv.Atoi(strings.TrimSpace(parts[0]))
	if err != nil {
		return -1, -1, err
	}

	y, err := strconv.Atoi(strings.TrimSpace(parts[1]))
	if err != nil {
		return -1, -1, err
	}

	return x, y, nil
}

//ParseDateStyleString expects string to be equivalent to "2000-10-20". Trims spaces.
func ParseDateStyleString(date string) (int, int, int, error) {
	trimmed := strings.TrimSpace(date)
	parts := strings.Split(trimmed, "-")
	if len(parts) != 3 {
		return -1, -1, -1, fmt.Errorf("expected 3 parts to date %s", trimmed)
	}

	y, err := strconv.Atoi(parts[0])
	if err != nil {
		return -1, -1, -1, err
	}

	m, err := strconv.Atoi(parts[1])
	if err != nil {
		return -1, -1, -1, err
	}

	d, err := strconv.Atoi(parts[2])
	if err != nil {
		return -1, -1, -1, err
	}

	return y, m, d, nil
}

//ParseTimeStyleString expects string to be equivalent to "00:11". Trims spaces.
func ParseTimeStyleString(time string) (int, int, error) {
	trimmed := strings.TrimSpace(time)
	parts := strings.Split(trimmed, ":")
	if len(parts) != 2 {
		return -1, -1, fmt.Errorf("expected 2 parts to date %s", trimmed)
	}

	h, err := strconv.Atoi(parts[0])
	if err != nil {
		return -1, -1, err
	}

	m, err := strconv.Atoi(parts[1])
	if err != nil {
		return -1, -1, err
	}

	return h, m, nil
}
