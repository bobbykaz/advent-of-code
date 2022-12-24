use core::panic;

use crate::util;

pub fn run() {
    p2();
}

pub fn p1() {
    let chunks = util::read_file_into_chunks("../input/y22/d22.txt","\n\n");
    let gl = util::read_string_into_lines(&chunks[0]);
    let il = util::read_string_into_lines(&chunks[1]);

    let mut grid = util::grid::strings_to_padded_char_grid(&gl, ' ');
    let cmds = parse_cmd(&il[0]);

    //util::grid::print_grid(&grid);

    let mut current_pos = find_start(&grid);
    println!("Start: {:?}", current_pos);
    for c in cmds {
        match c {
            C::R => {
                current_pos = current_pos.turn(C::R);
                println!("Turned Right: {:?}", current_pos);
            },
            C::L => {
                current_pos = current_pos.turn(C::L);
                println!("Turned Left: {:?}", current_pos);
            },
            C::Go(n) => {
                current_pos = move_forward(&grid, &current_pos, n, true);
                println!("Moved {n} to: {:?}", current_pos);
                grid.g[current_pos.r as usize][current_pos.c as usize] = match current_pos.d {
                    D::U => '^',
                    D::R => '>',
                    D::L => '<',
                    D::D => 'V',
                };
            },
        }
    }

    //util::grid::print_grid(&grid);

    println!("Final pos: {:?}", current_pos);
    let score = (current_pos.r + 1) * 1000 + (current_pos.c + 1)*4;
    let facing_bonus = match current_pos.d {
        D::R => 0,
        D::D => 1,
        D::L => 2,
        D::U => 3
    };
    println!("Score without facing: {score}");
    println!("Score with facing: {}", score + facing_bonus);

}

pub fn p2() {
    let chunks = util::read_file_into_chunks("../input/y22/d22.txt","\n\n");
    let gl = util::read_string_into_lines(&chunks[0]);
    let il = util::read_string_into_lines(&chunks[1]);

    let mut grid = util::grid::strings_to_padded_char_grid(&gl, ' ');
    let cmds = parse_cmd(&il[0]);

    //util::grid::print_grid(&grid);

    let mut current_pos = find_start(&grid);
    println!("Start: {:?}", current_pos);
    for c in cmds {
        match c {
            C::R => {
                current_pos = current_pos.turn(C::R);
                println!("Turned Right: {:?}", current_pos);
            },
            C::L => {
                current_pos = current_pos.turn(C::L);
                println!("Turned Left: {:?}", current_pos);
            },
            C::Go(n) => {
                println!("Moving {n} ");
                current_pos = move_forward(&grid, &current_pos, n, false);
                println!(" to: {:?}", current_pos);
                 
                grid.g[current_pos.r as usize][current_pos.c as usize] = match current_pos.d {
                    D::U => '^',
                    D::R => '>',
                    D::L => '<',
                    D::D => 'V',
                };
                
            },
        }
    }

    //util::grid::print_grid(&grid);

    println!("Final pos: {:?}", current_pos);
    let score = (current_pos.r + 1) * 1000 + (current_pos.c + 1)*4;
    let facing_bonus = match current_pos.d {
        D::R => 0,
        D::D => 1,
        D::L => 2,
        D::U => 3
    };
    println!("Score without facing: {score}");
    println!("Score with facing: {}", score + facing_bonus);

}

fn move_forward(g: &util::grid::Grid<char>, p:&Pos, n:u32, is_part_1: bool) -> Pos {
    let mut current = Pos{d: p.d.clone(), ..*p};
    for _ in 0..n {
        let mut next = current.next();
        if !is_valid_pos(g, &next) {
            if is_part_1 {
                next = wrap_pos(g, &next);
            } else {
                println!("Not Valid: {current:?} -> {next:?} ");
                next = wrap_pos_2(&current);
                println!("Wrapping to: {current:?} -> {next:?} ");
            }
        }

        if is_blocked_pos(g, &next) {
            println!(" blocked at {:?} ", next);
            return current;
        }
        println!("...{next:?}");
        current = next;
    }
    current
}

fn wrap_pos(g: &util::grid::Grid<char>, p:&Pos) -> Pos {
    match p.d {
        D::U =>{
            for r in (0..g.height).rev() {
                if g.g[r][p.c as usize] != ' ' {
                    return Pos {
                        r: r as i32,
                        d: p.d.clone(),
                        ..*p
                    };
                }
            }
            panic!("Couldnt wrap Up");
        },
        D::D =>{
            for r in 0..g.height {
                if g.g[r][p.c as usize] != ' ' {
                    return Pos {
                        r: r as i32,
                        d: p.d.clone(),
                        ..*p
                    };
                }
            }
            panic!("Couldnt wrap down");
        },
        D::R =>{
            for c in 0..g.width {
                if g.g[p.r as usize][c] != ' ' {
                    return Pos {
                        c: c as i32,
                        d: p.d.clone(),
                        ..*p
                    };
                }
            }
            panic!("Couldnt wrap right");
        },
        D::L =>{
            for c in (0..g.width).rev() {
                if g.g[p.r as usize][c] != ' ' {
                    return Pos {
                        c: c as i32,
                        d: p.d.clone(),
                        ..*p
                    };
                }
            }
            panic!("Couldnt wrap left");
        },
    }
}

