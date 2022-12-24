use std::{collections::{HashMap, VecDeque}, fmt};

use crate::util::{self, grid::Grid};

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d24.txt");
    
    let g = util::grid::strings_to_char_grid(&lines);
    let mut gs: Vec<Grid<Cell>> = vec![];
    gs.push(input_to_init_grid(g));
    //util::grid::print_grid(&gs[0]);
    
    let mut goals: Vec<(usize,usize)> = vec![];
    let (exit_r,exit_c) = (gs[0].height - 1, gs[0].width - 2);
    goals.push((exit_r,exit_c));
    goals.push((0,1));
    goals.push((exit_r,exit_c));
    let mut goal_idx = 0;
    
    let mut seen: HashMap<String,bool> = HashMap::new();
    let mut que: VecDeque<P> = VecDeque::new();
    let start = P {r:0, c:1, t:0};
    seen.insert(start.key(), true);
    que.push_back(start);
    
    while que.len() > 0 {
        let next = que.pop_front().expect("queue must have one item");
        //println!("processing: {next:?}");
        if next.r == goals[goal_idx].0 && next.c == goals[goal_idx].1 {
            println!("target found! {next:?}");
            que.clear();
            seen.clear();
            goal_idx += 1;
            if goal_idx == 3 {
                return ();
            }
        }
        let current_time = next.t + 1;
        while current_time >= gs.len() {
            let ng = step_grid(&gs[gs.len()-1]);
            //println!("==========");
            //util::grid::print_grid(&ng);
            gs.push(ng);
        }

        let ng = &gs[current_time];

        let wait_p = P {r:next.r, c: next.c, t: current_time};
        if !seen.contains_key(&wait_p.key()) && ng.g[wait_p.r][wait_p.c].is_empty() {
            seen.insert(wait_p.key(), true);
            que.push_back(wait_p);
        }

        let neighbors = util::grid::cardinal_neighbors(ng, next.r, next.c);
        for neighbor in neighbors {
            let pos = P {r:neighbor.row, c:neighbor.col, t:current_time};
            if neighbor.v.is_empty() && !seen.contains_key(&pos.key()) {
                seen.insert(pos.key(), true);
                que.push_back(pos);
            }
        }
    }
}

fn input_to_init_grid(g: util::grid::Grid<char>) -> util::grid::Grid<Cell> {
    let mut rslt = util::grid::new(g.width, g.height, Cell::new());
    for r in 0..g.height {
        for c in 0..g.width {
            match g.g[r][c] {
                '.' => {},
                '>' => {rslt.g[r][c].r = true;},
                'v' => {rslt.g[r][c].d = true;},
                '^' => {rslt.g[r][c].u = true;},
                '<' => {rslt.g[r][c].l = true;},
                '#' => {rslt.g[r][c].w = true;},
                _ => {unreachable!()}
            }
        }
    }
    rslt
}

fn step_grid(g: &util::grid::Grid<Cell>) -> util::grid::Grid<Cell> {
    let mut rslt = util::grid::new(g.width, g.height, Cell::new());
    //copy borders
    for r in 0..g.height {
        rslt.g[r][0] = g.g[r][0].clone();
        rslt.g[r][g.width-1] = g.g[r][g.width-1].clone();
    }

    for c in 0..g.width {
        rslt.g[0][c] = g.g[0][c];
        rslt.g[g.height-1][c] = g.g[g.height-1][c];
    }

    for r in 1..g.height-1 {
        for c in 1..g.width-1 {
            if g.g[r][c].l {
                let (nr,nc) = get_left(g, r, c);
                rslt.g[nr][nc].l = true;
            }
            if g.g[r][c].r {
                let (nr,nc) = get_right(g, r, c);
                rslt.g[nr][nc].r = true;
            }
            if g.g[r][c].u {
                let (nr,nc) = get_up(g, r, c);
                rslt.g[nr][nc].u = true;
            }
            if g.g[r][c].d {
                let (nr,nc) = get_down(g, r, c);
                rslt.g[nr][nc].d = true;
            }
        }
    }

    rslt
}

fn get_left(g: &util::grid::Grid<Cell>, r: usize,c: usize) -> (usize,usize) {
    if c > 1 {
        (r,c-1)
    } else {
        (r, g.width -2)
    }
}

fn get_right(g: &util::grid::Grid<Cell>, r: usize,c: usize) -> (usize,usize) {
    if c < g.width - 2 {
        (r,c+1)
    } else {
        (r, 1)
    }
}

fn get_up(g: &util::grid::Grid<Cell>, r: usize,c: usize) -> (usize,usize) {
    if r > 1 {
        (r-1,c)
    } else {
        (g.height-2, c)
    }
}

fn get_down(g: &util::grid::Grid<Cell>, r: usize,c: usize) -> (usize,usize) {
    if r < g.height - 2 {
        (r+1,c)
    } else {
        (1, c)
    }
}

#[derive(Eq,PartialEq,Copy,Clone)]
struct Cell {
    u:bool,
    d:bool,
    l:bool,
    r:bool,
    w:bool
}

impl fmt::Display for Cell {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        if self.is_empty() {return write!(f, ".");}
        if self.w {return write!(f, "#");}
        if self.u && !self.l && !self.d && !self.r{
            return write!(f, "^");
        }
        if !self.u && self.l && !self.d && !self.r{
            return write!(f, "<");
        }
        if !self.u && !self.l && self.d && !self.r{
            return write!(f, "v");
        }
        if !self.u && !self.l && !self.d && self.r{
            return write!(f, ">");
        }
        
        return write!(f, "?");
    }
}

impl Cell {
    fn new() -> Cell {
        Cell { u: false, d: false, l: false, r: false, w: false }
    }

    fn is_empty(&self) -> bool {
        !self.u && !self.d && !self.l && !self.r && !self.w
    }
}

#[derive(Eq,PartialEq,Copy,Clone,Debug)]
struct P {
    r:usize,
    c:usize,
    t:usize
}

impl P {
    fn key(&self) -> String {
        format!("{}-{}-{}",self.t,self.r,self.c)
    }
}