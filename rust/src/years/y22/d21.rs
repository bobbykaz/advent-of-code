use core::panic;
use std::collections::{HashMap, VecDeque};

use crate::util::{self, strings_to_ints};

use super::d7::pt1_strings;

pub fn run() {
    p2();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d21.txt");
    let monkeys_and_commands: Vec<(String, String)> = lines.iter().map(|l|init_parse(l)).collect();
    let mut monkey_index: HashMap<String,usize> = HashMap::new();
    for i in 0..monkeys_and_commands.len() {
        monkey_index.insert(monkeys_and_commands[i].0.clone(), i);
    }

    let mut mops: HashMap<usize,Mop> = HashMap::new();
    let mut comp_mop: HashMap<usize,i64> = HashMap::new();
    let mut que: VecDeque<usize> = VecDeque::new();
    for i in 0..monkeys_and_commands.len() {
        let cmd = monkeys_and_commands[i].1.clone();
        let mop = parse_mop(&cmd, &monkey_index);
        match mop {
            Mop::Val(something) => {
                println!("val: {i} -> {something}");
                comp_mop.insert(i, something);
            },
            _ => {que.push_back(i);}
        }
        mops.insert(i,mop);
    }

    while que.len() > 0  {
        let next = que.pop_front().expect("queue is empty???");
        let mop = mops.get(&next).expect("couldnt find mop???");
        match mop {
            Mop::Val(_) => {println!("somehow we found another Val???");},
            Mop::Mul(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} * {bv} = {}", av*bv);
                        comp_mop.insert(next, av * bv);
                    },
                    (_,_) => { que.push_back(next); }
                }
            },
            Mop::Div(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} / {bv} = {}", av/bv);
                        comp_mop.insert(next, av / bv);
                    },
                    (_,_) => { que.push_back(next); }
                }
            },
            Mop::Add(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} + {bv} = {}", av+bv);
                        comp_mop.insert(next, av + bv);
                    },
                    (_,_) => { que.push_back(next); }
                }
            },
            Mop::Min(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} - {bv} = {}", av-bv);
                        comp_mop.insert(next, av - bv);
                    },
                    (_,_) => { que.push_back(next); }
                }
            },
        }
    }

    let root_idx = monkey_index.get("root").expect("root must exist");
    let root_val = comp_mop.get(root_idx).expect("must have computed root val by now");

    println!("root val: {root_val}");
}

fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d21.txt");
    let monkeys_and_commands: Vec<(String, String)> = lines.iter().map(|l|init_parse(l)).collect();
    let mut monkey_index: HashMap<String,usize> = HashMap::new();
    for i in 0..monkeys_and_commands.len() {
        monkey_index.insert(monkeys_and_commands[i].0.clone(), i);
    }

    let mut mops: HashMap<usize,Mop> = HashMap::new();
    let mut comp_mop: HashMap<usize,i64> = HashMap::new();
    let mut que: VecDeque<usize> = VecDeque::new();
    for i in 0..monkeys_and_commands.len() {
        let cmd = monkeys_and_commands[i].1.clone();
        let mop = parse_mop(&cmd, &monkey_index);
        match mop {
            Mop::Val(something) => {
                println!("val: {i} -> {something}");
                comp_mop.insert(i, something);
            },
            _ => {que.push_back(i);}
        }
        mops.insert(i,mop);
    }

    let humn_idx = monkey_index.get("humn").expect("root must exist");
    comp_mop.remove(humn_idx);

    let mut failed_attempts = 0;
    while que.len() > 0 && failed_attempts < (que.len() * 2) {
        let next = que.pop_front().expect("queue is empty???");
        let mop = mops.get(&next).expect("couldnt find mop???");
        match mop {
            Mop::Val(_) => {println!("somehow we found another Val???");},
            Mop::Mul(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} * {bv} = {}", av*bv);
                        comp_mop.insert(next, av * bv);
                        failed_attempts = 0
                    },
                    (_,_) => { que.push_back(next); failed_attempts += 1;}
                }
            },
            Mop::Div(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} / {bv} = {}", av/bv);
                        comp_mop.insert(next, av / bv);
                        failed_attempts = 0
                    },
                    (_,_) => { que.push_back(next); failed_attempts += 1; }
                }
            },
            Mop::Add(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} + {bv} = {}", av+bv);
                        comp_mop.insert(next, av + bv);
                        failed_attempts = 0
                    },
                    (_,_) => { que.push_back(next); failed_attempts += 1; }
                }
            },
            Mop::Min(a,b) => {
                let av_o = comp_mop.get(a);
                let bv_o = comp_mop.get(b);
                match (av_o,bv_o) {
                    (Some(av),Some(bv)) => {
                        println!("{next} -> {av} - {bv} = {}", av-bv);
                        comp_mop.insert(next, av - bv);
                        failed_attempts = 0
                    },
                    (_,_) => { que.push_back(next); failed_attempts += 1; }
                }
            },
        }
    }

    println!("Calculated all we can, now reverse to figure out humn");

    let root_idx = monkey_index.get("root").expect("root must exist");
    let root_mop = &mops[root_idx];
    let (opt_a, opt_b) = match root_mop {
        Mop::Add(a,b) => (a,b),
        _ => panic!("shouldnt encounter a val here")
    };
    let a_o = comp_mop.get(&opt_a);
    let b_o = comp_mop.get(&opt_b);
    println!("Root has {}:{:?}| {}:{:?}", opt_a, a_o, opt_b, b_o);
    let (mut next_idx, mut known_val) = match (a_o,b_o) {
        (Some(a),None) => (*opt_b,*a),
        (None,Some(b)) => (*opt_a,*b),
        _ => panic!("found either two values or none when descending!")
    };
    println!("Root: idx {} == {}", next_idx, known_val);
    while next_idx != *humn_idx {
        let (idx,val) = descend_tree(next_idx, known_val, &mops, &comp_mop);
        next_idx = idx;
        known_val = val;
    }

    println!("humn val: {known_val}");
}

