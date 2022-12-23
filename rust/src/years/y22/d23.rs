use std::collections::{HashMap, VecDeque};

use crate::util::{self, grid::Grid};

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d23.txt");
    let og_grid = util::grid::strings_to_char_grid(&lines);

    let mut g = util::grid::pad_grid(og_grid, '.', 500);

    for r in 0..10000 {
        //util::grid::print_grid(&g);
        g = round(g, r);
        //println!("==========================");
    }

    let mut max_r = 0;
    let mut max_c = 0;
    let mut min_r = g.height;
    let mut min_c = g.width;
    for r in 0..g.height {
        for c in 0..g.width {
            if g.g[r][c] == '#' { 
                if r > max_r {max_r = r;}
                if r < min_r {min_r = r;}
                if c > max_c {max_c = c;}
                if c < min_c {min_c = c;}
            }
        }
    }

    let mut empty_count = 0;
    for r in min_r..(max_r+1) {
        for c in min_c..(max_c+1) {
            if g.g[r][c] == '.' {
                empty_count += 1;
            }
        }
    }

    println!("{empty_count}");

}

fn round(ig: Grid<char>, round:usize) -> Grid<char> {
    let mut g = ig;
    let mut mm: HashMap<String, usize> = HashMap::new();
    let mut moves: VecDeque<M> = VecDeque::new();
    //identify spot to move
    for r in 0..g.height {
        for c in 0..g.width {
            if g.g[r][c] == '#' {
                let next = get_next_spot(&g, r, c, round);
                match next {
                    Some(p) => {
                        let m = M{
                            from: P{r:r,c:c},
                            to:p
                        };
                        let key = pk(m.to.r,m.to.c);
                        if mm.contains_key(&key) {
                            mm.insert(key, 2);
                        } else {
                            mm.insert(key, 1);
                            moves.push_back(m);
                        }
                    },
                    None => {}
                };
            }
        }
    }

    let mut actual_moves = 0;
    while moves.len() > 0 {
        let nm = moves.pop_front().expect("moves is not empty");
        let num_move_to = mm.get(&pk(nm.to.r,nm.to.c)).expect("to-dest must already be in move map");
        if *num_move_to == 1 {
            g.g[nm.from.r][nm.from.c] = '.';
            g.g[nm.to.r][nm.to.c] = '#';
            actual_moves += 1;
        }
    }
    println!("Moves at round {round}: {actual_moves}");
    if actual_moves == 0 {
        panic!("No moves left!");
    }
    g
}

fn get_next_spot(g:&Grid<char>, r:usize, c:usize, round:usize) -> Option<P> {
    let d_o = dir_order(round);
    let nw = g.g[r-1][c-1];
    let n = g.g[r-1][c];
    let ne = g.g[r-1][c+1];
    let e = g.g[r][c+1];
    let se = g.g[r+1][c+1];
    let s = g.g[r+1][c];
    let sw = g.g[r+1][c-1];
    let w = g.g[r][c-1];
    
    let u_open = nw == '.' && n == '.' && ne == '.';
    let d_open = sw == '.' && s == '.' && se == '.';
    let l_open = nw == '.' && w == '.' && sw == '.';
    let r_open = ne == '.' && e == '.' && se == '.';
    let all_open = u_open && d_open && l_open && r_open;
    if all_open {
        return None;
    }

    for d in d_o {
        match d {
            D::N => {
                 if u_open {
                    return Some(P{r:r-1,c:c})
                }
            },
            D::S => {
                 if d_open {
                    return Some(P{r:r+1,c:c})
                }
            },
            D::W => {
                 if l_open {
                    return Some(P{r:r,c:c-1})
                }
            },
            D::E => {
                 if r_open {
                    return Some(P{r:r,c:c+1})
                }
            },
        }
    }

    return None;

}

fn dir_order(round:usize) -> Vec<D> {
    match round % 4 {
        0 => vec![D::N, D::S, D::W, D::E],
        1 => vec![D::S, D::W, D::E, D::N],
        2 => vec![D::W, D::E, D::N, D::S],
        3 => vec![D::E, D::N, D::S, D::W],
        _ => unreachable!()
    }
}

fn pk(r:usize,c:usize) -> String {
    format!("{r}-{c}")
}

enum D {
    N,S,E,W
}

#[derive(Copy,Clone)]
struct M {
    from: P,
    to: P
}

#[derive(Copy,Clone)]
struct P {
    r:usize,
    c:usize
}