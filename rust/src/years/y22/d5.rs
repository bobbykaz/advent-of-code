use crate::util;

pub fn run() {
    let chunks = util::read_file_into_chunks("../input/y22/d5.txt","\n\n");
    let stacks = util::read_string_into_lines(&chunks[0]);
    let inst = util::read_string_into_lines(&chunks[1]);

    let p1 = pt1(&stacks, &inst);
    println!("Part 1: {p1}");

    let p2 = pt2(&stacks, &inst);
    println!("Part 2: {p2}");
}

pub fn pt1(stacks: &Vec<String>, inst: &Vec<String>) -> String {
    let mut ps = parse_stacks(&stacks);
    let pi = parse_instructions(&inst);
    for (count, src,dest) in pi {
        let src_i = src-1;
        let dest_i = dest-1;
        for _ in 0..count {
            match ps[src_i].pop() {
                Some(ch) => ps[dest_i].push(ch),
                None => panic!("empty stack found!")
            };
        }
    }

    for s in ps {
        let last_ch = s[s.len()-1];
        print!("{last_ch}");
    }
    println!("");
    "\nDone".into()
}

pub fn pt2(stacks: &Vec<String>, inst: &Vec<String>) -> String {
    let mut ps = parse_stacks(&stacks);
    let pi = parse_instructions(&inst);
    for (count, src,dest) in pi {
        let mut temp: Vec<char> = Vec::new();
        let src_i = src-1;
        let dest_i = dest-1;
        for _ in 0..count {
            match ps[src_i].pop() {
                Some(ch) => temp.push(ch),
                None => panic!("empty stack found!")
            };
        }
        temp.reverse();
        for c in temp {
            ps[dest_i].push(c);
        }
    }

    for s in ps {
        let last_ch = s[s.len()-1];
        print!("{last_ch}");
    }
    println!("");
    "\nDone".into()
}

fn parse_stacks(lines: &Vec<String>) -> Vec<Vec<char>> {
    //[Z] [G] [V] [V] [Q] [M] [L] [N] [R]
    // 1   2   3   4   5   6   7   8   9
    let mut stacks: Vec<Vec<char>> = Vec::new();
    for _ in 0..9 {
        let stack: Vec<char> = Vec::new();
        stacks.push(stack);
    }

    for l in 0..(lines.len()-1) {
        let line = &lines[l];
        let chars: Vec<char> = line.chars().collect();
        let mut i = 0;
        while i < 9 {
            let ci = 1 + (i * 4);
            if chars[ci].is_alphabetic() {
                stacks[i].push(chars[ci]);
            }
            i+=1;
        }
    } 

    for i in 0..stacks.len() {
        stacks[i].reverse();
        let st = &stacks[i];
        println!("Stack {i}: {st:?}")
    }

    stacks
}

fn parse_instructions(lines: &Vec<String>) -> Vec<(usize,usize,usize)> {
    //move 1 from 3 to 5
    let mut rslt: Vec<(usize,usize,usize)> = Vec::new();
    for line in lines {
        let mut split = util::string_split_multi(&line, ["move "," from ", " to "].to_vec());
        split.remove(0);
        let parsed = util::strings_to_ints(&split);
        rslt.push((parsed[0] as usize, parsed[1] as usize, parsed[2] as usize));
    }
    rslt
}
