use core::panic;
use std::collections::HashMap;

use crate::util;

pub fn run() {
    p1();
    p2();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d9.txt"); 
    let mut h = Pos {x:0,y:0};
    let mut t = Pos {x:0,y:0};
    let mut visits: HashMap<String, usize> = HashMap::new();
    visits.insert(t.to_string(), 1);

    for l in lines {
        let (d,v) = parse_line(l);
        for _ in 0..v {
            let old_h = h.clone();
            h = h.move_in(&d);
            if !h.neighbors(&t) {
                t = old_h;
                let k = t.to_string();
                match visits.get(&k) {
                    Some(n) => {
                        let nv = *n +1;
                        visits.insert(k, nv);
                    },
                    None => {visits.insert(k, 1);}
                };
            }
        }
    }

    let visited_places = visits.keys().count();
    println!("visited places: {visited_places}");
}

pub fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d9.txt"); 
    let mut rope: Vec<Pos> = vec![];
    for _ in 0..10 {
        rope.push(Pos {x:0,y:0})
    }
    let mut visits: HashMap<String, usize> = HashMap::new();
    visits.insert(rope[0].to_string(), 1);

    for l in lines {
        println!("{l}");
        let (d,v) = parse_line(l);
        for i in 0..v {
            //move the head, and if the next seg moves, repeat down the line
            //println!("...{i}");

            rope[0] = rope[0].move_in(&d);
            
            for r in 1..rope.len() {
                if !rope[r-1].neighbors(&rope[r]) {
                    //println!("...moving segment {r}");
                    rope[r] = rope[r].move_to(&rope[r-1]);
                    
                    if r == (rope.len() - 1) {
                        let k = rope[r].to_string();
                        match visits.get(&k) {
                            Some(n) => {
                                let nv = *n +1;
                                visits.insert(k, nv);
                            },
                            None => {visits.insert(k, 1);}
                        };
                    }
                } else {
                    //println!("...stopping at {r}");
                    break;
                }
            }
        }
    }

    let visited_places = visits.keys().count();
    println!("visited places: {visited_places}");
    for p in rope {
        let s = p.to_string();
        println!("{s}");
    }
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

enum Dir {
    U,D,L,R
}

#[derive(PartialEq)]
struct Pos {
    x: i32,
    y: i32
}

impl Pos {
    fn neighbors(&self, other: &Pos) -> bool {
        let xd = self.x - other.x;
        let yd = self.y - other.y;

        xd.abs() <= 1 && yd.abs() <= 1
    }

    fn to_string(&self) -> String {
        format!("{},{}", self.x, self.y)
    }

    fn move_in(&self, d: &Dir) -> Pos {
        match *d {
            Dir::U => Pos {x: self.x, y: self.y + 1},
            Dir::D => Pos {x: self.x, y: self.y - 1},
            Dir::L => Pos {x: self.x - 1, y: self.y},
            Dir::R => Pos {x: self.x + 1, y: self.y}
        }
    }

    fn move_to(&self, other: &Pos) -> Pos {
        let xd = other.x - self.x;
        let yd = other.y - self.y;
        let horizontal = xd != 0;
        let vertical = yd != 0;

        if horizontal &&  !vertical {
            Pos {
                x: self.x + (xd / xd.abs()),
                y: self.y
            }
        } else if vertical && !horizontal {
            Pos {
                x: self.x,
                y: self.y + (yd / yd.abs())
            }
        } else { // diagonal
            Pos {
                x: self.x + (xd / xd.abs()),
                y: self.y + (yd / yd.abs())
            }
        }
    }

    fn clone(&self) -> Pos {
        Pos { ..*self }
    }
}

