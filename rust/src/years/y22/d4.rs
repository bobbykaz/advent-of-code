use crate::utilities::read_file_into_lines;

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
    let secs: Vec<&str> = line.split(',').collect();
    let s1: Vec<&str> = secs[0].split('-').collect();
    let s2: Vec<&str> = secs[1].split('-').collect();
    let l1: i32 = s1[0].parse().expect("couldnt parse first lower bound");
    let h1: i32 = s1[1].parse().expect("couldnt parse first upper bound");
    let l2: i32 = s2[0].parse().expect("couldnt parse 2nd lower bound");
    let h2: i32 = s2[1].parse().expect("couldnt parse 2ndS upper bound");
    (l1, h1, l2, h2)
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
