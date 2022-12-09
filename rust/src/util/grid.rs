use std::fmt::Display;

 pub struct Grid<T> {
    pub g: Vec<Vec<T>>,
    pub width: usize,
    pub height: usize
 }

 pub struct GridCell<T> {
    pub v: T,
    pub row: usize,
    pub col: usize
 }

 pub fn strings_to_char_grid(input: &Vec<String>) -> Grid<char> {
    let h = input.len();
    let w = input[0].chars().count();

    let mut grslt: Vec<Vec<char>> = Vec::new();
    for l in input {
        grslt.push(l.chars().collect())
    }
    Grid {
        g: grslt,
        height: h,
        width: w
    }
 }

 pub fn strings_to_n10_grid(input: &Vec<String>) -> Grid<i32> {
    let h = input.len();
    let w = input[0].chars().count();

    let mut grslt: Vec<Vec<i32>> = vec![];
    for l in input {
        let mut row : Vec<i32> = vec![];
        for c in l.chars() {
            let n = c.to_digit(10)
            .expect("failed to parse character") as i32;
            row.push(n);
        }
        grslt.push(row);
    }
    Grid {
        g: grslt,
        height: h,
        width: w
    }
 }

 pub fn new<T:Copy>(w:usize, h:usize, default_val:T) -> Grid<T> {

    let mut grslt: Vec<Vec<T>> = vec![];
    for _ in 0..h {
        let mut row : Vec<T> = vec![];
        for _ in 0..w {
            row.push(default_val);
        }
        grslt.push(row);
    }
    Grid {
        g: grslt,
        height: h,
        width: w
    }
 }

 pub fn print_grid<T:Display>(g: &Grid<T>) {
    for row in &g.g {
        for i in row {
            print!("{i} ");
        }
        println!("");
    }
 }