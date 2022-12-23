use core::panic;

use crate::util;

pub fn run() {
    p1();
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
                current_pos = move_forward(&grid, &current_pos, n);
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
    println!("Score without facing: {score}");

}

fn move_forward(g: &util::grid::Grid<char>, p:&Pos, n:u32) -> Pos {
    let mut current = Pos{d: p.d.clone(), ..*p};
    for _ in 0..n {
        let mut next = current.next();
        if !is_valid_pos(g, &next) {
            next = wrap_pos(g, &next);
        }

        if is_blocked_pos(g, &next) {
            println!("blocked at {:?}", next);
            break;
        }

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

fn wrap_pos2(g: &util::grid::Grid<char>, p:&Pos) -> Pos {
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

fn is_blocked_pos(g: &util::grid::Grid<char>, p:&Pos) -> bool {
    g.g[p.r as usize][p.c as usize] == '#' 
}

fn is_valid_pos(g: &util::grid::Grid<char>, p:&Pos) -> bool {
    if p.r < 0 || p.c < 0 {
        return false;
    }

    if p.r as usize >= g.height || p.c as usize >= g.width {
        return false;
    }

    if g.g[p.r as usize][p.c as usize] == ' ' {
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
    //pre-find-replace R and L with ,R, and ,L, respectively
    let pts:Vec<String> = l.split(",").map(|s|s.to_string()).collect();
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