fn descend_tree(current_idx:usize, rslt_val: i64,mops: &HashMap<usize,Mop>,comp_mop: &HashMap<usize,i64>) -> (usize,i64) {
    let this_mop = &mops[&current_idx];
    let (opt_a, opt_b) = match this_mop {
        Mop::Add(a,b) => (a,b),
        Mop::Div(a,b) => (a,b),
        Mop::Mul(a,b) => (a,b),
        Mop::Min(a,b) => (a,b),
        _ => panic!("shouldnt encounter a val here")
    };
    println!("Reversing: {}: {:?} == {}", current_idx, this_mop, rslt_val);
    //figure out which value is already calculated:
    let a_o = comp_mop.get(&opt_a);
    let b_o = comp_mop.get(&opt_b);
    println!("...  {:?}:{:?}", a_o, b_o);
    let (next_idx, next_rslt) = match (a_o,b_o) {
        (Some(a),None) => {
            let modded_val = match this_mop {
                Mop::Add(_,_) => rslt_val - *a,     // a + ???? = rslt_val => ??? = rslt_val - a
                Mop::Div(_,_) => *a / rslt_val,     // a / ???? = rslt_val => a / rslt_val = ???
                Mop::Mul(_,_) => rslt_val / *a,     // a * ???? = rslt_val => rslt_val / a = ????
                Mop::Min(_,_) => *a - rslt_val,     // a - ???? = rslt_val => a - rslt_val = ???
                _ => panic!("shouldnt encounter a val here")
            };

            (opt_b,modded_val)
        },
        (None,Some(b)) => {
            let modded_val = match this_mop {
                Mop::Add(_,_) => rslt_val - *b, // ???? + b = rslt => ??? = rslt - b
                Mop::Div(_,_) => rslt_val * *b, // ???? / b  = rslt => rslt * b = ???
                Mop::Mul(_,_) => rslt_val / *b, // ???? * b  = rslt => rslt / b
                Mop::Min(_,_) => rslt_val + *b, // ???? - b  = rslt => rslt + b
                _ => panic!("shouldnt encounter a val here")
            };

            (opt_a,modded_val)
        },
        _ => panic!("found either two values or none when descending!")
    };
    
    (*next_idx,next_rslt)
}

fn init_parse(l: &String) -> (String, String) {
     let pts = util::string_split_multi(l, vec![": "]);
     (pts[0].clone(), pts[1].clone())
}

fn parse_mop(l:&String, mi: &HashMap<String,usize>) -> Mop {
    if l.chars().next().expect("must have a char").is_ascii_digit() {
        return Mop::Val(l.parse().expect("failed to parse as Val"));
    }

    if l.contains("*") {
        let pts = util::string_split_multi(l, vec![" * "]);
        let mi1 = mi.get(&pts[0]).expect("couldnt find monkey 1 idx");
        let mi2 = mi.get(&pts[1]).expect("couldnt find monkey 2 idx");
        return Mop::Mul(*mi1, *mi2);
    }

    if l.contains("+") {
        let pts = util::string_split_multi(l, vec![" + "]);
        let mi1 = mi.get(&pts[0]).expect("couldnt find monkey 1 idx");
        let mi2 = mi.get(&pts[1]).expect("couldnt find monkey 2 idx");
        return Mop::Add(*mi1, *mi2);
    }

    if l.contains("-") {
        let pts = util::string_split_multi(l, vec![" - "]);
        let mi1 = mi.get(&pts[0]).expect("couldnt find monkey 1 idx");
        let mi2 = mi.get(&pts[1]).expect("couldnt find monkey 2 idx");
        return Mop::Min(*mi1, *mi2);
    }

    if l.contains("/") {
        let pts = util::string_split_multi(l, vec![" / "]);
        let mi1 = mi.get(&pts[0]).expect("couldnt find monkey 1 idx");
        let mi2 = mi.get(&pts[1]).expect("couldnt find monkey 2 idx");
        return Mop::Div(*mi1, *mi2);
    }

    panic!("couldnt parse monkey op???")
}

#[derive(Debug)]
enum Mop {
    Add(usize,usize),
    Mul(usize,usize),
    Div(usize,usize),
    Min(usize,usize),
    Val(i64)
}