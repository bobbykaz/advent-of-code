use core::panic;
use std::collections::HashSet;

use crate::util;

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d9.txt"); 
    let mut H = Pos {x:0,y:0};
    let mut T = Pos {x:0,y:0};
}

fn parse_line(line: String) -> (Dir, usize) {
    let pts: Vec<String> = line.split(" ").map(|x|x.to_string()).collect();
    let direction = match &pts[0] as &str {
        "U" => Dir::U,
        "D" => Dir::D,
        "L" => Dir::L,
        "R" => Dir::R,
        _ => panic!("couldnt parse dir")
    };

    let val:usize = pts[1].parse().expect("couldnt parse val");
    (direction, val)
}

fn t_pos_after_move(h: &Pos, t: &Pos, d: Dir) -> Pos {
    if *h == *t {
        return Pos {
            x: t.x,
            y: t.y
        };
    }

    panic!("TODO");
}

enum Dir {
    U,D,L,R
}

#[derive(PartialEq)]
struct Pos {
    x: i32,
    y: i32
}

