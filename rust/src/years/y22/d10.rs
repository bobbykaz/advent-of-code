use core::panic;
use std::collections::HashSet;

use crate::util;

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d10.txt"); 
    let cmds: Vec<CMD> = lines.iter().map(|x|parse_cmd(x)).collect();
    let mut x: i64 = 1;
    let mut cycle: i64 = 0;
    let mut signals: Vec<i64> = vec![];
    signals.push(0); //placeholder to make indexing easier

    for c in cmds {
        match c {
            CMD::Noop => {
                signals.push(x);
                cycle += 1;
                println!("{cycle}: {x}");
            },
            CMD::AddX(amt) => {
                signals.push(x);
                cycle += 1;
                println!("{cycle}: {x}");
                signals.push(x);
                cycle += 1;
                println!("{cycle}: {x}");
                x += amt;
            }
        }
    }

    let a = signals[20] * 20;
    let b = signals[60] * 60;
    let c = signals[100] * 100;
    let d = signals[140] * 140;
    let e = signals[180] * 180;
    let f = signals[220] * 220;
    println!("{a} {b} {c} {d} {e} {f}");
    let sum = a + b + c + d + e + f;
    println!("sum: {sum}");
    //20th, 60th, 100th, 140th, 180th, and 220th cycles
    let mut crt_line = String::new();
    for i in 0..6 as i64 {
        for p in 0..40 as i64 {
            let index = ((i*40) + p + 1) as usize;
            let sp = signals[index];
            if sp == p || (sp+1) == p || (sp-1) == p {
                crt_line.push('#');
                //println!("{index} : px: {p} ; sprite: {sp} -> #");
            } else {
                crt_line.push('.');
                //println!("{index} : px: {p} ; sprite: {sp} -> .");
            }
        }
        println!("{crt_line}");
        crt_line = String::new();
    }
}

fn parse_cmd(line: &String) -> CMD {
    let pts: Vec<String> = line.split(" ").map(|x|x.to_string()).collect();
    match &pts[0] as &str {
        "noop" => CMD::Noop,
        "addx" => CMD::AddX(pts[1].parse().expect("Couldnt parse addx param")),
        _ => panic!("couldnt parse dir")
    }
}

enum CMD {
    Noop,
    AddX(i64)
}

