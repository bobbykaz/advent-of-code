use crate::util;

pub fn run() {
    pt1();
    pt2();
}

pub fn pt1() {
    let lines = util::read_file_into_lines("../input/y22/d8.txt");

    let trees = util::grid::strings_to_n10_grid(&lines);
    let mut w_vis = util::grid::new(trees.width, trees.height, true);
    //l to r
    for r in 1..trees.height-1 {
        for c in 1..trees.width-1 {
            for comp in 0..c {
                if !(trees.g[r][c] > trees.g[r][comp]) {
                    w_vis.g[r][c] = false;
                    //println!("{r},{c} not visible from left");
                    break;
                }
            }
        }
    }
    let mut n_vis = util::grid::new(trees.width, trees.height, true);
    //u to d
    for c in 1..trees.width-1 {
        for r in 1..trees.height-1 {
            for comp in 0..r {
                if !(trees.g[r][c] > trees.g[comp][c]) {
                    n_vis.g[r][c] = false;
                    //println!("{r},{c} not visible from top");
                    break;
                }
            }
        }
    }
    let mut e_vis = util::grid::new(trees.width, trees.height, true);
    //r to l
    for r in 1..(trees.height-1) {
        for c in (1..(trees.width-1)).rev() {
            for comp in (c+1)..trees.width {
                if !(trees.g[r][c] > trees.g[r][comp]) {
                    e_vis.g[r][c] = false;
                    //println!("{r},{c} not visible from right");
                    break;
                }
            }
        }
    }
    let mut s_vis = util::grid::new(trees.width, trees.height, true);
    //d to u
    for c in 1..(trees.width-1) {
        for r in (1..(trees.height-1)).rev() {
            for comp in (r+1)..trees.height {
                if !(trees.g[r][c] > trees.g[comp][c]) {
                    s_vis.g[r][c] = false;
                    //println!("{r},{c} not visible from bottom");
                    break;
                }
            }
        }
    }

    //edges always visible - width + height * 2, minus double counted corners
    let mut total_visible = (trees.width * 2) + (trees.height * 2) - 4;
    for r in 1..trees.height-1 {
        for c in 1..trees.width-1 {
            if w_vis.g[r][c] ||
                n_vis.g[r][c] ||
                e_vis.g[r][c] ||
                s_vis.g[r][c] {
                    total_visible += 1;
            }
        }
    }

    println!("total visible: {total_visible}");
}

pub fn pt2() {
    let lines = util::read_file_into_lines("../input/y22/d8.txt");

    let trees = util::grid::strings_to_n10_grid(&lines);
    let mut max_score = 0;
    for r in 1..trees.height-1 {
        for c in 1..trees.width-1 {
            let current = trees.g[r][c];

            let mut t_up = 0;
            for x in (0..r).rev() {
                if current > trees.g[x][c] {
                    t_up += 1;
                } else {
                    t_up += 1;
                    break;
                }
            }

            let mut t_right = 0;
            for x in (c+1)..trees.width {
                if current > trees.g[r][x] {
                    t_right += 1;
                } else {
                    t_right += 1;
                    break;
                }
            }

            let mut t_down = 0;
            for x in r+1..trees.height {
                if current > trees.g[x][c] {
                    t_down += 1;
                } else {
                    t_down += 1;
                    break;
                }
            }

            let mut t_left = 0;
            for x in (0..c).rev() {
                if current > trees.g[r][x] {
                    t_left += 1;
                } else {
                    t_left += 1;
                    break;
                }
            }

            let score = t_up * t_down * t_left * t_right;
            if score > max_score {
                println!("New max scenic score: {score} at {r},{c} value {current}");
                println!("... from ({t_up} * {t_left} * {t_down} * {t_right})");
                max_score = score;
            }
        }
    }

    println!("best score: {max_score}");
}

