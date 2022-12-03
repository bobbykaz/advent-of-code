use crate::utilities::read_file_into_lines;

pub fn run() {
    let lines = read_file_into_lines("../input/y22/d3.txt");

    let p1 = pt1(&lines);
    println!("Part 1: {p1}");

    let p2 = pt2(&lines);
    println!("Part 2: {p2}");
}

pub fn pt1(lines: &Vec<String>) -> u32 {
    let mut sum: u32 = 0;

    for i in 0..lines.len() {
        let line = &lines[i];
        let uniq_val = rucksack_uniq_item(line);
        let score = item_priority(uniq_val);
        println!("uniq item {uniq_val} -> {score}");
        sum += score;
    }

    sum
}

pub fn pt2(lines: &Vec<String>) -> u32 {
    let mut badge_sum: u32 = 0;
    let mut i = 0;
    while i < lines.len() {
        let uniq = rucksack_badge(&lines[i], &lines[i + 1], &lines[i + 2]);
        let score = item_priority(uniq);
        println!("uniq badge {uniq} -> {score}");
        badge_sum += score;
        i += 3
    }
    badge_sum
}

pub fn rucksack_uniq_item(sack: &String) -> char {
    let pts = sack.split_at(sack.len() / 2);
    let mut p1s: Vec<char> = pts.0.chars().collect();
    p1s.sort();
    let mut p2s: Vec<char> = pts.1.chars().collect();
    p2s.sort();
    for i in 0..p1s.len() {
        let cur = p1s[i];
        if p2s.contains(&cur) {
            return cur;
        }
    }
    panic!("did not find matchin item")
}

pub fn rucksack_badge(s1: &String, s2: &String, s3: &String) -> char {
    let c1: Vec<char> = s1.chars().collect();
    for c in c1 {
        if s2.contains(c) && s3.contains(c) {
            return c;
        }
    }
    panic!("common badge not found")
}

pub fn item_priority(item: char) -> u32 {
    if item >= 'a' && item <= 'z' {
        item as u32 - 'a' as u32 + 1
    } else {
        item as u32 - 'A' as u32 + 27
    }
}
