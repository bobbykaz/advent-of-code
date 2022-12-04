use crate::util::{read_file_into_lines, self};

pub fn run() {
    let lines = read_file_into_lines("../input/y22/d4.txt");

    let p1 = pt1(&lines);
    println!("Part 1: {p1}");

    let p2 = pt2(&lines);
    println!("Part 2: {p2}");
}

pub fn pt1(lines: &Vec<String>) -> u32 {
    let mut sum: u32 = 0;

    for i in 0..lines.len() {
        let line = &lines[i];
        let (l1, h1, l2, h2) = parse_line(line);
        if l2 >= l1 && l2 <= h1 && h2 >= l1 && h2 <= h1 {
            println!("2nd is in 1st: {l1} - {h1} ; {l2} - {h2} : {line}");
            sum += 1
        } else if l1 >= l2 && l1 <= h2 && h1 >= l2 && h1 <= h2 {
            println!("1st is in 2nd:  {line}");
            sum += 1
        }
    }

    sum
}

pub fn parse_line(line: &String) -> (i32, i32, i32, i32) {
    let parsed = util::string_split_multi(line, ["-",",","-"].to_vec());
    let i = util::strings_to_ints(&parsed);

    (i[0], i[1], i[2], i[3])
}

pub fn pt2(lines: &Vec<String>) -> u32 {
    let mut sum: u32 = 0;

    for i in 0..lines.len() {
        let line = &lines[i];
        let (l1, h1, l2, h2) = parse_line(line);
        if (l2 <= h1 && l2 >= l1) || (l1 <= h2 && l1 >= l2) {
            println!("Overlap: {l1} - {h1} ; {l2} - {h2} : {line}");
            sum += 1
        }
    }

    sum
}
