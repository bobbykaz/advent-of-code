use std::collections::HashSet;

use crate::util;

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d6.txt");

    let p1 = find_marker(&lines[0], 4);
    println!("Part 1: {p1}");

    let p2 = find_marker(&lines[0], 14);
    println!("Part 2: {p2}");
}

fn find_marker(line: &String, marker_length: usize) -> usize {
    let mut cur: Vec<char> = vec!();
    let mut i: usize = 0;
    for c in line.chars() {
        cur.push(c);
        if cur.len() > marker_length {
            cur.remove(0);
        }

        if cur.len() == marker_length && is_unique(&cur){
            println!("{cur:?}");
            return i + 1
        }
        i+=1;
    }
    panic!("did not find start");
}

fn is_unique(chars: &Vec<char>) -> bool {
    let mut set: HashSet<&char> = HashSet::new();
    for c in chars {
        set.insert(c);
    }
    set.len() == chars.len()
}