/*    ____ ____     
     |   2|   1|
     |____|____|     
     |   3|
 ____|____|         
|   5|   4|
|____|____|         
|   6|
|____|              
*/
fn wrap_pos_2(p:&Pos) -> Pos {
    match p.d {
        D::U =>{
            // 1 -> 6, direction same
            if p.r == 0 && p.c >= 100 && p.c <=149 {
                return Pos {r:199, c: p.c - 100, d:D::U};
            } 
            // 2 -> 6 facing E
            else if p.r == 0 && p.c >= 50 && p.c <= 99  {
                return Pos {r:(p.c - 50) + 150, c: 0, d:D::R};
            }
            // 5 -> 3 facing E
            else if p.r == 100 && p.c <= 49  {
                return Pos {r:p.c + 50, c: 50, d:D::R};
            }
            // 6,4,3 OK
        },
        D::D =>{
            // 6 -> 1, direction same
            if p.r == 199 {
                return Pos {r:0, c: p.c + 100, d:D::D};
            }
            // 1 -> 3 facing W
            else if p.r == 49 && p.c >= 100 && p.c <= 149 {
                return Pos {r: (p.c - 100) + 50, c: 99, d:D::L};
            } 
            // 4 -> 6,facing W
            else if p.r == 149 && p.c >= 50 && p.c <= 99 {
                return Pos {r: (p.c - 50) + 150, c: 49, d:D::L};
            } 
            // 2,3,5 OK
        },
        D::R =>{
            // 1 -> 4 facing W
            if p.c == 149 {
                return Pos {r:149 - (p.r), c: 99, d:D::L};
            }
            // 3 -> 1 facing N
            else if p.c == 99 && p.r >= 50 && p.r <= 99 {
                return Pos {r:49, c: (p.r - 50) + 100, d:D::U};
            }
            // 4 -> 1 facing W
            else if p.c == 99 && p.r >= 100 && p.r <= 149 {
                return Pos {r:49 - (p.r - 100), c: 149, d:D::L};
            }
            // 6 -> 4 facing N
            else if p.c == 49 && p.r >= 150 {
                return Pos {r:149, c: 50 + (p.r - 150), d:D::U};
            }
            // 2,5 OK
        },
        D::L =>{
            // 2 -> 5 facing E
            if p.c == 50 && p.r <= 49 {
                return Pos {r:149 - (p.r), c: 0, d:D::R};
            }
            // 3 -> 5 facing S
            else if p.c == 50 && p.r <= 99 {
                return Pos {r:100, c: p.r - 50, d:D::D};
            }
            // 5 -> 2 facing E
            else if p.c == 0 && p.r <= 149 {
                return Pos {r:49 - (p.r - 100), c: 50, d:D::R};
            }
            // 6 -> 2 facing S
            else if p.c == 0 && p.r >= 150 {
                return Pos {r:0, c: 50 + (p.r - 150), d:D::D};
            }
            // 1,4 OK
        },
    }
    panic!("unknown wrap for {p:?}");
}

fn is_blocked_pos(g: &util::grid::Grid<char>, p:&Pos) -> bool {
    g.g[p.r as usize][p.c as usize] == '#' 
}

fn is_valid_pos(g: &util::grid::Grid<char>, p:&Pos) -> bool {
    if p.r < 0 || p.c < 0 {
        print!("$$");
        return false;
    }

    if p.r as usize >= g.height || p.c as usize >= g.width {
        print!("[]");
        return false;
    }

    if g.g[p.r as usize][p.c as usize] == ' ' {
        print!("X");
        return false;
    }

    return true;
}

fn find_start(g: &util::grid::Grid<char>) -> Pos {
    for i in 0..g.width {
        if g.g[0][i] == '.' {
            return Pos{
                r:0,c:i as i32, d: D::R
            };
        }
    }
    panic!("couldnt find start");
}

fn parse_cmd(l:&String) -> Vec<C> {
    let lr = l.replace("R", ",R,");
    let lr2 = lr.replace("L", ",L,");

    let pts:Vec<String> = lr2.split(",").map(|s|s.to_string()).collect();
    let mut rslt: Vec<C> = vec![];
    for pt in pts {
        match &pt as &str {
            "R" => {rslt.push(C::R)},
            "L" => {rslt.push(C::L)},
            s => {rslt.push(C::Go(s.parse().expect("Couldnt parse number in path")))}
        }
    }

    rslt
}

#[derive(Debug)]
struct Pos {
    r: i32,
    c: i32,
    d: D
}

impl Pos {
    fn turn(&self, c:C) -> Pos {
        match c {
            C::R => {
                let nd = match self.d {
                    D::U => D:: R,
                    D::R => D::D,
                    D::D => D::L,
                    D::L => D::U
                };
                Pos{d:nd, ..*self}
            },
            C::L => {
                let nd = match self.d {
                    D::U => D::L,
                    D::L => D::D,
                    D::D => D::R,
                    D::R => D::U
                };
                Pos{d:nd, ..*self}
            },
            _ => panic!("cant turn on Go")
        }
    }

    fn next(&self) -> Pos {
        let (dr,dc) = match self.d {
            D::U => (-1,0),
            D::L => (0,-1),
            D::D => (1,0),
            D::R => (0,1)
        };
        Pos { r: self.r+dr, c: self.c+dc, d: self.d.clone() }
    }
}

#[derive(Debug)]
enum C {
    R,
    L,
    Go(u32)
}

#[derive(Debug)]
enum D {
    U,
    D,
    R,
    L
}

impl D {
    fn clone(&self) -> D {
        match self {
            D::U => D::U,
            D::D => D::D,
            D::L => D::L,
            D::R => D::R,
        }
    }
}