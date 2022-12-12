use std::{vec};

use crate::util;
use crate::util::grid::{Grid, GridCell};

pub fn run() {
    p1();
    p2();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d12.txt");
    let mut hmap = util::grid::strings_to_char_grid(&lines);
    let mut visited = util::grid::new(hmap.width, hmap.height, false);
    let s = find_s(&hmap);
    hmap.g[s.row][s.col] = 'a';
    visited.g[s.row][s.col] = true;
    let e = find_e(&hmap);
    hmap.g[e.row][e.col] = 'z';
    let mut bfs: Vec<GridCell<i32>> = vec![s];
    
    loop {
        let curr = bfs.remove(0);
        //println!("processing {}: {}: {},{}", curr.v, hmap.g[curr.row][curr.col], curr.row, curr.col);
        let curr_char = hmap.g[curr.row][curr.col];
        if curr.row == e.row && curr.col == e.col {
            let moves = curr.v;
            println!("Found exit after {moves} moves");
            break;
        }
        let neighbors = util::grid::cardinal_neighbors(&hmap, curr.row, curr.col);
        //println!("found {} neighbors", neighbors.len());
        for n in neighbors {
            //println!("...checking neighbor {} {} {}", n.v, n.row,n.col);
            if !visited.g[n.row][n.col] {
                //println!("...proc neighbor {} {} {}", n.v, n.row,n.col);
                if n.v as u32 <= (curr_char as u32) + 1 {
                    visited.g[n.row][n.col] = true;
                    bfs.push(GridCell {
                        v: curr.v + 1,
                        row: n.row,
                        col: n.col
                    });
                }

            }
        }
    }
    
}

pub fn p2() {
    println!("Starting part 2");
    let lines = util::read_file_into_lines("../input/y22/d12.txt");
    let mut hmap = util::grid::strings_to_char_grid(&lines);

    let s = find_s(&hmap);
    hmap.g[s.row][s.col] = 'a';
    let e = find_e(&hmap);
    hmap.g[e.row][e.col] = 'z';

    let mut rslts:Vec<i32> =  vec![];

    for r in 0..hmap.height {
        for c in 0..hmap.width {
            if hmap.g[r][c] == 'a' { 
                let mut visited = util::grid::new(hmap.width, hmap.height, false);
                visited.g[r][c] = true;
                let mut bfs: Vec<GridCell<i32>> = vec![GridCell{v:0, row:r, col:c}];
                loop {
                    if bfs.len() == 0 {
                        println!("no exit found, redo");
                        break;
                    }
                    
                    let curr = bfs.remove(0);
                    //println!("processing {}: {}: {},{}", curr.v, hmap.g[curr.row][curr.col], curr.row, curr.col);
                    let curr_char = hmap.g[curr.row][curr.col];
                    if curr.row == e.row && curr.col == e.col {
                        let moves = curr.v;
                        println!("Found exit after {moves} moves");
                        rslts.push(moves);
                        break;
                    }
                    let neighbors = util::grid::cardinal_neighbors(&hmap, curr.row, curr.col);
                    //println!("found {} neighbors", neighbors.len());
                    for n in neighbors {
                        //println!("...checking neighbor {} {} {}", n.v, n.row,n.col);
                        if !visited.g[n.row][n.col] {
                            //println!("...proc neighbor {} {} {}", n.v, n.row,n.col);
                            if n.v as u32 <= (curr_char as u32) + 1 {
                                visited.g[n.row][n.col] = true;
                                bfs.push(GridCell {
                                    v: curr.v + 1,
                                    row: n.row,
                                    col: n.col
                                });
                            }
            
                        }
                    }
                }
            }
        }
    }
    
    rslts.sort();
    println!("all results: {rslts:?}");
    
}

fn find_s(g: &Grid<char>) -> GridCell<i32> {
    for r in 0..g.height {
        for c in 0..g.width {
            if g.g[r][c] == 'S' {
                return GridCell {
                    v: 0,
                    row: r,
                    col: c
                };
            }
        }
    }
    panic!("couldnt find grid start");
}

fn find_e(g: &Grid<char>) -> GridCell<i32> {
    for r in 0..g.height {
        for c in 0..g.width {
            if g.g[r][c] == 'E' {
                return GridCell {
                    v: 0,
                    row: r,
                    col: c
                };
            }
        }
    }
    panic!("couldnt find grid start");
}