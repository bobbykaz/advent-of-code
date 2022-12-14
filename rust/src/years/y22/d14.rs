use std::fmt::{Display, self};

use crate::util::{self, grid::Grid};

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d14.txt");
    let rocks: Vec<Vec<Pos>> = lines.iter().map(|x|parse_line(x)).collect();
    let mut cave: Grid<char> = util::grid::new(700, 180, '.');

    //println!("rocks: {}",rocks.len());
    let mut max_y = 0;
    for rock_def in rocks {
        //println!("rock_def: {}", rock_def.len());
        for i in 0..(rock_def.len()-1) {
            let pts = lerp(&rock_def[i], &rock_def[i+1]);
            //println!("... lerp len: {}", pts.len());
            for pt in pts.iter()  {
                cave.g[pt.1][pt.0] = '#';
                if pt.1 > max_y {
                    max_y = pt.1;
                }
            }
        }
    }

    for i in 0..cave.width {
        cave.g[max_y + 2][i] = '#';
    }

    let mut sand_in_bounds = true;
    let mut sand_count = 0;
    while sand_in_bounds {
        let mut sand = Pos(500,0);
        let mut sand_stopped = false;
        while !sand_stopped & sand_in_bounds {
            //println!("Sand falling to {} {}", sand.0, sand.1);
            let (down, dl, dr) = possible_pos(&sand);
            //println!("{} {} {}", down, dl, dr);
            if !sand_target_in_bounds(&cave, &down) {
                sand_in_bounds = false
            } else {
                if sand_target_free(&cave, &down) {
                    sand = down;
                } else if sand_target_free(&cave, &dl) {
                    sand = dl;
                }else if sand_target_free(&cave, &dr) {
                    sand = dr;
                } else {
                    sand_stopped = true;
                }
            }
        }
        println!("Sand fell to {} {}", sand.0,sand.1);
        cave.g[sand.1][sand.0] = 'o';
        sand_count += 1;
        if sand.0 == 500 && sand.1 == 0 {
            break;
        }
    }

    println!("sand count: {sand_count}");
    util::grid::print_grid(&cave);
}

fn sand_target_in_bounds(g:&Grid<char>, p: &Pos) -> bool {
    return p.1 < g.height;
}

fn sand_target_free(g:&Grid<char>, p: &Pos) -> bool {
    let ch = g.g[p.1][p.0];
    //println!("{ch} at {p}");
    ch == '.'
}

fn possible_pos(sand:&Pos) -> (Pos, Pos, Pos) {
    (Pos(sand.0,sand.1+1),Pos(sand.0-1,sand.1+1),Pos(sand.0+1,sand.1+1))
}

fn parse_line(l: &String) -> Vec<Pos>{
    let mut rslt = vec![];
    let pts = l.split(" -> ").into_iter();

    for pt in pts{
        let nums = util::string_to_ints(&pt.to_string(), ",");
        rslt.push(Pos(nums[0] as usize, nums[1] as usize));
    }

    return rslt;
}

fn lerp(p1: &Pos, p2: &Pos) -> Vec<Pos> {
    //println!("lerping {} {} to {} {}",p1.0, p1.1, p2.0, p2.1);
    if p1.0 == p2.0 {
        //println!("lerping y");
        return lerp_y(p1, p2)
    } else {
        //println!("lerping x");
        return lerp_x(p1,p2)
    }
}

fn lerp_x(p1: &Pos, p2: &Pos) -> Vec<Pos> {
    if p1.0 > p2.0 {
        return lerp_x(p2, p1);
    }
    
    let mut rslt = vec![];

    for i in p1.0..(p2.0+1) {
        //println!("{i}");
        rslt.push(Pos(i,p1.1))
    }

    return rslt;
}

fn lerp_y(p1: &Pos, p2: &Pos) -> Vec<Pos> {
    if p1.1 > p2.1 {
        return lerp_y(p2, p1);
    }
    
    let mut rslt = vec![];

    for i in p1.1..(p2.1+1) {
        //println!("{i}");
        rslt.push(Pos(p1.0, i))
    }

    return rslt;
}

struct Pos(usize,usize);

impl Display for Pos {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "({}, {})", self.0, self.1)
    }
}